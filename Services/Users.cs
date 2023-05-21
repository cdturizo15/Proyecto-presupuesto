namespace ProyectoPresupuesto.Services
{
    public interface IUsers
    {
        public int GetId();
    }
    public class Users: IUsers
    {
        public int GetId()
        {
            return 1;
        }
    }
}