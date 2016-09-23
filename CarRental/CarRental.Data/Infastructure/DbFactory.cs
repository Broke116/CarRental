using CarRental.Data.App_Data;

namespace CarRental.Data.Infastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        CarRentalDBEntities dbContext;

        public CarRentalDBEntities Init()
        {
            return dbContext ?? (dbContext = new CarRentalDBEntities());
        }

        protected override void DisposeCore()
        {
            dbContext?.Dispose();
        }
    }
}
