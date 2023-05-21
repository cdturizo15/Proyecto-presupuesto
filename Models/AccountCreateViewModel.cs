using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoPresupuesto.Models;
public class AccountCreateViewModel: Account
{
    public IEnumerable <SelectListItem> AccountTypes { get; set; }
}