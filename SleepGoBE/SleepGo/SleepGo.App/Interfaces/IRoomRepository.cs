using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.Domain.Entities;
using SleepGo.Domain.Enums;

namespace SleepGo.App.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<PaginationResponseDto<Room>> GetAllPagedRoomsByRoomTypeAsync(RoomType roomType, int pageIndex, int pageSize);
    }
}
