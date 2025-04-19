using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{
    public interface IAccountTypeRepository
    {
        Task Create(AccountType accountType);
        Task<bool> DoesExist(string name, int userId);
        Task<IEnumerable<AccountType>> Retrieve(int userId);
        Task<AccountType> RetrieveById(int id, int userId);
        Task Update(AccountType accountType);
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

        public async Task<bool> DoesExist(string name, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var exists = await connection.QueryFirstOrDefaultAsync<int>($@"SELECT COUNT(*) 
                                                                               FROM AccountType 
                                                                               WHERE name = @name AND userId = @userId",
                                                                               new { name, userId });
                return exists > 0;
            }
        }

        public async Task<IEnumerable<AccountType>> Retrieve(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<AccountType>(@$"SELECT[Id],[name], [userId], [order]
                                                                 FROM [BudgetManagement].[dbo].[accountType]
                                                                 WHERE userId = @userId;", new { userId });
            }
        }

        public async Task Update(AccountType accountType)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync($@"UPDATE AccountType
                                                SET name = @name
                                                WHERE Id = @Id", accountType);
            }
        }
        public async Task<AccountType> RetrieveById(int id, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<AccountType>($@"SELECT [Id], [name], [userId], [order]
                                                                                 FROM AccountType
                                                                                 WHERE Id = @Id AND userId = @userId", 
                                                                                 new { id, userId });
            }
        }
    }
}
