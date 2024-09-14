using System.Text.Json.Serialization;

namespace RentalManager.WebApi.Common;

public record Error
{
    /// <summary>
    /// Represents a special error instance used to represent no error.
    /// </summary>
    public static readonly Error None = new( string.Empty, ErrorType.Failure);

    /// <summary>
    /// Represents a special error instance used to represent no error.
    /// </summary>
    public static readonly Error NullValue = new("The specified result value is null.",
        ErrorType.Failure);

    private Error(string message, ErrorType errorType)
    {        
        Message = message;
        ErrorType = errorType;
    }
    [JsonPropertyName("mensagem")]
    public string Message { get; }
    [JsonIgnore]
    public ErrorType ErrorType { get; }

    /// <summary>
    /// Represents a failure error with its code and message.
    /// </summary>    
    /// <param name="message">The message describing the error.</param>
    /// <returns>A new instance of the Error class representing a failure error.</returns>
    public static Error Failure(string message) => new(message, ErrorType.Failure);

    /// <summary>
    ///     Represents a validation error with its code and message.
    /// </summary>
    /// <param name="message">The message describing the error.</param>
    /// <returns>A new instance of the Error class representing a validation error.</returns>
    public static Error Validation(string message) => new(message, ErrorType.Validation);

    /// <summary>
    ///     Represents a not found error with its code and message.
    /// </summary>
    /// <param name="message">The message describing the error.</param>
    /// <returns>A new instance of the Error class representing a not found error.</returns>
    public static Error NotFound(string message) => new(message, ErrorType.NotFound);

    /// <summary>
    ///     Represents a conflict error with its code and message.
    /// </summary>
    /// <param name="message">The message describing the error.</param>
    /// <returns>A new instance of the Error class representing a conflict error.</returns>
    public static Error Conflict(string message) => new(message, ErrorType.Conflict);
}

/// <summary>
///     Enumeration of error types.
/// </summary>
public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3
}
