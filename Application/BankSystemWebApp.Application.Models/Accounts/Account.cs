
namespace Models.Accounts;

public record Account(
    long id,
    long bankId,
    string Name,
    string Surname,
    string Age,
    string email,
    int Passport,
    string Address,
    long pin,
    decimal balance);