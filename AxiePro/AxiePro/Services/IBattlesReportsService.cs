using AxiePro.Models.Axies;
using AxiePro.Models.Reports;
using Microsoft.EntityFrameworkCore;

namespace AxiePro.Services
{
    public interface IBattlesReportsService
    {
        public Task<List<BattleHistoryReport>> GetBattleHistoryByBuild(string BuildId);
        public Task<List<BattleHistoryReport>> GetBattleHistory(string roninUid);
        public Task<List<IGrouping<string, BattleHistoryReport>>> GetRecentTeams(string roninUid);
        public  Task<List<FavouriteTeam>> LeaderBoardTeams();
        public Task<List<AxieTeam>> GetRecentOpponents(string roninUid);
        public Task<BuildTeam> GetBuild(string BuildId);
    }
    public class BattlesReportsService : IBattlesReportsService
    {

        private readonly DataContext _dataContext;
        public BattlesReportsService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<BattleHistoryReport>> GetBattleHistory(string roninUid)
        {
    
            var battleHistory = await GetBattleHistoryQuery(roninUid).ToListAsync();
            return battleHistory;
        }
        public IQueryable<BattleHistoryReport> GetBattleHistoryQuery(string roninParse)
        {
            //var roninParse = "0x" + roninUid.Substring(6);
            var battleHistoryQuery = from b in _dataContext.BattleHistory
                                     where b.WinnerUserUid == roninParse ||
                                     b.LoserUserUid == roninParse
                                     join wat in _dataContext.AxieTeam
                                     on b.WinnerTeamHash equals wat.TeamHash
                                     join lat in _dataContext.AxieTeam
                                     on b.LoserTeamHash equals lat.TeamHash
                                     orderby b.StartDateTime descending
                                     select new BattleHistoryReport
                                     {
                                         BattleDateTime = b.StartDateTime,
                                         BattleUid = b.BattleUid,
                                         FirstOpponentFighter = roninParse != b.WinnerUserUid ? wat.FirstFighter : lat.FirstFighter,
                                         SecondOpponentFighter = roninParse != b.WinnerUserUid ? wat.SecondFighter : lat.SecondFighter,
                                         ThirdOpponentFighter = roninParse != b.WinnerUserUid ? wat.ThirdFighter : lat.ThirdFighter,
                                         FirstUserFighter = roninParse == b.WinnerUserUid ? wat.FirstFighter : lat.FirstFighter,
                                         SecondUserFighter = roninParse == b.WinnerUserUid ? wat.SecondFighter : lat.SecondFighter,
                                         ThirdUserFighter = roninParse == b.WinnerUserUid ? wat.ThirdFighter : lat.ThirdFighter,
                                         OpponentUid = roninParse != b.WinnerUserUid ? b.WinnerUserUid : b.LoserUserUid,
                                         OpponentClassHash = roninParse != b.WinnerUserUid ? wat.ClassHash :lat.ClassHash,
                                         Result = roninParse != b.WinnerUserUid ? "Lose" : "Win",
                                         TimeAgo = TimeAgo(b.EndDateTime),
                                         BuildHash = roninParse != b.WinnerUserUid ? lat.BuildHash : wat.BuildHash,
                                         OpponentBuildHash = roninParse != b.WinnerUserUid ? wat.BuildHash : lat.BuildHash,
                                         Draw = b.Draw

                                     };
           
            return battleHistoryQuery;
        }
        public static string TimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.UtcNow.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "a day ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }

        public async Task<List<IGrouping<string,BattleHistoryReport>>> GetRecentTeams(string roninUid)
        {
            var roninParse = "0x" + roninUid.Substring(6);
            var battleQuery =  GetBattleHistoryQuery(roninParse).Distinct();

            var favTeamQuery = from at in _dataContext.AxieTeam
                          join bq in battleQuery
                          on at.BuildHash equals bq.BuildHash
                          group bq by bq.BuildHash into newgroup
                          select newgroup;
                 
            var favTeam = await favTeamQuery.ToListAsync();
            return favTeam;

        }

        public Task<List<AxieTeam>> GetRecentOpponents(string roninUid)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FavouriteTeam>> LeaderBoardTeams()
        {
            var battleWinners = from att in _dataContext.AxieTeam
                                join b in _dataContext.BattleHistory
                                on att.TeamHash equals b.WinnerTeamHash
                                where b.LeaderboardGame == true && b.EndDateTime > DateTime.Now.AddDays(-3)
                                group b by att.BuildHash into g
                                select new
                                {
                                    g.Key,
                                    Wins = g.Count(),
                                    Losses = 0

                                };

            var battleLossers = from att in _dataContext.AxieTeam
                                join b in _dataContext.BattleHistory
                                on att.TeamHash equals b.LoserTeamHash
                                where b.LeaderboardGame == true && b.EndDateTime > DateTime.Now.AddDays(-3)
                                group b by att.BuildHash into g
                                select new
                                {
                                    g.Key,
                                    Wins = 0,
                                    Losses = g.Count()

                                };
            var unionBattle = (from w in battleWinners select w).Union(battleLossers).OrderByDescending(a => a.Wins).GroupBy(a => a.Key)
                .Select(a => new
                {
                    BuildHash = a.Key,
                    Wins = a.Sum(a => a.Wins),
                    Losses = a.Sum(a => a.Losses),

                });

            var finalQuery = from a in   _dataContext.AxieTeam
                                        
                        join bh in unionBattle on a.BuildHash equals bh.BuildHash
                        orderby bh.Wins descending

                             select new FavouriteTeam
                             {
                                 BuildUid = a.BuildHash,
                                 FirstUserFighter = a.FirstFighter,
                                 SecondUserFighter = a.SecondFighter,
                                 ThirdUserFighter = a.ThirdFighter,
                                 Losses = bh.Losses,
                                 Wins = bh.Wins
                             };
            var model = await finalQuery.GroupBy(a => a.BuildUid, (key, g) => g.First()).ToListAsync();
            model = model.OrderByDescending(a => a.Wins).Take(50).ToList();
            return model;
        }

        public async Task<List<BattleHistoryReport>> GetBattleHistoryByBuild(string buildId)
        {

            //var roninParse = "0x" + roninUid.Substring(6);
            var battleHistoryQuery = from b in _dataContext.BattleHistory
                                    
                                     join wat in _dataContext.AxieTeam
                                     on b.WinnerTeamHash equals wat.TeamHash
                                     join lat in _dataContext.AxieTeam
                                     on b.LoserTeamHash equals lat.TeamHash
                                     where lat.BuildHash == buildId ||
                                     wat.BuildHash == buildId
                                     orderby b.StartDateTime descending
                                     select new BattleHistoryReport
                                     {
                                         BattleDateTime = b.StartDateTime,
                                         BattleUid = b.BattleUid,
                                         FirstOpponentFighter = buildId != wat.BuildHash ? wat.FirstFighter : lat.FirstFighter,
                                         SecondOpponentFighter = buildId != wat.BuildHash ? wat.SecondFighter : lat.SecondFighter,
                                         ThirdOpponentFighter = buildId != wat.BuildHash ? wat.ThirdFighter : lat.ThirdFighter,
                                         FirstUserFighter = buildId == wat.BuildHash ? wat.FirstFighter : lat.FirstFighter,
                                         SecondUserFighter = buildId == wat.BuildHash ? wat.SecondFighter : lat.SecondFighter,
                                         ThirdUserFighter = buildId == wat.BuildHash ? wat.ThirdFighter : lat.ThirdFighter,
                                         OpponentUid = buildId != wat.BuildHash ? b.WinnerUserUid : b.LoserUserUid,
                                         OpponentClassHash = buildId != wat.BuildHash ? wat.ClassHash : lat.ClassHash,
                                         Result = buildId != wat.BuildHash ? "Lose" : "Win",
                                         TimeAgo = TimeAgo(b.StartDateTime),
                                         BuildHash = buildId != wat.BuildHash ? lat.BuildHash : wat.BuildHash,
                                         OpponentBuildHash = buildId !=wat.BuildHash ? wat.BuildHash : lat.BuildHash,
                                         UserUid = buildId == wat.BuildHash ? b.WinnerUserUid : b.LoserUserUid,
                                         Draw = b.Draw

                                     };
            var model = await battleHistoryQuery.Where(a=>a.BattleDateTime >= DateTime.Now.AddDays(-3)).ToListAsync();
            return model;
        }

        public async Task<BuildTeam> GetBuild(string buildHash)
        {
            var build = await _dataContext.AxieTeam.Where(a => a.BuildHash == buildHash).Select( a => new BuildTeam
            {
                BuildHash = a.BuildHash,
                FirstAxie = a.FirstFighter,
                SecondAxie = a.SecondFighter,
                ThirdAxie = a.ThirdFighter
                


            }).FirstOrDefaultAsync();
            return build;
        }
    }
}
