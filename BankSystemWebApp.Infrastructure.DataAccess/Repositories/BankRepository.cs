using System.Data.SqlClient;
using BankSystemWebApp.Application.Abstractions.Repositories;
using BankSystemWebApp.Application.Contracts.Operations.Services;
using Microsoft.EntityFrameworkCore;
using Models.Accounts;
using Models.Banks;
using Npgsql;

namespace DataAccess.Repositories;

public class BankRepository : IBankRepository
{
    private readonly ApplicationContext _dbContext;

    public BankRepository(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Bank?> FindBankByName(string name, long adminEntryKey)
    {
        return await _dbContext.Banks
            .Where(a => a.name == name)
            .FirstOrDefaultAsync(a => a.adminEntryKey == adminEntryKey);
    }

    public async Task<Account?> FindAccountById(long accountId)
    {
        return await _dbContext.Accounts
            .Where(a => a.id == accountId)
            .FirstOrDefaultAsync();
    }

    public async Task CreateBank(string name, long adminEntryKey)
    {
        const string request = """
                               insert into banks (name) values (@name);
                               insert into banksentrykeys (adminEntryKey)
                               values (@adminEntryKey);
                               """;
        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new SqlParameter("name", name));
        command.Parameters.Add(new SqlParameter("adminEntryKey", adminEntryKey));

        await command.ExecuteNonQueryAsync();
    }

    public async Task TransferMoneyToAnotherBank(long id, long addresseeBankId, decimal money)
    {
        var bank = await _dbContext.Banks.FindAsync(id);
        var transferredBank = await _dbContext.Banks.FindAsync(addresseeBankId);

        if (bank is not null)
        {
            if (transferredBank is not null)
            {
                bank = bank with { balance = bank.balance - money };
                transferredBank = transferredBank with { balance = transferredBank.balance + money };

                await _dbContext.SaveChangesAsync();
            }
        }
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