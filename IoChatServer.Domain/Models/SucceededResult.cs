namespace IoChatServer.Domain.Models;

public class SucceededResult
{
    public bool Succeeded { get; }
    public List<string> Errors { get; }

    public static SucceededResult Success => new (true, (string?) null);
    public static SucceededResult Failure(string? error) => new(false, error);
    public static SucceededResult Failure(List<string>? errors) => new(false, errors);

    public SucceededResult(bool succeeded, List<string>? errors = null)
    {
        Succeeded = succeeded;
        Errors = errors ?? new();
    }

    public SucceededResult(bool succeeded, string? error = null) 
        : this(succeeded, string.IsNullOrEmpty(error) ? null : new List<string> { error } ) 
    {
    }
}

public class SucceededResult<TResult> : SucceededResult
{
    public TResult? Result { get; }

    public SucceededResult(TResult result) 
        : base(true, (string?) null)
    {
        Result = result;
    }

    private SucceededResult(bool succeeded, 
        List<string>? errors = null) : base(succeeded, errors)
    {
    }

    private SucceededResult(bool succeeded, 
        string? error = null) : base(succeeded, error)
    {
    }

    public static SucceededResult<TResult> Success(TResult result) => new(result);

    public static SucceededResult<TResult> Failure(string? error) => new(false, error);
    public static SucceededResult<TResult> Failure(List<string>? errors) => new(false, errors);
}