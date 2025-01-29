using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.Domain.Entities;
using SleepGo.Domain.Enums;

namespace SleepGo.App.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<PaginationResponseDto<Room>> GetAllPagedRoomsFromHotelByRoomTypeAsync(Guid hotelId, RoomType roomType, int pageIndex, int pageSize);
        Task<PaginationResponseDto<Room>> GetAllPagedAvailableRoomsFromHotelByRoomTypeAsync(Guid hotelId, RoomType roomType, int pageIndex, int pageSize);
        Task<Room?> GetAvailableRoomsFromHotelByRoomTypeAsync(Guid hotelId, RoomType roomType);
        Task<ICollection<Room>> GetRoomsByHotelIdAsync(Guid hotelId);
    }
}
