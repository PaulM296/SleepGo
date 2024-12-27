using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Interfaces
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<PaginationResponseDto<Hotel>> GetAllPagedHotelsByCountryAsync(string country, int pageIndex, int pageSize);
        Task<PaginationResponseDto<Hotel>> GetAllPagedHotelsByNameAsync(string name, int pageIndex, int pageSize);
        Task<PaginationResponseDto<Hotel>> GetAllPagedHotelsAsync(int pageIndex, int pageSize);
        Task<Hotel> GetAllHotelReviewsByHotelId(Guid hotelId);
    }
}
