namespace AxiePro.Models.Payment
{
    public class Membership
    {
        public int Id { get; set; }
        public DateTime MembershipStartDate { get; set; }
         public DateTime MembershipEndDate { get; set; }
        public string MemberUid { get; set; }
        public string TransactionUid { get; set; }
    }
}
