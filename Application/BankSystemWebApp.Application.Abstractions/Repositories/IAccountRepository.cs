using Models.Accounts;

namespace BankSystemWebApp.Application.Abstractions.Repositories;

public interface IAccountRepository
{
    Account? FindAccountByName(string name, string surname, long pin);
    void SignUp(
        string name,
        string surname,
        long pin,
        string age,
        string email,
        int passport,
        string address);
    decimal ShowAccountBalance(long id);
    void AddMoney(long id, decimal money);
    void RemoveMoney(long id, decimal money);
    void TransferMoney(long id, long AddresseeId, decimal money);
    IList<string> ShowAccountHistory(long id);
}