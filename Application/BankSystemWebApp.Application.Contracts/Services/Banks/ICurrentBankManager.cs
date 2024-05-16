using Models.Banks;

namespace Contracts.Banks;

public interface ICurrentBankManager
{
    Task<Bank>? Bank { get; }
}