namespace CarRental.Data.Infastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
