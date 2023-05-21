using AutoMapper;
using ProyectoPresupuesto.Models;
namespace ProyectoPresupuesto.Services
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountCreateViewModel>();
        }
    }
}