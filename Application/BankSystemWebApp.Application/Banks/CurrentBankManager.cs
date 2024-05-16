using Contracts.Banks;
using Models.Banks;

namespace BankSystemWebApp.Application.Banks;

public class CurrentBankManager : ICurrentBankManager
{
    public Task<Bank>? Bank { get; set; }
}