using BankSystemWebApp.Application.Abstractions.Repositories;
using BankSystemWebApp.Application.Accounts;
using BankSystemWebApp.Application.Contracts.Operations;
using BankSystemWebApp.Application.Contracts.Operations.Services;
using Contracts.Banks;
using Contracts.LoginResults;
using Contracts.OperationResults;

namespace BankSystemWebApp.Application.Banks;

internal class BankService : IBankService
{
    private ITypeOfOperation? _operation;
    private readonly IBankRepository _repository;
    private readonly CurrentAccountManager? _currentAccountManager;
    private readonly CurrentBankManager _currentBankManager;

    public BankService(IBankRepository repository, CurrentBankManager currentBankManager)
    {
        _repository = repository;
        _currentBankManager = currentBankManager;
    }
    
    public LoginResult SignIn(string name, long adminEntryKey)
    {
        var bank = _repository.FindBankByName(name, adminEntryKey);

        if (bank is null) return new LoginResult.NotFound();
        
        _currentBankManager.Bank = bank;
        return new LoginResult.Success();
    }

    public OperationResult FindAccountById(long accountId)
    {
        var account = _repository.FindAccountById(accountId);
        if (account is null) return new OperationResult.Rejected();

        if (_currentAccountManager != null) _currentAccountManager.Account = account;
        return new OperationResult.Completed();
    }

    public OperationResult TransferMoneyToAnotherBank(long anotherBankId)
    {
        if (_currentBankManager.Bank is null) return new OperationResult.Rejected();
        
        _repository.TransferMoneyToAnotherBank(_currentBankManager.Bank.id, anotherBankId);
        return new OperationResult.Completed();
    }

    public OperationResult ChangeLimitsForCreditCard(decimal sum)
    {
        if (_currentAccountManager?.Account is null) return new OperationResult.Rejected();
        
        _repository.ChangeLimitsForCreditCard(_currentAccountManager.Account.id, sum);
        _operation = new OperationChangeLimits();
        return new OperationResult.Completed();
    }

    public OperationResult ChangePercentageOfCredit(float percent)
    {
        if (_currentAccountManager?.Account is null) return new OperationResult.Rejected();
        
        _repository.ChangePercentageOfCredit(_currentAccountManager.Account.id, percent);
        _operation = new OperationChangePercent();
        return new OperationResult.Completed();
    }

    public OperationResult CancelTransaction()
    {
        if (_currentAccountManager?.Account is null) return new OperationResult.Rejected();

        _repository.CancelTransaction(_currentAccountManager.Account.id);
        _operation = new OperationCancelTransaction();
        return new OperationResult.Completed();
    }

    public OperationResult HistoryOfTransactions()
    {
        if (_currentBankManager.Bank is null) return new OperationResult.Rejected();

        _repository.HistoryOfTransactions();
        return new OperationResult.Completed();
    }

    public OperationResult UserNotification()
    {
        if (_currentAccountManager?.Account is null) return new OperationResult.Rejected();

        if (_operation != null) _repository.UserNotification(_currentAccountManager.Account.id, _operation);
        return new OperationResult.Completed();
    }
}