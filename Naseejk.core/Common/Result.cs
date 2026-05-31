namespace Naseejk.core.Common;

public class Result<T>
{
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; } = Array.Empty<string>();

    public T? Data { get; set; }
    public static Result<T> Success(T data) => new Result<T> { Succeeded = true, Data = data };

    public static Result<T> Failure(IEnumerable<string> errors)
        => new Result<T> { Succeeded = false, Errors = errors.ToArray() };

    public static Result<T> Failure(string error)
        => new Result<T> { Succeeded = false, Errors = new[] { error } };
}