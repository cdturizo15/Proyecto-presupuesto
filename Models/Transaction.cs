
using System.ComponentModel.DataAnnotations;

namespace ProyectoPresupuesto.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de categoria")]
        public int CategoryId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta")]
        public int AccountId { get; set; }
        [Display(Name = "Fecha Transaccion")]
        [DataType(DataType.Date)]
        public DateTime DateTransaction { get; set; } = DateTime.Today;
        public decimal Amount { get; set; }
        [StringLength(maximumLength: 500, ErrorMessage ="La nota no puede tener mas de 500 caracteres")]
        public string Note { get; set; }
    }
}