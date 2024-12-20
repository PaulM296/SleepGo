using Microsoft.EntityFrameworkCore;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.Domain.Enums;

namespace SleepGo.Infrastructure.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(SleepGoDbContext context) : base(context)
        {

        }

        public async Task<PaginationResponseDto<Room>> GetAllPagedRoomsByRoomTypeAsync(RoomType roomType, int pageIndex, int pageSize)
        {
            var roomsByRoomType = await _context.Rooms
                .Include(h => h.Hotel)
                .Where(r => r.RoomType == roomType)
                .OrderByDescending(r => r.Price)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Rooms.Where(r => r.RoomType == roomType).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Room>(roomsByRoomType, pageIndex, totalPages);
        }
    }
}
