namespace Budget_Management.Models
{
    public class GetTransactionByUserParameters
    {
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
