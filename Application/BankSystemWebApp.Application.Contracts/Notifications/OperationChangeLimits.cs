using BankSystemWebApp.Application.Contracts.Operations.Services;

namespace BankSystemWebApp.Application.Contracts.Operations;

public class OperationChangeLimits : ITypeOfOperation
{
    public string LogText()
    {
        return "Your Credit card limits changed: ";
    }
}