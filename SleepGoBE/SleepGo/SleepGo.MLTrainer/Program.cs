using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SleepGo.Infrastructure;
using SleepGo.MLTrainer.Services;

var services = new ServiceCollection();

services.AddDbContext<SleepGoDbContext>(options =>
    options.UseSqlServer("Your_Connection_String_Here"));

var trainer = new HotelRecommendationTrainer();

string trainingDataPath = "Data/hotel_data.csv";

trainer.Train(trainingDataPath);
trainer.Test(trainingDataPath);
