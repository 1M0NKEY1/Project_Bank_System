using Models.Accounts;

namespace BankSystemWebApp.Application.Contracts.Services.Accounts;

public interface ICurrentAccountManager
{
    Task<Account>? Account { get; }
}