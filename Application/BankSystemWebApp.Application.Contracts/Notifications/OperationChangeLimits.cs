using System.Globalization;
using BankSystemWebApp.Application.Contracts.Operations.Services;

namespace BankSystemWebApp.Application.Contracts.Operations;

public class OperationChangeLimits : ITypeOfOperation
{
    private readonly decimal _money;
    public OperationChangeLimits(decimal money)
    {
        _money = money;
    }
    public string LogText()
    {
        return "Your Credit card limits changed: " + _money.ToString(CultureInfo.InvariantCulture);
    }
}