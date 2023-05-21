using Microsoft.AspNetCore.Mvc;
using ProyectoPresupuesto.Models;
using ProyectoPresupuesto.Services;
using ProyectoPresupuesto.Validations;
namespace ProyectoPresupuesto.Controllers
{
    public class AccountsTypesController: Controller
    {
        private readonly IRepositoryAccountTypes repositoryAccountTypes;
        private readonly IRepeatedAccountType repeatedAccountType; 
        private readonly IUsers users;
        public AccountsTypesController(IRepositoryAccountTypes repositoryAccountTypes,
         IRepeatedAccountType repeatedAccountType, IUsers users)
        {
            this.repositoryAccountTypes = repositoryAccountTypes;
            this.repeatedAccountType = repeatedAccountType;
            this.users = users;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var id = users.GetId();
            var list = await this.repositoryAccountTypes.Get(id);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }
            var alreadyExists = await repeatedAccountType.AlreadyExists(accountType); 
            if (alreadyExists)
            {
                ModelState.AddModelError
                (nameof(accountType.Name) ,$"Ya existe un tipo de cuenta con el nombre {accountType.Name}, por favor elija otro nombre");
                return View(accountType);
            }
            await repositoryAccountTypes.Create(accountType);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = users.GetId();
            var accountType = await repositoryAccountTypes.GetById(id, userId);
            if(accountType is null)
            {
                return RedirectToAction("AccountNotFound", "Home");
            }
            return View(accountType);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(AccountType accountType)
        {
            var userId = users.GetId();
            var AccountTypeExist = await repositoryAccountTypes.GetById(accountType.Id, userId);
            if (AccountTypeExist is null)
            {
                return RedirectToAction("AccountNotFound" ,"Home");
            }
            await repositoryAccountTypes.Edit(accountType);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = users.GetId();
            var accountType = await repositoryAccountTypes.GetById(id, userId);
            if (accountType is null)
            {
                return RedirectToAction("AccountNotFound", "Home");
            }
            return View(accountType);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(AccountType accountType)
        {
            var userId = users.GetId();

            var accountTypeExist = await repositoryAccountTypes.GetById(accountType.Id, userId);
            if (accountTypeExist is null)
            {
                return RedirectToAction("AccountNotFound", "Home");
            }
            await repositoryAccountTypes.Delete(accountType);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Order([FromBody] int[] ids)
        {
            var userid = users.GetId();
            var accountTypes = await repositoryAccountTypes.Get(userid);
            var idsAccountTypes = accountTypes.Select(x => x.Id);
            var otherIds = ids.Except(idsAccountTypes).ToList();
            if (otherIds.Count > 0)
            {
                return Forbid();
            }
            var accountTypesOrdered = ids.Select((values, index)=>
                             new AccountType()
                             {
                                 Id = values,
                                 Order = index+1
                             }).AsEnumerable();
            await repositoryAccountTypes.Order(accountTypesOrdered);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> AccountTypeExist(string name)
        {
           var userid = users.GetId();
           var yaExiste = await repositoryAccountTypes.NameExist(name, userid);
           if(yaExiste)
           {
               return Json(@$"Ya existe un tipo de cuenta con el nombre {name}");
           }
           return Json(true);
        }
    }
    
}