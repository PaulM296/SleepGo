using Microsoft.EntityFrameworkCore;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(SleepGoDbContext context) : base(context)
        {

        }

        public async Task<PaginationResponseDto<Reservation>> GetAllPagedReservationsByUserIdAsync(Guid userId, int pageIndex, int pageSize)
        {
            var userReservations = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(room => room.Hotel)
                .Include(au => au.AppUser)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CheckIn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Reservations.Where(r => r.UserId == userId).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Reservation>(userReservations, pageIndex, totalPages);
        }

        public async Task<PaginationResponseDto<Reservation>> GetAllPagedReservationsByHotelIdAsync(Guid hotelId, int pageIndex, int pageSize)
        {
            var hotelReservations = await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(room => room.Hotel)
                .Include(au => au.AppUser)
                .Where(r => r.Room.HotelId == hotelId)
                .OrderByDescending(r => r.CheckIn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Reservations.Where(r => r.Room.HotelId == hotelId).CountAsync();
            var totalPages = (int)Math.Ceiling((count / (double)pageSize));

            return new PaginationResponseDto<Reservation>(hotelReservations, pageIndex, totalPages);
        }

        public async Task<ICollection<Reservation>> GetAllReservationsByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(room => room.Hotel)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<ICollection<Reservation>> GetReservationsByRoomIdAsync(Guid roomId)
        {
            return await _context.Reservations
                .Where(res => res.RoomId == roomId)
                .ToListAsync();
        }
    }
}
