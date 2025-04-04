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

        public async Task<PaginationResponseDto<Room>> GetAllPagedRoomsFromHotelByRoomTypeAsync(Guid hotelId, RoomType roomType, int pageIndex, int pageSize)
        {
            var roomsByRoomType = await _context.Rooms
                .Include(h => h.Hotel)
                .Where(r => r.HotelId == hotelId && r.RoomType == roomType)
                .OrderByDescending(r => r.Price)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Rooms.Where(r => r.HotelId == hotelId && r.RoomType == roomType).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Room>(roomsByRoomType, pageIndex, totalPages);
        }

        public async Task<PaginationResponseDto<Room>> GetAllPagedAvailableRoomsFromHotelByRoomTypeAsync(Guid hotelId, RoomType roomType, int pageIndex, int pageSize)
        {
            var availableRooms = await _context.Rooms
                .Include(h => h.Hotel)
                .Where(r => r.HotelId == hotelId && r.RoomType == roomType && !r.IsReserved)
                .OrderByDescending(r => r.Price)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Rooms.Where(r => r.HotelId == hotelId && r.RoomType == roomType && !r.IsReserved).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Room>(availableRooms, pageIndex, totalPages);
        }

        public async Task<Room?> GetAvailableRoomsFromHotelByRoomTypeAsync(Guid hotelId, RoomType roomType)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId && r.RoomType == roomType && !r.IsReserved)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Room>> GetRoomsByHotelIdAsync(Guid hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();
        }

        public async Task<ICollection<Room>> GetAvailableRoomsByHotelIdAsync(Guid hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId && !r.IsReserved)
                .ToListAsync();
        }

        public async Task<ICollection<Room>> GetAllRoomsByHotelIdAsync(Guid hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();
        }

        public IQueryable<Room> GetQueryable()
        {
            return _context.Rooms.Include(r => r.Hotel);
        }

    }
}
