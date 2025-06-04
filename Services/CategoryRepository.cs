using Budget_Management.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Services
{
    public interface ICategoryRepository
    {
        Task Create(Category category);
        Task Delete(int id);
        Task<IEnumerable<Category>> GetAll(int userId);
        Task<IEnumerable<Category>> GetAll(int userId, OperationType operationType);
        Task<Category> GetById(int id, int userId);
        Task Update(Category category);
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

        public async Task<IEnumerable<Category>> GetAll(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Category>(@"SELECT * 
                                                               FROM category 
                                                               WHERE userId = @userId",
                                                               new { userId});

            }
        }

        public async Task<IEnumerable<Category>> GetAll(int userId, OperationType operationType)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Category>(@"SELECT * 
                                                               FROM category 
                                                               WHERE userId = @userId AND operationTypeId = @operationType", 
                                                               new { userId, operationType });
               
            }
        }

        public async Task<Category> GetById(int id, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstAsync<Category>(@"SELECT * FROM category 
                                                                    WHERE Id = @id AND userId = @userId", new { id, userId });
            }
        }

        public async Task Update(Category category)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(@"UPDATE category
                                                SET [name] = @Name,
	                                                [operationTypeId] = @operationTypeId 
                                                WHERE [Id] = @Id", category);
            }
        }

        public async Task Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(@"DELETE FROM category WHERE Id = @Id", new { id });
            }
        }
    }
}
