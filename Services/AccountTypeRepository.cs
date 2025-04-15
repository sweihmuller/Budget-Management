using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{
    public interface IAccountTypeRepository
    {
        Task Create(AccountType accountType);
    }
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly string _connectionString;

        public AccountTypeRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task Create(AccountType accountType)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>($@"INSERT INTO AccountType (name, userId, [order])
                                                        VAlUES (@name, @userId, 0)
                                                        SELECT SCOPE_IDENTITY();", accountType);
                accountType.Id = id;    
            }
        }   
    }
}
