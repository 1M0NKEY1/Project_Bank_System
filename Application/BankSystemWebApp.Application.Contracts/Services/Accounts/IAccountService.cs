using Contracts.LoginResults;
using Contracts.OperationResults;
using Models.Accounts.TypesOfAccount;

namespace BankSystemWebApp.Application.Contracts.Services.Accounts;

public interface IAccountService
{
    LoginResult SignIn(string name, string surname, long pin);
    OperationResult SignUp(
        string name,
        string surname,
        long pin,
        string age,
        string email,
        int passport,
        string address,
        TypeOfCard typeOfCard);
    Task<decimal> ShowAccountBalance();
    OperationResult AddMoney(decimal money);
    OperationResult RemoveMoney(decimal money);
    OperationResult TransferMoney(long addresseeId, decimal money);
    OperationResult ShowAccountHistory();
    OperationResult ShowAllNotifications();
}