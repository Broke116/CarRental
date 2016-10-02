using CarRental.Web.Structure.Mapper;

namespace CarRental.Web
{
    public class Bootstrapper
    {
        public static void Run()
        {
            AutofacConfig.SetAutofacContainer();
        }
    }
}