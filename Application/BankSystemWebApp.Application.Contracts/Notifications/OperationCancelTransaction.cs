using BankSystemWebApp.Application.Contracts.Operations.Services;

namespace BankSystemWebApp.Application.Contracts.Operations;

public class OperationCancelTransaction : ITypeOfOperation
{
    public string LogText()
    {
        return "Your transaction canceled.";
    }
}