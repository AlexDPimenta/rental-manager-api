using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Options;
using RentalManager.WebApi.Settings;
using System.IO;

namespace RentalManager.WebApi.Persistence.Service;

public class AzureStorageService : IAzureStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _blobClient;
    private readonly string _containerName;

    public AzureStorageService(IOptions<AzureStorageSettings> options)
    {
        _blobServiceClient = new BlobServiceClient(options.Value.ConnectionString);
        _blobClient = _blobServiceClient.GetBlobContainerClient(options.Value.ContainerName);
        _containerName = options.Value.ContainerName;
    }   

    public async Task<string> UploadFileAsync(string fileName, 
        string base64Image, CancellationToken cancellationToken)    
    {
        await _blobClient.CreateIfNotExistsAsync();

        var blockBlobClient = _blobClient.GetBlockBlobClient(fileName);
        byte[] imageBytes = Convert.FromBase64String(base64Image);        

        var options = new BlockBlobOpenWriteOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = "jpg"
            }
        };

        var streamOpen = await blockBlobClient.OpenWriteAsync(overwrite: true, options);

        using (var stream = new MemoryStream(imageBytes))
        {
            await stream.CopyToAsync(streamOpen);
        }
            
        await streamOpen.FlushAsync();

        return blockBlobClient.Uri.AbsoluteUri;                
    }
}
