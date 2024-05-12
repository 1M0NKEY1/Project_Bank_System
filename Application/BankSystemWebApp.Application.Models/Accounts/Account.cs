using Models.Users;

namespace Models.Accounts;

public record Account(
    long id,
    long bankId,
    User user,
    long pin,
    decimal balance);