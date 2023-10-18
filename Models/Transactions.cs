namespace AlifTestTask.Models
{
    public class Transactions
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int TranType { get; set;}
        public int Status { get; set; }
        public string Error { get; set; }
        public int ErrorStatusCode { get; set; }
        public DateTime TranDate { get; set; }
    }
}
