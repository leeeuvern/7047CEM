using AxiePro.Models.Payment;
using AxiePro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AxiePro.Pages.Member
{
    public class ListModel : PageModel
    {
        private readonly IPaymentProcessService _processService;

        [BindProperty]
        public List<Membership> MembershipDataSource { get; set; }
        public ListModel(IPaymentProcessService processService)
        {
            _processService = processService;
        }
        public async Task OnGet()
        {
            MembershipDataSource = await _processService.GetMemberList();
        }
    }
}
