using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{

    public interface IAccountRepository
    {
        Task Create(Account account);
        Task<Account> GetById(int id, int userId);
        Task<IEnumerable<Account>> Search(int userId);
        Task Update(CreationAccountViewModel creationAccountViewModel);
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

        public async Task<Account> GetById(int id, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return  (await connection.QueryFirstOrDefaultAsync<Account>(
                        @"SELECT a.Id, a.[name], a.[balance], at.[id] as [accountTypeId]
                        FROM account a
                        INNER JOIN accountType at ON a.accountTypeId = at.Id
                        WHERE at.userId = @userId and a.id = @Id", new {userId, id }));
            }
        }

        public async Task Update(CreationAccountViewModel creationAccountViewModel)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(@"UPDATE account 
                                                SET [name] = @Name, [accountTypeId] = @AccountTypeId, [balance] = @Balance, [description] = @Description 
                                                WHERE Id = @Id", creationAccountViewModel);
            }
        }

    }
}
