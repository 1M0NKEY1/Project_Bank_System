using BankSystemWebApp.Application.Contracts.Services.Accounts;
using Models.Accounts;

namespace BankSystemWebApp.Application.Accounts;

internal class CurrentAccountManager : ICurrentAccountManager
{
    public Account? Account { get; set; }
}