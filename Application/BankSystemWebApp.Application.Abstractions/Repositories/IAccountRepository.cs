using Models.Accounts;

namespace BankSystemWebApp.Application.Abstractions.Repositories;

public interface IAccountRepository
{
    Task<Account?> FindAccountByName(string name, string surname, long pin);
    Task SignUp(string name,
        string surname,
        long pin,
        string age,
        string email,
        int passport,
        string address);
    Task<decimal> ShowAccountBalance(long id);
    Task AddMoney(long id, decimal money);
    Task RemoveMoney(long id, decimal money);
    Task TransferMoney(long id, long AddresseeId, decimal money);
    Task<IList<string>> ShowAccountHistory(long id);
}