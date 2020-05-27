using FaceId.Web.Api.Options;
using FaceId.Web.Core.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FaceId.Web.Api.Services
{
    public class BlobStorageFileService : IFileService, IFileServiceInitializer
    {
        private readonly IOptions<BlobStorageOption> _options;
        private readonly CloudBlobClient _cloudBlobClient;
        public BlobStorageFileService(IOptions<BlobStorageOption> options)
        {
            _options = options;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(options.Value.ConnectionString);
            _cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }
        public async Task InitializeAsync(string containerName)
        {
            CloudBlobContainer cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();
        }

        public async Task SaveImageAsync(IFormFile file, string folder, string containerName, IFileNameService fileNameService)
        {
            CloudBlobContainer cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);

            var saveFileName = fileNameService.GetFileName(file);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"{folder}/{saveFileName}");
            await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());
        }
    }
}
