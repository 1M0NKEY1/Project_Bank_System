using BankSystemWebApp.Application.Abstractions.Repositories;
using BankSystemWebApp.Application.Contracts.Operations.Services;
using Models.Accounts;
using Models.Banks;

namespace DataAccess.Repositories;

public class BankRepository : IBankRepository
{
    public Task<Bank>? FindBankByName(string name, long adminEntryKey)
    {
        throw new NotImplementedException();
    }

    public Task<Account>? FindAccountById(long accountId)
    {
        throw new NotImplementedException();
    }

    public Task TransferMoneyToAnotherBank(long id, long anotherBankId)
    {
        throw new NotImplementedException();
    }

    public Task ChangeLimitsForCreditCard(long accountId, decimal sum)
    {
        throw new NotImplementedException();
    }

    public Task ChangePercentageOfCredit(long accountId, float percent)
    {
        throw new NotImplementedException();
    }

    public Task CancelTransaction(long accountId)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> HistoryOfTransactions()
    {
        throw new NotImplementedException();
    }

    public Task UserNotification(long accountId, ITypeOfOperation operation)
    {
        throw new NotImplementedException();
    }
}