using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FaceId.Web.Core.Abstract
{
    public interface IFileService
    {
        Task SaveImageAsync(IFormFile file, string folder, string containerName, IFileNameService fileNameService);
    }
}
