namespace Contracts.OperationResults;

public abstract record OperationResult
{
    private OperationResult() {}

    public sealed record Completed : OperationResult;

    public sealed record Rejected : OperationResult;
}