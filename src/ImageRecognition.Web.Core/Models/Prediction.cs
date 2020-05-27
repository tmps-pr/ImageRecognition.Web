using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace FaceId.Web.Core.Models
{
    public class Prediction
    {
        [ColumnName("Score")]
        public float[] Score;

        [ColumnName("PredictedLabel")]
        public string PredictedLabel;
    }
}
