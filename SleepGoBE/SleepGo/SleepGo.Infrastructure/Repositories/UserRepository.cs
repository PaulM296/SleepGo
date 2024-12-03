using Microsoft.EntityFrameworkCore;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
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
            var user = await _context.Set<AppUser>().FirstOrDefaultAsync(u => u.Id == id);

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
    }
}
