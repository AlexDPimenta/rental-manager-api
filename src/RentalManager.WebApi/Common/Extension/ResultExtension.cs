namespace RentalManager.WebApi.Common.Extension;

public static class ResultExtensions
{
    public static T Match<T>(
        this Result<T> result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }
}
