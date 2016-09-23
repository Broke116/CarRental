using System;
using CarRental.Data.App_Data;

namespace CarRental.Data.Infastructure
{
    public interface IDbFactory : IDisposable
    {
        CarRentalDBEntities Init();
    }
}
