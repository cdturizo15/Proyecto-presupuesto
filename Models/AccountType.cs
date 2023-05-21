using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoPresupuesto.Validations;
namespace ProyectoPresupuesto.Models;

public class AccountType
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo nombre es requerido")]
    [StringLength(maximumLength: 50, MinimumLength =3, ErrorMessage = "El campo nombre debe tener entre 3 y 50 caracteres")]
    [CamelCaseAtrribute]
    [Remote (action: "AccountTypeExist", controller: "AccountsTypes")]
    public string Name { get; set; }

    public int UserId { get; set; }

    public int Order { get; set; }
}
