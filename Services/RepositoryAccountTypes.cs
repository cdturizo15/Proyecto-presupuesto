using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPresupuesto.Models;

namespace ProyectoPresupuesto.Services
{
    public interface IRepositoryAccountTypes
    {
        Task Create(AccountType accountType);
        Task <bool> NameExist(string name, int userId);
        Task<IEnumerable<AccountType>> Get(int userId);
        Task Edit(AccountType accountType);
        Task <AccountType> GetById (int id, int userId);
        Task Delete(AccountType accountType);
        Task Order(IEnumerable<AccountType> accountTypes);
    }
    public class RepositoryAccountTypes: IRepositoryAccountTypes
    {
        private readonly string connectionString;

       
        public RepositoryAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("AccountTypeInsert", new{Userid = accountType.UserId, Name = accountType.Name}, commandType: System.Data.CommandType.StoredProcedure);
            accountType.Id = id;
        }

        public async Task Edit(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountsTypes SET Name = @Name WHERE Id = @Id",accountType);
        }
        public async Task Delete(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM AccountsTypes WHERE Id = @Id", accountType);
        }
        public async Task<IEnumerable<AccountType>> Get(int userId)
        {
            await using var connection = new SqlConnection(connectionString);
            var accountTypes = await connection.QueryAsync<AccountType>(
                @"select Id, Name, UserId, AccountOrder
                from AccountsTypes 
                where UserId = @UserId
                order by AccountOrder;",
                new { userId });
            return accountTypes;
        }
        public async Task<bool> NameExist(string name, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(
                @"select 1
                from AccountsTypes 
                where Name = @Name 
                and UserId = @UserId;",
                new { name, userId });
            return exists==1;
        }
        public async Task<AccountType> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountType>(
                @"select Id, Name, UserId, AccountOrder
                from AccountsTypes 
                where Id = @Id 
                and UserId = @UserId;",
                new { id, userId });
        }
        public async Task Order(IEnumerable<AccountType> accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountsTypes SET AccountOrder = @Order WHERE Id = @Id", accountTypes);
        }
        
    }
    
}