using AxiePro.Models.Payment;
using Microsoft.EntityFrameworkCore;

namespace AxiePro.Services
{
    public interface IPaymentProcessService
    {
        public Task<List<PaymentTransaction>> GetPaymentList(string PayeeAddressUid);
        public Task<Boolean> HasValidMembership(string address);
        public Task<List<Membership>> GetMemberList();
    }
    public class PaymentProcessService : IPaymentProcessService
    {
        private readonly DataContext _dataContext;
        public PaymentProcessService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<PaymentTransaction>> GetPaymentList(string PayeeAddressUid)
        {
            var model = await _dataContext.PaymentTransaction.Where(a => a.PayeeAddressUid == PayeeAddressUid).ToListAsync();
            return model;
        }
        public async Task<List<Membership>> GetMemberList()
        {
            var model = await _dataContext.Membership.Where(a =>a.MembershipEndDate >= DateTime.Now).ToListAsync();
            return model;
        }
        public async Task<bool> HasValidMembership(string address)
        {
            var hasSuscribed = await _dataContext.Membership.Where(a => a.MemberUid == address && a.MembershipEndDate >= DateTime.Now).FirstOrDefaultAsync();
            if (hasSuscribed != null)
                return true;
            else
                return false;
        }
    }
}
