using MassTransit.SagaStateMachine;

namespace RentalManager.WebApi.Common;

public class Result
{
    /// <summary>
    ///     Represents the result of an operation.
    /// </summary>
    /// <param name="isSuccess">A boolean value indicating whether the operation was successful.</param>
    /// <param name="error">An instance of the Error class representing the error that occurred during the operation, if any.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the isSuccess parameter is true and the error parameter is not Error.None,
    ///     or when the isSuccess parameter is false and the error parameter is Error.None.
    /// </exception>
    protected Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }
    


    /// <summary>
    ///     Gets a value indicating whether the operation was successful or not.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the operation was successful; otherwise, <c>false</c>.
    /// </value>
    public bool IsSuccess { get; }

    /// <summary>
    ///     Gets a value indicating whether the operation is a failure.
    /// </summary>
    /// <remarks>
    ///     This property returns the negation of the <see cref="IsSuccess" /> property.
    /// </remarks>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    ///     Gets the error property.
    /// </summary>
    /// <value>
    ///     The error.
    /// </value>
    public Error Error { get; }

    public Error[] Errors { get; }

    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new Result<TValue>(default, false, error);
    }

    /// <summary>
    ///     Creates a new instance of the Result class with a successful result and no error.
    /// </summary>
    /// <returns>A new instance of the Result class with a successful result and no error.</returns>
    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    /// <summary>
    ///     Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to be set.</param>
    /// <returns>A new instance of the result class with the specified value set as the result.</returns>
    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }           



    /// <summary>
    ///     Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The error to associate with the failure result.</param>
    /// <returns>A new <see cref="Result" /> object representing a failure with the specified error.</returns>
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }    

    /// <summary>
    ///     Creates a new instance of the Result class with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap in the Result instance.</param>
    /// <returns>
    ///     A Result instance containing the specified value if it is not null, otherwise a Result instance indicating a
    ///     failure with the Error.NullValue.
    /// </returns>
    public static Result<TValue> Create<TValue>(TValue? value)
    {
        return value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }
}

public sealed record Error(string mensagem)
{
    public static readonly Error NullValue = new("The specified result value is null.");

    public static readonly Error None = new(string.Empty);
}

public class Result<TValue> : Result
{
    /// <summary>
    ///     Represents the value of a variable.
    /// </summary>
    private readonly TValue? _value;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{TValue}" /> class.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <param name="isSuccess">A flag indicating whether the result is a success.</param>
    /// <param name="error">The error associated with the result, if any.</param>
    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }


    /// <summary>
    ///     Gets the value of the property.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when trying to access the value of a failure result.</exception>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    /// <summary>
    ///     Represents an implicit conversion operator for converting a nullable value to a Result object.
    /// </summary>
    /// <param name="value">The nullable value to be converted.</param>
    /// <typeparam name="TValue">The type of the nullable value.</typeparam>
    /// <returns>A Result object that represents the converted nullable value.</returns>
    public static implicit operator Result<TValue>(TValue? value)
    {
        return Create(value);
    }
}
