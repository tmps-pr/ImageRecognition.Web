using FaceId.Web.Core.Models;
using Microsoft.Extensions.ML;
using System.Threading.Tasks;

namespace FaceId.Web.Core.QueryHandlers
{
    public class GetGenderPredictionHandler : GetPredictionHandler<GenderInMemoryImageData>
    {
        public GetGenderPredictionHandler(PredictionEnginePool<GenderInMemoryImageData, Prediction> predictionEnginePool) : base(predictionEnginePool)
        {
        }

        protected override GenderInMemoryImageData GetImageInput(byte[] bytes)
        {
            return new GenderInMemoryImageData(bytes, null, null);
        }
    }
}
