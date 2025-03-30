using Microsoft.ML.Data;

namespace SleepGo.MLTrainer.Models
{
    public class HotelRecommendationPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }
        public float Score { get; set; }
        public float Probability { get; set; }
    }
}
