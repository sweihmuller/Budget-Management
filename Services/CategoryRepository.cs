using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{
    public interface ICategoryRepository
    {
        Task Create(Category category);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Category category)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>(@"INSERT INTO category ([name], [operationTypeId], [userId]) " +
                                                   "VALUES (@Name, @OperationTypeId, @UserId);" +
                                                   "SELECT SCOPE_IDENTITY();", category);
                category.Id = id;
            }
        }
    }
}
