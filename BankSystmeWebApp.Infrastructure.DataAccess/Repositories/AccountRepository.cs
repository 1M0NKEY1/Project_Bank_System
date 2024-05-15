using System.Data.SqlClient;
using BankSystemWebApp.Application.Abstractions.Repositories;
using Models.Accounts;
using Npgsql;

namespace DataAccess.Repositories;

public class AccountRepository : IAccountRepository
{
    public Account? FindAccountByName(string name, string surname, long pin)
    {
        const string request = """
                           SELECT a.id, a.bankId, a.Name, a.Surname, a.Age, a.email, a.Address, apas.Passport, ab.Balance
                           FROM accounts a
                           INNER JOIN public.accountpin ap ON a.id = ap.accountid
                           INNER JOIN public.accountpassport apas on a.id = apas.accountid
                           INNER JOIN public.accountbalance ab ON a.id = ab.accountid
                           WHERE a.Name = @name AND a.Surname = @surname AND ap.PIN = @pin;
                           """;
        
        using var connection = new NpgsqlConnection(new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 6432,
            Username = "postgres",
            Password = "postgres",
            SslMode = SslMode.Prefer,
        }.ConnectionString);
        
        connection.Open();

        using var command = new NpgsqlCommand(request, connection);
        command.Parameters.Add(new SqlParameter("name", name));
        command.Parameters.Add(new SqlParameter("surname", surname));
        command.Parameters.Add(new SqlParameter("pin", pin));

        using var reader = command.ExecuteReader();
        
        if (reader.Read() is false) return null;

        return new Account(
            id: reader.GetInt64(0),
            bankId: reader.GetInt64(1),
            Name: reader.GetString(2),
            Surname: reader.GetString(3),
            Age: reader.GetString(4),
            email: reader.GetString(5),
            Address: reader.GetString(6),
            Passport: reader.GetInt32(7),
            pin: reader.GetInt64(8),
            balance: reader.GetDecimal(9));
    }

    public void SignUp(string name, string surname, long pin, string age, string email, int passport, string address)
    {
        const string request = """
                               insert into accounts (Name,
                                                     Surname,
                                                     Age,
                                                     Email,
                                                     Address)
                               values (@name,
                                       @surname,
                                       @age,
                                       @email
                                       @address);
                               insert into accountpassport (passport) values (@passport);
                               insert into accountpin (pin) values (@pin);
                               insert into accountbalance (balance) values (0);
                               """;
    }

    public decimal ShowAccountBalance(long id)
    {
        throw new NotImplementedException();
    }

    public void AddMoney(long id, decimal money)
    {
        throw new NotImplementedException();
    }

    public void RemoveMoney(long id, decimal money)
    {
        throw new NotImplementedException();
    }

    public void TransferMoney(long id, long AddresseeId, decimal money)
    {
        throw new NotImplementedException();
    }

    public IList<string> ShowAccountHistory(long id)
    {
        throw new NotImplementedException();
    }
}