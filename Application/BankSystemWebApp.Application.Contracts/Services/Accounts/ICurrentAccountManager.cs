using Models.Accounts;

namespace BankSystemWebApp.Application.Contracts.Services.Accounts;

public interface ICurrentAccountManager
{
    Account Account { get; }
}