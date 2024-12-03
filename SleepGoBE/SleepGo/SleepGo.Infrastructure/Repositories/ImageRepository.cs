using Microsoft.EntityFrameworkCore;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly SleepGoDbContext _context;

        public ImageRepository(SleepGoDbContext context)
        {
            _context = context;
        }

        public async Task RemoveImage(Guid id)
        {
            var image = await _context.Images.SingleOrDefaultAsync(x => x.Id == id);
            if (image != null)
            {
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Image> GetImageById(Guid id)
        {
            return await _context.Images.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UploadImage(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }
    }
}
