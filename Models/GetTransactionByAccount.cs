namespace Budget_Management.Models
{
    public class GetTransactionByAccount
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
