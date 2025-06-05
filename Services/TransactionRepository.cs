using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{
    public interface ITransactionRepository
    {
        Task Create(Transaction transaction);
    }
    public class TransactionRepository(IConfiguration configuration) : ITransactionRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task Create(Transaction transaction)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>("Insert_Transaction", 
                new
                {

                    transaction.UserId,
                    transaction.DateTransaction ,
                    transaction.Amount,
                    transaction.CategoryId,
                    transaction.Note,
                    transaction.AccountId
                },
                commandType: System.Data.CommandType.StoredProcedure);
               
               transaction.Id = id;
            }

        }

    }
}
