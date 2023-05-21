
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoPresupuesto.Models
{
    public class TransactionCreateViewModel: Transaction
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Accounts { get; set; }
        public OperationTypeId OperationTypeId { get; set; } = OperationTypeId.Ingreso;
    }
    
}