using BankSystemWebApp.Application.Models.CentreBank;
using Contracts.CenterBank;

namespace BankSystemWebApp.Application.CenterBank;

public class CurrentCentreBankManager : ICurrentCentreBankManager
{
    public CentreBank CentreBank { get; set; }
}