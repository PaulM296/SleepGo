using Microsoft.EntityFrameworkCore;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.Domain.Enums;
using SleepGo.Infrastructure.Exceptions;

namespace SleepGo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SleepGoDbContext _context;

        public UserRepository(SleepGoDbContext context)
        {
            _context = context;    
        }
        public void ChangePassword(Guid id, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetByIdAsync(Guid id)
        {
            var user = await _context.Set<AppUser>()
            .Include(u => u.UserProfile)
            .Include(u => u.Hotel)
                .ThenInclude(h => h.Rooms)
            .Include(u => u.Hotel)
                .ThenInclude(h => h.Amenities)
            .Include(u => u.Hotel)
                .ThenInclude(h => h.Reviews)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID {id} not found.");
            }

            return user;
        }

        public async Task<AppUser> RemoveUserAsync(AppUser user)
        {
            var entityToRemove = await _context.Set<AppUser>().FirstOrDefaultAsync(u => u.Id == user.Id);

            if (entityToRemove == null)
            {
                throw new EntityNotFoundException($"The {nameof(AppUser)} does not exist, therefore it could not be removed.");
            }

            _context.Set<AppUser>().Remove(entityToRemove);
            await _context.SaveChangesAsync();

            return entityToRemove;
        }

        public async Task<AppUser> UpdateUserAsync(AppUser updatedUser)
        {
            var user = await _context.Set<AppUser>().FirstOrDefaultAsync(e => e.Id == updatedUser.Id);

            if (user == null)
            {
                throw new EntityNotFoundException($"The {nameof(AppUser)} has not been found, therefore it could not be removed.");
            }

            _context.Set<AppUser>().Update(updatedUser);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<PaginationResponseDto<AppUser>> GetPaginatedUsersByIdAsync(int pageIndex, int pageSize)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                    .ThenInclude(i => i.Image)
                .Where(u => u.Role == Role.User)
                .OrderBy(u => u.UserName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Users.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<AppUser>(user, pageIndex, totalPages);
        }

        public async Task<PaginationResponseDto<AppUser>> GetPaginatedHotelsByIdAsync(int pageIndex, int pageSize)
        {
            var user = await _context.Users
                .Include(u => u.Hotel)
                    .ThenInclude(i => i.Image)
                .Where(u => u.Role == Role.Hotel)
                .OrderBy(u => u.UserName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Users.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<AppUser>(user, pageIndex, totalPages);
        }
    }
}
