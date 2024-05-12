using BankSystemWebApp.Application.Abstractions.Repositories;
using BankSystemWebApp.Application.Banks;
using Contracts.CenterBank;
using Contracts.OperationResults;

namespace BankSystemWebApp.Application.CenterBank;

internal class CentreBankService : ICentreBankService
{
    private readonly ICentreBankRepository _repository;
    private readonly CurrentBankManager? _currentBankManager;

    public CentreBankService(ICentreBankRepository repository)
    {
        _repository = repository;
    }
    
    public OperationResult FindBankByName(string name)
    {
        var bank = _repository.FindBankByName(name);

        if (bank is null) return new OperationResult.Rejected();

        if (_currentBankManager != null) _currentBankManager.Bank = bank;
        return new OperationResult.Completed();
    }
    
    public OperationResult CreateBank(string name)
    {
        if (_currentBankManager?.Bank is null) return new OperationResult.Rejected();
        
        _repository.CreateBank(name);
        return new OperationResult.Completed();
    }
    
    public OperationResult TransactionBetweenBanksAccess()
    {
        if (_currentBankManager?.Bank is null) return new OperationResult.Rejected();
        
        _repository.TransactionAccess();
        return new OperationResult.Completed();
    }

    public OperationResult BankNotification()
    {
        if (_currentBankManager?.Bank is null) return new OperationResult.Rejected();
        
        _repository.BankNotification(_currentBankManager.Bank.id);
        return new OperationResult.Completed();
    }
}