namespace CarRental.Service.Interfaces
{
    public interface IEncryptionService
    {
        string CreateSalt();
        string EncryptPassword(string password, string salt);
    }
}
