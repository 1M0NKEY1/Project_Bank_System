using System.Data.SqlClient;
using System.Globalization;
using BankSystemWebApp.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Accounts;
using Models.Accounts.TypesOfAccount;
using Npgsql;

namespace DataAccess.Repositories;

public class AccountRepository : IAccountRepository
{
    private const decimal StartBalance = 0;
    private readonly ApplicationContext _dbContext;

    public AccountRepository(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Account?> FindAccountByName(string name, string surname, long pin)
    {
        return await _dbContext.Accounts
            .Where(a => a.Name == name && a.Surname == surname)
            .FirstOrDefaultAsync(a => a.pin == pin);
    }

    public async Task SignUp(string name, string surname, long pin, string age, string email, int passport, string address, TypeOfCard typeOfCard)
    {
        const string request = """
                               insert into accounts (name, surname, age, email, address, type)
                               values (@name, @surname, @age, @email, @address, @typeOfCard);
                               insert into accountpassport (passport) values (@passport);
                               insert into accountpin (pin) values (@pin);
                               insert into accountbalance (balance) values (@StartBalance);
                               """;

        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new SqlParameter("name", name));
        command.Parameters.Add(new SqlParameter("surname", surname));
        command.Parameters.Add(new SqlParameter("pin", pin));
        command.Parameters.Add(new SqlParameter("age", age));
        command.Parameters.Add(new SqlParameter("email", email));
        command.Parameters.Add(new SqlParameter("address", address));
        command.Parameters.Add(new SqlParameter("type", typeOfCard));
        command.Parameters.Add(new SqlParameter("passport", passport));
        command.Parameters.Add(new SqlParameter("balance", StartBalance));

        await command.ExecuteNonQueryAsync();
    }

    public async Task<decimal> ShowAccountBalance(long id)
    {
        var account = await _dbContext.Accounts.FindAsync(id);

        if (account is null) return 0;

        return account.balance;
    }

    public async Task AddMoney(long id, decimal money)
    {
        var account = await _dbContext.Accounts.FindAsync(id);

        if (account is not null)
        {
            account = account with { balance = account.balance + money };
            
            var operation = "Replenishment of the balance for " + money.ToString(CultureInfo.InvariantCulture);
            await UpdateOperationInHistory(account.id, operation);
            
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveMoney(long id, decimal money)
    {
        var account = await _dbContext.Accounts.FindAsync(id);

        if (account is not null)
        {
            account = account with { balance = account.balance - money };
            
            var operation = "Withdrawal of " + money.ToString(CultureInfo.InvariantCulture) + " from the balance";
            await UpdateOperationInHistory(account.id, operation);
            
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task TransferMoney(long id, long addresseeId, decimal money)
    {
        var account = await _dbContext.Accounts.FindAsync(id);
        var transferredAccount = await _dbContext.Accounts.FindAsync(addresseeId);
        
        if (account is not null)
        {
            if (transferredAccount is not null)
            {
                account = account with { balance = account.balance - money };
                transferredAccount = transferredAccount with { balance = transferredAccount.balance + money };
                
                var operationFrom = "Transfer: " + money.ToString(CultureInfo.InvariantCulture)
                                                 + "to: "
                                                 + transferredAccount.Name
                                                 + transferredAccount.Surname;
                var operationTo = "You have been transferred: " + money.ToString(CultureInfo.InvariantCulture)
                                                                + "from"
                                                                + account.Name
                                                                + account.Surname;

                await UpdateOperationInHistory(account.id, operationFrom);
                await UpdateOperationInHistory(transferredAccount.id, operationTo);

                await _dbContext.SaveChangesAsync();
            }
            
        }
    }

    public async Task<IList<string>> ShowAccountHistory(long id)
    {
        var tmpList = new List<string>();

        const string request = """
                               select operationType
                               from accountoperationhistory
                               where accountid = @id;
                               """;
        
        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new NpgsqlParameter("accountId", id));

        await using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            tmpList.Add(reader.GetString(0));
        }

        return tmpList;
    }

    public async Task<IList<string>> ShowAllNotifications(long id)
    {
        var tmpList = new List<string>();
        const string request = """
                               select notifications
                               from accountnotifications
                               where accountid = @id;
                               """;
        
        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new NpgsqlParameter("accountId", id));

        await using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            tmpList.Add(reader.GetString(0));
        }

        return tmpList;
    }

    private static async Task UpdateOperationInHistory(long id, string operation)
    {
        const string request = """
                               insert into accountoperationhistory (accountid, operationtype)
                               values (@id, @operation);
                               """;
        
        await using var connection = await DatabaseConnection.GetCConnectionAsync();

        await using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new NpgsqlParameter("accountId", id));
        command.Parameters.Add(new NpgsqlParameter("operationType", operation));
        
        await command.ExecuteNonQueryAsync();
    }
}