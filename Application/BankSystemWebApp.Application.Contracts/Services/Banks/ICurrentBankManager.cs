using Models.Banks;

namespace Contracts.Banks;

public interface ICurrentBankManager
{
    Bank? Bank { get; }
}