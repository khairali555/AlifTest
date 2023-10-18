namespace AlifTestTask.Models
{
    public class TranInfo
    {
        public string? Account { get; set; }
        public string? Phone { get; set; }
        public decimal? Amount { get; set; }
        public int TranType { get; set; }
        public DateTime? Date { get; set; }        

    }

    public class FullTranInfo
    {
        public List<TranInfo>? TranInfo { get; set; }
        public int Count { get; set; }
    }
}
