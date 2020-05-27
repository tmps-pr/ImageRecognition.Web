using FaceId.Web.Core.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace FaceId.Web.Api.Services
{
    public class DateFileNameService : IFileNameService
    {
        public string GetFileName(IFormFile file)
        {
            var extension = string.IsNullOrWhiteSpace(file.FileName) ? "" : Path.GetExtension(file.FileName);
            return $"{ DateTimeOffset.UtcNow.Ticks}.{ extension}";
        }
    }
}
