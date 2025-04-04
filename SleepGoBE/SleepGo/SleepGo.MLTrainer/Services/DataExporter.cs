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
                    RoomType = (int)r.Room.RoomType,
                    Label = true
                })
                .ToList();

            var negativeRecords = new List<HotelRecommendationData>();

            var allUsers = await _context.Users.ToListAsync();
            var allRooms = await _context.Rooms.Include(r => r.Hotel).ToListAsync();
            var reservedRoomIds = data.Select(r => r.RoomId).ToHashSet();

            // For each user, create a few fake "not reserved" examples
            foreach (var user in allUsers)
            {
                var random = new Random();

                // Pick 3 rooms the user hasn't reserved
                var unreservedRooms = allRooms
                    .Where(r => !data.Any(d => d.UserId == user.Id && d.RoomId == r.Id))
                    .OrderBy(_ => random.Next())
                    .Take(3);

                foreach (var room in unreservedRooms)
                {
                    negativeRecords.Add(new HotelRecommendationData
                    {
                        UserId = user.Id.ToString(),
                        HotelId = room.HotelId.ToString(),
                        HotelRating = (float)room.Hotel.Rating,
                        PricePaid = (float)room.Price,
                        City = room.Hotel.City,
                        Country = room.Hotel.Country,
                        RoomType = (int)room.RoomType,
                        Label = false
                    });
                }
            }

            var directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            records.AddRange(negativeRecords);

            using var writer = new StreamWriter(outputPath);
            using var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);

            Console.WriteLine($"Exported {records.Count} rows ({records.Count(r => r.Label)} positive, {records.Count(r => !r.Label)} negative) to {outputPath}");
        }
    }
}
