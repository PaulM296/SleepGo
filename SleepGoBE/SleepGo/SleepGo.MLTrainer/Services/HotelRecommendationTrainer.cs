using Microsoft.ML;
using SleepGo.MLTrainer.Models;

namespace SleepGo.MLTrainer.Services
{
    public class HotelRecommendationTrainer
    {
        private readonly string _modelPath = "hotel_recommendation_mode.zip";

        public void Train(string dataPath)
        {
            var mlContext = new MLContext();

            // Load data
            IDataView data = mlContext.Data.LoadFromTextFile<HotelRecommendationData>(
                path: dataPath,
                hasHeader: true,
                separatorChar: ','
            );

            // Data process pipeline
            var dataProcessPipeline = mlContext.Transforms
                .Categorical.OneHotEncoding("UserId")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("HotelId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("City"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("Country"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("RoomType"))
                .Append(mlContext.Transforms.Concatenate("Features", "UserId", 
                "HotelId", "HotelRating", "PricePaid", "City", "Country", "RoomType"));

            // Choose an algorithm
            var trainer = mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);


            // Train the model
            var model = trainingPipeline.Fit(data);

            // Save the model
            mlContext.Model.Save(model, data.Schema, _modelPath);

            Console.WriteLine($"Model trained and saved to {_modelPath}");
        }

        public void Test(string dataPath)
        {
            var mlContext = new MLContext();

            // Load the model
            ITransformer trainedModel = mlContext.Model.Load(_modelPath, out var modelInputSchema);

            var predictionEngine = mlContext.Model.CreatePredictionEngine<HotelRecommendationData, HotelRecommendationPrediction>(trainedModel);

            // Load sample test data
            var testData = mlContext.Data.LoadFromTextFile<HotelRecommendationData>(
                path: dataPath,
                hasHeader: true,
                separatorChar: ',');

            var predictions = mlContext.Data.CreateEnumerable<HotelRecommendationData>(testData, reuseRowObject: false);

            foreach (var item in predictions)
            {
                var prediction = predictionEngine.Predict(item);
                Console.WriteLine($"Hotel: {item.HotelId}, Predicted: {prediction.PredictedLabel}, Score: {prediction.Score:F2}, Probability: {prediction.Probability:P2}");
            }
        }
    }
}
