using Microsoft.EntityFrameworkCore;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(SleepGoDbContext context) : base(context)
        {

        }

        public async Task<PaginationResponseDto<Review>> GetAllPagedReviewsByHotelIdAsync(Guid hotelId, int pageIndex, int pageSize)
        {
            var reviewsForHotel = await _context.Reviews
                .Include(au => au.AppUser)
                .ThenInclude(u => u.UserProfile)
                .Where(r => r.HotelId == hotelId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Reviews.Where(r => r.HotelId == hotelId).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Review>(reviewsForHotel, pageIndex, pageSize);
        }

        public async Task<PaginationResponseDto<Review>> GetAllPagedReviewsByUserIdAsync(Guid userId, int pageIndex, int pageSize)
        {
            var reviewsByUser = await _context.Reviews
               .Include(au => au.AppUser)
               .ThenInclude(u => u.UserProfile)
               .Where(r => r.UserId == userId)
               .OrderByDescending(r => r.CreatedAt)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            var count = await _context.Reviews.Where(r => r.UserId == userId).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Review>(reviewsByUser, pageIndex, pageSize);
        }

        public async Task<ICollection<Review>> GetAllReviewsByUserIdAsync(Guid userId)
        {
            return await _context.Reviews
                .Include(r => r.Hotel)
                .Include(r => r.AppUser)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
