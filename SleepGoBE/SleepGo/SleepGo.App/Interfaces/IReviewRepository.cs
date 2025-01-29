using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<PaginationResponseDto<Review>> GetAllPagedReviewsByHotelIdAsync(Guid hotelId, int pageIndex, int pageSize);
        Task<PaginationResponseDto<Review>> GetAllPagedReviewsByUserIdAsync(Guid userId, int pageIndex, int pageSize);
        Task<ICollection<Review>> GetAllReviewsByUserIdAsync(Guid userId);
        Task<ICollection<Review>> GetAllReviewsByHotelIdAsync(Guid hotelId);
    }
}
