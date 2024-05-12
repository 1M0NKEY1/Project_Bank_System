using BankSystemWebApp.Application.Models.CentreBank;

namespace Contracts.CenterBank;

public interface ICurrentCentreBankManager
{
    CentreBank CentreBank { get; }
}