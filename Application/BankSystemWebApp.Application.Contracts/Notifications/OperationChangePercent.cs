using BankSystemWebApp.Application.Contracts.Operations.Services;

namespace BankSystemWebApp.Application.Contracts.Operations;

public class OperationChangePercent : ITypeOfOperation
{
    public string LogText()
    {
        return "Your Credit card percent changed: ";
    }
}