﻿using BankSystemWebApp.Application.Contracts.Operations.Services;
using Models.Accounts;
using Models.Banks;

namespace BankSystemWebApp.Application.Abstractions.Repositories;

public interface IBankRepository
{
    Task<Bank?> FindBankByName(string name, long adminEntryKey);

    Task<Account?> FindAccountById(long accountId);

    Task CreateBank(string name, long adminEntryKey);

    Task TransferMoneyToAnotherBank(long id, long addresseeBankId, decimal money);

    Task ChangeLimitsForCreditCard(long accountId, decimal sum);

    Task ChangePercentageOfCredit(long accountId, float percent);

    Task CancelTransaction(long accountId);

    Task<IList<string>> HistoryOfTransactions();
    
    Task UserNotification(long accountId, ITypeOfOperation operation);
    
}