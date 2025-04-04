using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SleepGo.Infrastructure;
using SleepGo.MLTrainer.Services;

var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var connectionString = configuration.GetConnectionString("Default");
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string is missing!");
    return;
}

services.AddDbContext<SleepGoDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddTransient<DataExporter>();
services.AddTransient<HotelRecommendationTrainer>();

var provider = services.BuildServiceProvider();

var exporter = provider.GetRequiredService<DataExporter>();
await exporter.ExportToCsvAsync("Data/hotel_data.csv");

var trainer = new HotelRecommendationTrainer();

string trainingDataPath = "Data/hotel_data.csv";

trainer.Train(trainingDataPath);
trainer.Test(trainingDataPath);

Console.WriteLine("Training completed!");