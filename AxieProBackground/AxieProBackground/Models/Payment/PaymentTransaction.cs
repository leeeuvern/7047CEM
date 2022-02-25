namespace AxiePro.Models.Payment
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public string PaymentTransactionUid { get; set; }
        public string PayeeAddressUid { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public int offset { get; set; }
        public int Amount { get; set; }
    }
}
