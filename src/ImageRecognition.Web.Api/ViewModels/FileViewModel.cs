using Microsoft.AspNetCore.Http;

namespace FaceId.Web.Api.ViewModels
{
    public class FileViewModel
    {
        public IFormFile File { get; set; }
    }
}
