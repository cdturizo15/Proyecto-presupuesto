using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPresupuesto.Models;

namespace ProyectoPresupuesto.Services
{
    public interface IrepositoryTransactions
    {
        Task Create(Transaction transaction);
        Task<IEnumerable<Category>> GetCategories(int OperationTypeId ,int userId);
    }
    public class RepositoryTransactions: IrepositoryTransactions
    {
        private readonly string connectionstring;
        public RepositoryTransactions(IConfiguration configuration)
        {
            this.connectionstring = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Transaction transaction)
        {
            using var connection = new SqlConnection(this.connectionstring);
            var id = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Transactions (UserId, CategoryId, AccountId, DateTransaction, Amount, Note)
                VALUES (@UserId, @CategoryId, @AccountId, @DateTransaction, @Amount, @Note); 
                Update Accounts
                set balance += @Amount
                where Id = @AccountId;
                select SCOPE_IDENTITY();", transaction);
            transaction.Id = id;
        }
        public async Task<IEnumerable<Category>> GetCategories(int OperationTypeId, int userId)
        {
            using var connection = new SqlConnection(this.connectionstring);
            var categories = await connection.QueryAsync<Category>(@"
                SELECT Id, Name
                FROM Categories
                WHERE UserId = @UserId and OperationTypeId = @OperationTypeId", new { OperationTypeId = OperationTypeId, UserId = userId });
            return categories;
        }
    }
}