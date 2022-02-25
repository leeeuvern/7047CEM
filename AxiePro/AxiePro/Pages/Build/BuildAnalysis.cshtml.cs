using AxiePro.Models.Battles;
using AxiePro.Models.Reports;
using AxiePro.Reports;
using AxiePro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AxiePro.Models.Battles.MmrRequest;

namespace AxiePro.Pages.Build
{
    public class AnalysisModel : PageModel
    {
        private readonly IAxieApiService _axieApiService;
        private readonly IAxieDataFactory _axieDataFactory;

        private readonly IBattlesReportsService _battlesReportService;

        public AnalysisModel(IAxieApiService axieApiService, IAxieDataFactory axieDataFactory, IBattlesReportsService battlesReportService)
        {
            _axieApiService = axieApiService;
            _axieDataFactory = axieDataFactory;
            _battlesReportService = battlesReportService;
        }
        [BindProperty]
        public List<BattleHistoryReport> BattlesDataSource { get; set; }
    //    public List<IGrouping<string, BattleHistoryReport>> FavouriteTeams { get; set; }
        public List<IGrouping<string, BattleHistoryReport>> Opponents { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
         public BuildTeam Build { get; set; }
        public WinRecord WinRecord { get; set; }
        public async Task OnGet(string buildId)
        {

            BattlesDataSource = await _battlesReportService.GetBattleHistoryByBuild(buildId);
            //PlayerMmrProxy = await _axieApiService.GetMmrProxy(roninId);
         //   FavouriteTeams = BattlesDataSource.GroupBy(a => a.BuildHash).OrderByDescending(a => a.Count()).ToList();
         //   PlayerMmr = await _axieApiService.GetMmr(roninId);
            WinRecord = new WinRecord();
            WinRecord.Wins = BattlesDataSource.Count(a => a.Result == "Win");
            WinRecord.Losses = BattlesDataSource.Count(a => a.Result == "Lose");
            var winRate = ((decimal)WinRecord.Wins / (decimal)(WinRecord.Wins + WinRecord.Losses)) * 100;
            WinRecord.WinRate = Decimal.ToInt32(winRate);
            Opponents = BattlesDataSource.GroupBy(a => a.OpponentClassHash).OrderByDescending(a => a.Count()).ToList();
            Build = await _battlesReportService.GetBuild(buildId);
        }
        public async Task OnPost(string buildId)
        {



            BattlesDataSource = await _battlesReportService.GetBattleHistoryByBuild(buildId);
            //PlayerMmrProxy = await _axieApiService.GetMmrProxy(roninId);
            //   FavouriteTeams = BattlesDataSource.GroupBy(a => a.BuildHash).OrderByDescending(a => a.Count()).ToList();
            //   PlayerMmr = await _axieApiService.GetMmr(roninId);
            WinRecord = new WinRecord();
            WinRecord.Wins = BattlesDataSource.Count(a => a.Result == "Win");
            WinRecord.Losses = BattlesDataSource.Count(a => a.Result == "Lose");
            var winRate = ((decimal)WinRecord.Wins / (decimal)(WinRecord.Wins + WinRecord.Losses)) * 100;
            WinRecord.WinRate = Decimal.ToInt32(winRate);
            Opponents = BattlesDataSource.GroupBy(a => a.OpponentClassHash).OrderByDescending(a => a.Count()).ToList();
            Build = await _battlesReportService.GetBuild(buildId);
        }
    }
}
