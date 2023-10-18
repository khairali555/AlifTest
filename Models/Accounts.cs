namespace AlifTestTask.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Account { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }        
    }
}
