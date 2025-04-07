using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{
    public class AccountTypeRepositoryBase
    {
        private string _connectionString;

        public void Create(AccountType accountType)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var id = connection.QuerySingle<int>($@"INSERT INTO AccountType (name, userId, order)
                                                        VAlUES (@name, userId, 0)
                                                        SELECT SCOPE_IDENTITY();", accountType);
                accountType.Id = id;
            }
        }
    }
}