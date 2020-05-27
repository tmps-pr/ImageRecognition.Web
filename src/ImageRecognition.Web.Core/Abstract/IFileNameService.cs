using Microsoft.AspNetCore.Http;

namespace FaceId.Web.Core.Abstract
{
    public interface IFileNameService 
    {
        public string GetFileName(IFormFile file);
    }
}
