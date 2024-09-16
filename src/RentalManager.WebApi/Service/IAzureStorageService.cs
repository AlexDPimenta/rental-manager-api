namespace RentalManager.WebApi.Service;

public interface IAzureStorageService
{
    Task<string> UploadFileAsync(string fileName, string base64Image, CancellationToken cancellationToken);
}
