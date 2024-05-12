namespace Models.Users;

public record User(
    string Name,
    string Surname,
    string Age,
    string email,
    int Passport,
    string Address);