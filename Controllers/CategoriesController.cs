using Microsoft.AspNetCore.Mvc;
using ProyectoPresupuesto.Models;
using ProyectoPresupuesto.Services;

namespace ProyectoPresupuesto.Controllers
{
    public class CategoriesController: Controller
    {
        private readonly IrepositoryCategories repositoryCategories;
        private readonly IUsers users;

        public CategoriesController(IrepositoryCategories repositoryCategories, IUsers users)
        {
            this.repositoryCategories = repositoryCategories;
            this.users = users;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var id = users.GetId();
            var list = await this.repositoryCategories.Get(id);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {           
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var userId = users.GetId();
            category.UserId = userId;
            await this.repositoryCategories.Create(category);
            return RedirectToAction("Index");
            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = users.GetId();
            var category = await this.repositoryCategories.GetById(id, userId);
            if(category is null)
            {
                return NotFound();
            }
            category.UserId = userId;
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var userId = users.GetId();
            category.UserId = userId;
            var categoryExist = await this.repositoryCategories.GetById(category.Id, userId);
            if (categoryExist is null)
            {
                return NotFound();
            }
            await this.repositoryCategories.Edit(category);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task <IActionResult> Delete(int id)
        {
            var userId = users.GetId();
            var categoryExist = await this.repositoryCategories.GetById(id, userId);
            if (categoryExist is null)
            {
                return NotFound();
            }
            categoryExist.UserId = userId;
            return View(categoryExist);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Category category)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var categoryExist = await this.repositoryCategories.GetById(category.Id, category.UserId);
            if (categoryExist is null)
            {
                return NotFound();
            }
            await this.repositoryCategories.Delete(category.Id, category.UserId);
            return RedirectToAction("Index");
        }
    }
}