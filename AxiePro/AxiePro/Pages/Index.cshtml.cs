using AxiePro.Models.Reports;
using AxiePro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AxiePro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBattlesReportsService _battleReportService;
        public IndexModel(ILogger<IndexModel> logger, IBattlesReportsService battleReportService)
        {
            _logger = logger;
            _battleReportService = battleReportService;
        }
        [BindProperty]
        public List<FavouriteTeam> FavouriteTeams { get; set; }
        public async Task OnGet()
        {
            FavouriteTeams = await _battleReportService.LeaderBoardTeams();
        }
    }
}