using BankSystemWebApp.Application.Contracts.Operations.Services;
using Models.Accounts;
using Models.Banks;

namespace BankSystemWebApp.Application.Abstractions.Repositories;

public interface IBankRepository
{
    Bank? FindBankByName(string name, long adminEntryKey);

    Account? FindAccountById(long accountId);

    void TransferMoneyToAnotherBank(long id, long anotherBankId);

    void ChangeLimitsForCreditCard(long accountId, decimal sum);

    void ChangePercentageOfCredit(long accountId, float percent);

    void CancelTransaction(long accountId);

    IList<string> HistoryOfTransactions();
    
    void UserNotification(long accountId, ITypeOfOperation operation);
    
}