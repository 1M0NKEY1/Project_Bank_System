﻿using BankSystemWebApp.Application.Abstractions.Repositories;
using BankSystemWebApp.Application.Contracts.Services.Accounts;
using Contracts.LoginResults;
using Contracts.OperationResults;
using Models.Accounts.TypesOfAccount;

namespace BankSystemWebApp.Application.Accounts;

internal class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly CurrentAccountManager _currentAccount;

    public AccountService(IAccountRepository repository, CurrentAccountManager currentAccount)
    {
        _repository = repository;
        _currentAccount = currentAccount;
    }
    
    public LoginResult SignIn(string name, string surname, long pin)
    {
        var account = _repository.FindAccountByName(name, surname, pin);

        if (account is null) return new LoginResult.NotFound();

        _currentAccount.Account = account;
        return new LoginResult.Success();
    }

    public OperationResult SignUp(
        string name, 
        string surname, 
        long pin, 
        string age, 
        string email, 
        int passport, 
        string address, 
        TypeOfCard typeOfCard)
    {
        var account = _repository.FindAccountByName(name, surname, pin);
        if (account is not null) return new OperationResult.Rejected();
        
        _repository.SignUp(
            name,
            surname,
            pin,
            age,
            email,
            passport,
            address,
            typeOfCard);
        return new OperationResult.Completed();
    }

    public async Task<decimal> ShowAccountBalance()
    {
        if (_currentAccount.Account is null) return 0;

        return await _repository.ShowAccountBalance(_currentAccount.Account.Id);
    }

    public OperationResult AddMoney(decimal money)
    {
        if (_currentAccount.Account is null) return new OperationResult.Rejected();
        
        _repository.AddMoney(_currentAccount.Account.Id, money);
        return new OperationResult.Completed();
    }

    public OperationResult RemoveMoney(decimal money)
    {
        if (_currentAccount.Account is null) return new OperationResult.Rejected();
        
        _repository.RemoveMoney(_currentAccount.Account.Id, money);
        return new OperationResult.Completed();
    }

    public OperationResult TransferMoney(long addresseeId, decimal money)
    {
        if (_currentAccount.Account is null) return new OperationResult.Rejected();
        
        _repository.TransferMoney(_currentAccount.Account.Id, addresseeId, money);
        return new OperationResult.Completed();
    }

    public OperationResult ShowAccountHistory()
    {
        if (_currentAccount.Account is null) return new OperationResult.Rejected();

        _repository.ShowAccountHistory(_currentAccount.Account.Id);
        return new OperationResult.Completed();
    }

    public OperationResult ShowAllNotifications()
    {
        if (_currentAccount.Account is null) return new OperationResult.Rejected();

        _repository.ShowAllNotifications(_currentAccount.Account.Id);
        return new OperationResult.Completed();
    }
}