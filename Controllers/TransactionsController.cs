using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoPresupuesto.Models;
using ProyectoPresupuesto.Services;

namespace ProyectoPresupuesto.Controllers
{
    
    public class TransactionsController: Controller
    {
        private readonly IrepositoryTransactions repositoryTransactions;
        private readonly IrepositoryCategories repositoryCategories;
        private readonly IrepositoryAccounts repositoryAccounts;
        private readonly IUsers users;
        public TransactionsController(IrepositoryTransactions repositoryTransactions, 
        IrepositoryCategories repositoryCategories, IrepositoryAccounts repositoryAccounts, IUsers users)
        {
            this.repositoryTransactions = repositoryTransactions;
            this.repositoryCategories = repositoryCategories;
            this.repositoryAccounts = repositoryAccounts;
            this.users = users;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = users.GetId();
            var model = new TransactionCreateViewModel();
            model.Accounts = await GetAccounts(userId);
            model.Categories = await GetCategories(((int)model.OperationTypeId), userId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TransactionCreateViewModel transaction)
        {
            var userId = users.GetId();
            transaction.UserId = userId;
            if (!ModelState.IsValid)
            {
                transaction.Accounts = await GetAccounts(userId);
                transaction.Categories = await GetCategories(((int)transaction.OperationTypeId), userId);
                return View(transaction);
            }
            if(transaction.OperationTypeId == OperationTypeId.Gasto)
            {
                transaction.Amount *= -1;
            }
            await this.repositoryTransactions.Create(transaction);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> GetCategories([FromBody] int operationTypeId)
        {
            var userId = users.GetId();
            var categories = await repositoryTransactions.GetCategories(operationTypeId, userId);
            return Ok(categories);
        }
        private async Task<IEnumerable<SelectListItem>> GetCategories(int operationTypeId, int userId)
        {
            var accountCategories = await repositoryTransactions.GetCategories(operationTypeId, userId);
            return accountCategories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
        private async Task<IEnumerable<SelectListItem>> GetAccounts(int userId)
        {
            var accountAccounts = await repositoryAccounts.Search(userId);
            return accountAccounts.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

    }
}