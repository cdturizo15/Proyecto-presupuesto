using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoPresupuesto.Models;
using ProyectoPresupuesto.Services;

namespace ProyectoPresupuesto.Controllers;
public class AccountsController: Controller
{
    private readonly IRepositoryAccountTypes repositoryAccountTypes;
    private readonly IUsers users;
    private readonly IrepositoryAccounts repositoryAccounts;
    private readonly IMapper mapper;
    public AccountsController(IRepositoryAccountTypes repositoryAccountTypes,
            IUsers users, IrepositoryAccounts repositoryAccounts, IMapper mapper)
    {
        this.repositoryAccountTypes = repositoryAccountTypes;
        this.users = users;
        this.repositoryAccounts = repositoryAccounts;
        this.mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = users.GetId();
        var accounts = await repositoryAccounts.Search(userId);
        var model = accounts
        .GroupBy(x => x.AccountTypeName)
        .Select(group => new IndexAccountsViewModel
        {
            AccountType = group.Key,
            Accounts = group.AsEnumerable()
        }).ToList();
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var userId = users.GetId();
        var model = new AccountCreateViewModel();
        model.AccountTypes = await GetAccountTypes(userId);
        return View(model);
    }
    [HttpPost]
    public async Task <IActionResult> Create(AccountCreateViewModel account)
    {
        var userId = users.GetId();
        var accountType = await repositoryAccountTypes.GetById(account.AccountTypeId, userId);
        if (accountType is null)
        {
            return RedirectToAction("AccountNotFound", "Home");
        }
        if (!ModelState.IsValid)
        {
            account.AccountTypes = await GetAccountTypes(userId);
            return View(account);
        }
        await repositoryAccounts.Create(account);
        return RedirectToAction("index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = users.GetId();
        var account = await repositoryAccounts.GetById(id, userId);
        if (account is null)
        {
            return RedirectToAction("AccountNotFound", "Home");
        }
        var model = mapper.Map<AccountCreateViewModel>(account);
        model.AccountTypes = await GetAccountTypes(userId);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(AccountCreateViewModel accountEdited)
    {
        var userId = users.GetId();
        var account = await repositoryAccounts.GetById(accountEdited.Id, userId);
        if (account is null)
        {
            return RedirectToAction("AccountNotFound", "Home");
        }

        var accountType = await repositoryAccountTypes.GetById(accountEdited.AccountTypeId, userId);

        if(accountType is null)
        {
            return RedirectToAction("AccountNotFound", "Home");
        }
        if (!ModelState.IsValid)
        {
            accountEdited.AccountTypes = await GetAccountTypes(userId);
            return View(account);
        }
        await repositoryAccounts.Edit(accountEdited);
        return RedirectToAction("index");
    }
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = users.GetId();
        var account = await repositoryAccounts.GetById(id, userId);
        if (account is null)
        {
            return RedirectToAction("AccountNotFound", "Home");
        }
        return View(account);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(Account account)
    {
        var userId = users.GetId();
        var accountExist = await repositoryAccounts.GetById(account.Id, userId);
        if (accountExist is null)
        {
            return RedirectToAction("AccountNotFound", "Home");
        }
        var accountType = await repositoryAccountTypes.GetById(account.AccountTypeId, userId);
        if (accountType is null)
        {
            System.Console.WriteLine("No existe tipo cuenta");
            return RedirectToAction("AccountNotFound", "Home");
        }
        await repositoryAccounts.Delete(account.Id);
        return RedirectToAction("index");
    }
    private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
    {
        var accountTypes = await repositoryAccountTypes.Get(userId);
        return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
    }
}
