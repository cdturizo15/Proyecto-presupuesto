using ProyectoPresupuesto.Models;
using ProyectoPresupuesto.Services;
namespace ProyectoPresupuesto.Validations{
    public interface IRepeatedAccountType{
            Task<bool> AlreadyExists(AccountType accountType);
    }
    public class RepeatedAccountType: IRepeatedAccountType
    {
        private readonly IRepositoryAccountTypes repositoryAccountTypes;
        private readonly IUsers users;
        
        public RepeatedAccountType(IRepositoryAccountTypes repositoryAccountTypes,
        IUsers users)

        {
            this.repositoryAccountTypes = repositoryAccountTypes;
            this.users = users;
        }
        public async Task <bool> AlreadyExists(AccountType accountType){
            accountType.UserId = users.GetId();
            var alreadyExists = await repositoryAccountTypes.NameExist(accountType.Name, accountType.UserId);
            if (alreadyExists)
            {
                return true;
            }
            return false;
        }
             
    }
}