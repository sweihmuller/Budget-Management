using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{
    public interface ITransactionRepository
    {
        Task Create(Transaction transaction);
        Task Delete(int id);
        Task<IEnumerable<Transaction>> GetAccountById(GetTransactionByAccount moodel);
        Task<Transaction> GetById(int id, int userId);
    }
    public class TransactionRepository(IConfiguration configuration) : ITransactionRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task Create(Transaction transaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>("Insert_Transaction",
                new
                {

                    transaction.UserId,
                    transaction.DateTransaction,
                    transaction.Amount,
                    transaction.CategoryId,
                    transaction.Note,
                    transaction.AccountId
                },
                commandType: System.Data.CommandType.StoredProcedure);

                transaction.Id = id;
            }

        }

        public async Task<IEnumerable<Transaction>> GetAccountById(GetTransactionByAccount model)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Transaction>(@"
                       SELECT t.Id, t.amount, t.dateTransaction, c.[name] as Category, a.[name] as Account, c.operationTypeId
                       FROM transactions t
                       INNER JOIN category c ON c.Id = t.categoryId
                       INNER JOIN account a ON a.id = t.accountId
                       WHERE t.accountId = @AccountId AND
                             t.userId = @UserId AND
                             dateTransaction BETWEEN @StartDate AND @EndDate", model);
            }
        }

        public async Task<Transaction> GetById(int id, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Transaction>(
                        @"SELECT * 
                        FROM transactions
                        INNER JOIN category cat
                        ON cat.Id = transactions.categoryId
                        WHERE transactions.Id = @Id AND transactions.userId = @userId;"
                        , new { id, userId});
            }
        }

        public async Task Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("Delete_Transaction",
                    new { id }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
