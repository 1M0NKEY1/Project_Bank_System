using Contracts.OperationResults;

namespace Contracts.CenterBank;

public interface ICentreBankService
{
    OperationResult CreateBank(string name);

    OperationResult FindBankByName(string name); 

    OperationResult TransactionBetweenBanksAccess();

    OperationResult BankNotification();
}