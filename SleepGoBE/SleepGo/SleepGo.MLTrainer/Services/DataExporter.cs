using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SleepGo.Infrastructure;
using SleepGo.MLTrainer.Models;
using System.Globalization;

namespace SleepGo.MLTrainer.Services
{
    public class DataExporter
    {
        private readonly SleepGoDbContext _context;

        public DataExporter(SleepGoDbContext context)
        {
            _context = context;
        }

        public async Task ExportToCsvAsync(string outputPath)
        {
            var data = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(room => room.Hotel)
                    .ToListAsync();

            var records = data
                .Where(r => r.Status == "Successful")
                .Select(r => new HotelRecommendationData
                {
                    UserId = r.UserId.ToString(),
                    HotelId = r.Room.HotelId.ToString(),
                    HotelRating = (float)r.Room.Hotel.Rating,
                    PricePaid = (float)r.Price,
                    City = r.Room.Hotel.City,
                    Country = r.Room.Hotel.Country,
                    RoomType = r.Room.RoomType,
                    Checkin = r.CheckIn,
                    Checkout = r.CheckOut,
                    Label = 1f
                })
                .ToList();

            using var writer = new StreamWriter(outputPath);
            using var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);

            Console.WriteLine($"Exported {records.Count} rows to {outputPath}");
        }
    }
}
