using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Interfaces
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        Task<PaginationResponseDto<Reservation>> GetAllPagedReservationsByUserIdAsync(Guid userId, int pageIndex, int pageSize);
        Task<PaginationResponseDto<Reservation>> GetAllPagedReservationsByHotelIdAsync(Guid hotelId, int pageIndex, int pageSize);
        Task<ICollection<Reservation>> GetAllReservationsByUserIdAsync(Guid userId);
        Task<ICollection<Reservation>> GetReservationsByRoomIdAsync(Guid roomId);
    }
}
