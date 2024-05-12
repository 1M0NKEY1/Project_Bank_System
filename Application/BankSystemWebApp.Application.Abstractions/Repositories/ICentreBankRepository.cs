using Models.Banks;

namespace BankSystemWebApp.Application.Abstractions.Repositories;

public interface ICentreBankRepository
{
    void CreateBank(string name);

    Bank? FindBankByName(string name); 

    bool TransactionAccess();

    void BankNotification(long bankId);
}