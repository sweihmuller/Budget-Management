namespace Budget_Management.Models
{
    public class DetailedReportTransactions
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public IEnumerable<GetTransactionByDate> TransactionsByDate { get; set; }
        public decimal DepositBalance => TransactionsByDate.Sum(x => x.DepositBalance);
        public decimal WithdrawalBalance => TransactionsByDate.Sum(x => x.WithdrawalBalance);
        public decimal TotalBalance => DepositBalance - WithdrawalBalance;
        public class GetTransactionByDate
        {
            public DateTime DateTransaction { get; set; }
            public IEnumerable<Transaction> Transactions { get; set; }
            public decimal DepositBalance => Transactions
                                             .Where(x => x.OperationTypeId == OperationType.Income)
                                             .Sum(x => x.Amount);
            public decimal WithdrawalBalance => Transactions
                                             .Where(x => x.OperationTypeId == OperationType.Expense)
                                             .Sum(x => x.Amount);
        }
    }
}
