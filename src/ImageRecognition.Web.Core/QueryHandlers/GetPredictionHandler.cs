using FaceId.Web.Core.Models;
using FaceId.Web.Core.Queries;
using MediatR;
using Microsoft.Extensions.ML;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FaceId.Web.Core.QueryHandlers
{
    public abstract class GetPredictionHandler<T> : IRequestHandler<GetPrediction<T>, Dictionary<string, float>>
        where T: InMemoryImageData
    {
        private readonly PredictionEnginePool<T, Prediction> _predictionEnginePool;
        private volatile object obj = new object();
        public GetPredictionHandler(PredictionEnginePool<T, Prediction> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        protected abstract T GetImageInput(byte[] bytes);
        public async Task<Dictionary<string, float>> Handle(GetPrediction<T> request, CancellationToken cancellationToken)
        {
            var imageMemoryStream = new MemoryStream();
            await request.File.CopyToAsync(imageMemoryStream);
            byte[] imageData = imageMemoryStream.ToArray();

            var imageInputData = GetImageInput(imageData);
            Prediction prediction = null;
            lock(obj)
            { 
                prediction = _predictionEnginePool.Predict(imageInputData);
            }
            var engine = _predictionEnginePool.GetPredictionEngine();
            var scoreEntries = GetScoresWithLabelsSorted(engine.OutputSchema, "Score", prediction.Score);
            return scoreEntries;
        }

        private static Dictionary<string, float> GetScoresWithLabelsSorted(DataViewSchema schema, string name, float[] scores)
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            var column = schema.GetColumnOrNull(name);

            var slotNames = new VBuffer<ReadOnlyMemory<char>>();
            column.Value.GetSlotNames(ref slotNames);
            var names = new string[slotNames.Length];
            var num = 0;
            foreach (var denseValue in slotNames.DenseValues())
            {
                result.Add(denseValue.ToString(), scores[num++]);
            }

            return result.OrderByDescending(c => c.Value).ToDictionary(i => i.Key, i => i.Value);
        }
    }
}
