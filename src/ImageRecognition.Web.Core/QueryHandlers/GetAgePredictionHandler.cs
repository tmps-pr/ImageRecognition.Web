using FaceId.Web.Core.Models;
using FaceId.Web.Core.Queries;
using Microsoft.Extensions.ML;

namespace FaceId.Web.Core.QueryHandlers
{
    public class GetAgePredictionHandler : GetPredictionHandler<AgeInMemoryImageData>
    {
        public GetAgePredictionHandler(PredictionEnginePool<AgeInMemoryImageData, Prediction> predictionEnginePool) : base(predictionEnginePool)
        {
        }

        protected override AgeInMemoryImageData GetImageInput(byte[] bytes)
        {
            return new AgeInMemoryImageData(bytes, null, null);
        }
    }
}
