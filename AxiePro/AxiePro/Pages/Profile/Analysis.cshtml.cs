using AxiePro.Models.Battles;
using AxiePro.Models.Reports;
using AxiePro.Reports;
using AxiePro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AxiePro.Models.Battles.MmrRequest;

namespace AxiePro.Pages.Profile
{
    public class AnalysisModel : PageModel
    {
        private readonly IAxieApiService _axieApiService;
        private readonly IAxieDataFactory _axieDataFactory;
        private readonly IPaymentProcessService _processService;

        private readonly IBattlesReportsService _battlesReportService;

        public AnalysisModel(IAxieApiService axieApiService, IAxieDataFactory axieDataFactory, IBattlesReportsService battlesReportService, IPaymentProcessService processService)
        {
            _axieApiService = axieApiService;
            _axieDataFactory = axieDataFactory;
            _battlesReportService = battlesReportService;
            _processService = processService;
        }
        [BindProperty]
        public List<BattleHistoryReport> BattlesDataSource { get; set; }
        public List<IGrouping<string, BattleHistoryReport>>  FavouriteTeams { get; set; }
        public List<IGrouping<string, BattleHistoryReport>> Opponents { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public MmrPlayer PlayerMmr { get; set; }
        public ProxyMmr PlayerMmrProxy { get; set; }
        public WinRecord WinRecord { get; set; }
        public bool IsMember { get; set; }
        public async Task OnGet(string roninId)
        {
            if (String.IsNullOrEmpty(roninId))
                roninId = SearchString;
            if (String.IsNullOrEmpty(roninId))
                return;
            if(roninId.Substring(0,1) == "r")
                 roninId = "0x" + roninId.Substring(6);
            else
            if (roninId.Substring(0, 1) != "0")
                return;
            // await _axieDataFactory.AddAllAxies();
            var model = await _axieApiService.GetBattlePvp(roninId);
             BattlesDataSource = await _battlesReportService.GetBattleHistory(roninId);
            PlayerMmrProxy = await _axieApiService.GetMmrProxy(roninId);
            FavouriteTeams = BattlesDataSource.GroupBy(a => a.BuildHash).OrderByDescending(a=>a.Count()).ToList();
            PlayerMmr = await _axieApiService.GetMmr(roninId);
            WinRecord = new WinRecord();
            WinRecord.Wins = BattlesDataSource.Count(a => a.Result == "Win");
            WinRecord.Losses = BattlesDataSource.Count(a => a.Result == "Lose");
            var winRate =((decimal)WinRecord.Wins / (decimal)(WinRecord.Wins + WinRecord.Losses))*100;
            WinRecord.WinRate = Decimal.ToInt32(winRate);
            Opponents = BattlesDataSource.GroupBy(a => a.OpponentClassHash).OrderByDescending(a=>a.Count()).ToList();
            IsMember = await _processService.HasValidMembership(roninId);
        }
        public async Task OnPost(string roninId)
        {
            if (String.IsNullOrEmpty(roninId))
                roninId = SearchString;
            if (String.IsNullOrEmpty(roninId))
                return;
            if (roninId.Substring(0, 1) == "r")
                roninId = "0x" + roninId.Substring(6);
            else
                    if (roninId.Substring(0, 1) != "0")
                return;
            // await _axieDataFactory.AddAllAxies();
           var model = await _axieApiService.GetBattlePvp(roninId);
            BattlesDataSource = await _battlesReportService.GetBattleHistory(roninId);
            PlayerMmrProxy = await _axieApiService.GetMmrProxy(roninId);
            FavouriteTeams = BattlesDataSource.GroupBy(a => a.BuildHash).OrderByDescending(a => a.Count()).ToList();
            PlayerMmr = await _axieApiService.GetMmr(roninId);
            WinRecord = new WinRecord();
            WinRecord.Wins = BattlesDataSource.Count(a => a.Result == "Win");
            WinRecord.Losses = BattlesDataSource.Count(a => a.Result == "Lose");
            var winRate = ((decimal)WinRecord.Wins / (decimal)(WinRecord.Wins + WinRecord.Losses)) * 100;
            WinRecord.WinRate = Decimal.ToInt32(winRate);
            Opponents = BattlesDataSource.GroupBy(a => a.OpponentClassHash).OrderByDescending(a => a.Count()).ToList();
            IsMember = await _processService.HasValidMembership(roninId);
        }
    }
}
