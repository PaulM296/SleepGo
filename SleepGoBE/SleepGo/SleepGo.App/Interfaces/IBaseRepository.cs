using SleepGo.Domain.Entities.BaseEntities;

namespace SleepGo.App.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> RemoveAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<ICollection<T>> GetAllAsync();
    }
}
