using System.Threading.Tasks;

namespace FaceId.Web.Core.Abstract
{
    public interface IFileServiceInitializer
    {
        Task InitializeAsync(string containerName);
    }
}
