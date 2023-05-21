using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPresupuesto.Models;

namespace ProyectoPresupuesto.Services;

public interface IrepositoryAccounts
{
    Task Create(Account account);
    Task <IEnumerable<Account>> Search(int userId);

    Task <Account> GetById(int id, int userId);

    Task Edit(Account account);
    
    Task Delete(int id);
}
public class RepositoryAccounts: IrepositoryAccounts
{
    private readonly string connectionString;
    public RepositoryAccounts(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }
   public async Task Create(Account account)
   {
        using var connection = new SqlConnection(connectionString);
        var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Accounts 
                    (Name, AccountTypeId,Balance, Description) 
                    VALUES (@Name, @AccountTypeId, @Balance, @Description); 
                    SELECT SCOPE_IDENTITY();", new{account.Name, account.AccountTypeId, account.Balance, account.Description});
        account.Id = id;
   }
   public async Task Edit(Account account)
   {
        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync(@"UPDATE Accounts SET Name = @Name, 
                    AccountTypeId = @AccountTypeId, Balance = @Balance, 
                    Description = @Description WHERE Id = @Id", account);
   }
   public async Task Delete(int id)
   {
        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync(@"DELETE FROM Accounts WHERE Id = @Id", new {id});
   }
   public async Task<IEnumerable<Account>> Search(int userId)
   {
        using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<Account>(@"select acc.Id, Balance,acc.Name, acct.Name as AccounttypeName
                            from Accounts acc
                            inner join AccountsTypes acct
                            on acct.id = acc.AccountTypeId
                            where acct.UserId = @UserId
                            order by acct.AccountOrder", new {userId});
   }
   public async Task<Account> GetById(int id, int userId)
   {
        using var connection = new SqlConnection(connectionString);
        return await connection.QuerySingleAsync<Account>(@"select acc.Id, Balance,acc.Name, acc.AccountTypeId, Description
                            from Accounts acc
                            inner join AccountsTypes acct
                            on acct.id = acc.AccountTypeId
                            where acct.UserId = @UserId
                            and acc.Id = @Id", new {userId, id});
   }
}