using System.ComponentModel.DataAnnotations;

namespace ProyectoPresupuesto.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage ="El campo nombre debe tener entre 3 y 50 caracteres")]
        public string Name { get; set; }
        public OperationTypeId OperationTypeId { get; set; }
        public int UserId { get; set; }
    }
}