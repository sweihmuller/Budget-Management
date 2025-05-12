using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{

    public interface IAccountRepository
    {
        Task Create(Account account);
        Task<IEnumerable<Account>> Search(int userId);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection");
         
        public async Task Create(Account account)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>(@"INSERT INTO account ([name], [accountTypeId], [balance], [description]) " +
                                                  "VALUES (@Name, @AccountTypeId, @Balance, @Description);" +
                                                  "SELECT SCOPE_IDENTITY();", account);
                account.Id = id;
            }
        }


        public async Task<IEnumerable<Account>> Search(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<Account>(
                    @"SELECT a.Id, a.[name], a.[balance], at.[name] as [accountType]
                      FROM account a
                      INNER JOIN accountType at ON a.accountTypeId = at.Id
                      WHERE at.userId = @userId
                      ORDER BY at.[order]", new { userId })
                );
            }
        }

    }
}
