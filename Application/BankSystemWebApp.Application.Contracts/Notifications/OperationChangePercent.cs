using System.Globalization;
using BankSystemWebApp.Application.Contracts.Operations.Services;

namespace BankSystemWebApp.Application.Contracts.Operations;

public class OperationChangePercent : ITypeOfOperation
{
    private readonly float _percent;

    public OperationChangePercent(float percent)
    {
        _percent = percent;
    }
    public string LogText()
    {
        return "Your Credit card percent changed: " + _percent.ToString(CultureInfo.InvariantCulture);
    }
}