using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using System.Diagnostics.Metrics;

namespace SleepGo.Infrastructure.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(SleepGoDbContext context) : base(context)
        {

        }

        public async Task<PaginationResponseDto<Hotel>> GetAllPagedHotelsByCountryAsync(string country, int pageIndex, int pageSize)
        {
            var hotelsByCountry = await _context.Hotels
                .Where(h => h.Country == country)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Hotels.Where(h => h.Country == country).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Hotel>(hotelsByCountry, pageIndex, pageSize);
        }

        public async Task<PaginationResponseDto<Hotel>> GetAllPagedHotelsByNameAsync(string name, int pageIndex, int pageSize)
        {
            var hotelsByName = await _context.Hotels
                .Where(h => h.HotelName == name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Hotels.Where(h => h.HotelName == name).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Hotel>(hotelsByName, pageIndex, pageSize);
        }

        public async Task<PaginationResponseDto<Hotel>> GetAllPagedHotelsAsync(int pageIndex, int pageSize)
        {
            var hotels = await _context.Hotels
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Hotels.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Hotel> (hotels, pageIndex, pageSize);
        }

        public async Task<Hotel> GetAllHotelReviewsByHotelId(Guid hotelId)
        {
            return await _context.Hotels
                .Include(h => h.Reviews)
                .FirstOrDefaultAsync(h => h.Id == hotelId);
        }
    }
}
