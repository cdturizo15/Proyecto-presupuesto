

using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPresupuesto.Models;

namespace ProyectoPresupuesto.Services
{
    public interface IrepositoryCategories
    {
        Task Create(Category category);
        Task<IEnumerable<Category>> Get(int userId);
        Task<Category> GetById(int id, int userId);
        Task Edit(Category category);
        Task Delete(int id, int userId);
    }
    public class RepositoryCategories: IrepositoryCategories
    {
        private readonly string connectionString;

        public RepositoryCategories(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task Create(Category category)
        {
            using var connection = new SqlConnection(this.connectionString);
            var id = await connection.QuerySingleAsync<int>("INSERT INTO Categories (Name, OperationTypeId, UserId) VALUES (@Name, @OperationTypeId, @UserId); SELECT SCOPE_IDENTITY();", category);
            category.Id = id;
        }
        public async Task<IEnumerable<Category>> Get(int userId)
        {
            using var connection = new SqlConnection(this.connectionString);
            return await connection.QueryAsync<Category>("SELECT * FROM Categories WHERE UserId = @UserId", new { UserId = userId });
        }
        public async Task<Category> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(this.connectionString);
            return await connection.QuerySingleAsync<Category>("SELECT * FROM Categories WHERE Id = @Id AND UserId = @UserId", new { Id = id, UserId = userId });
        }
        public async Task Edit(Category category)
        {
            using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync("UPDATE Categories SET Name = @Name, OperationTypeId = @OperationTypeId WHERE Id = @Id AND UserId = @UserId", category);
        }
        public async Task Delete(int id, int userId)
        {
            using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync("DELETE FROM Categories WHERE Id = @Id AND UserId = @UserId", new { Id = id, UserId = userId });
        }
    }

}