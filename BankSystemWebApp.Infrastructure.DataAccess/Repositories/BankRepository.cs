using System.Data.SqlClient;
using System.Globalization;
using BankSystemWebApp.Application.Abstractions.Repositories;
using BankSystemWebApp.Application.Contracts.Operations;
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
                
                var operationFrom = "Transfer: " + money.ToString(CultureInfo.InvariantCulture)
                                                 + "to: "
                                                 + transferredBank.name;
                var operationTo = "You have been transferred: " + money.ToString(CultureInfo.InvariantCulture)
                                                                + "from"
                                                                + bank.name;

                await UpdateOperationInHistory(bank.id, operationFrom);
                await UpdateOperationInHistory(transferredBank.id, operationTo);

                await _dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task ChangeLimitsForCreditCard(long accountId, decimal sum)
    {
        var account = await _dbContext.Accounts.FindAsync(accountId);

        if (account is not null)
        {
            account = account with { balance = account.balance + sum };
            
            await UserNotification(accountId, new OperationChangeLimits(sum));

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task ChangePercentageForCreditCard(long accountId, float percent)
    {
        const string request = """
                               update AccountCreditPercentage
                               set percent = @percent
                               where accountId = @accountId
                               """;
        
        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new SqlParameter("accountId", accountId));
        command.Parameters.Add(new SqlParameter("percent", percent));

        await UserNotification(accountId, new OperationChangePercent(percent));

        await command.ExecuteNonQueryAsync();
    }

    public async Task CancelTransaction(long accountId, long addresseeId, decimal money)
    {
        var account = await _dbContext.Banks.FindAsync(accountId);
        var transferredAccount = await _dbContext.Banks.FindAsync(addresseeId);

        if (account is not null)
        {
            if (transferredAccount is not null)
            {
                account = account with { balance = account.balance + money };
                transferredAccount = transferredAccount with { balance = transferredAccount.balance - money };

                await UserNotification(accountId, new OperationCancelTransaction());
                await UserNotification(addresseeId, new OperationCancelTransaction());

                await _dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task<IList<string>> HistoryOfTransactions(long id)
    {
        var tmpList = new List<string>();

        const string request = """
                               select operationType
                               from bankoperationhistory
                               where bankId = @id;
                               """;
        
        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new NpgsqlParameter("bankId", id));

        await using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            tmpList.Add(reader.GetString(0));
        }

        return tmpList;
    }

    public async Task UserNotification(long accountId, ITypeOfOperation operation)
    {
        const string request = """
                               insert into accountnotifications (accountid, notifications)
                               values (@id, @operation);
                               """;
        
        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new NpgsqlParameter("accountId", accountId));
        command.Parameters.Add(new NpgsqlParameter("notifications", operation.LogText()));
        
        await command.ExecuteNonQueryAsync();
    }
    
    private static async Task UpdateOperationInHistory(long id, string operation)
    {
        const string request = """
                               insert into bankoperationhistory (bankid, operationtype)
                               values (@id, @operation);
                               """;
        
        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new NpgsqlParameter("bankId", id));
        command.Parameters.Add(new NpgsqlParameter("operationType", operation));
        
        await command.ExecuteNonQueryAsync();
    }
}