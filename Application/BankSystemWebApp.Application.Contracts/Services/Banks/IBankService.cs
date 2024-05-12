using BankSystemWebApp.Application.Contracts.Operations.Services;
using Contracts.LoginResults;
using Contracts.OperationResults;

namespace Contracts.Banks;

public interface IBankService
{
    LoginResult SignIn(string name, long adminEntryKey);
    OperationResult FindAccountById(long accountId);
    OperationResult TransferMoneyToAnotherBank(long anotherBankId);
    OperationResult ChangeLimitsForCreditCard(decimal sum);
    OperationResult ChangePercentageOfCredit(float percent);
    OperationResult CancelTransaction();
    OperationResult HistoryOfTransactions();
    OperationResult UserNotification();
}