using System.ComponentModel.DataAnnotations;
using ProyectoPresupuesto.Validations;
namespace ProyectoPresupuesto.Models;

public class Account
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo nombre es requerido")]
    [StringLength(maximumLength: 50, MinimumLength =3, ErrorMessage = "El campo nombre ndebe tener entre 3 y 50 caracteres")]
    [CamelCaseAtrribute]
    public string Name { get; set; }
    public int AccountTypeId { get; set; }
    public decimal Balance { get; set; }
    public string Description {get; set;}

    public string AccountTypeName { get; set; }
}


    