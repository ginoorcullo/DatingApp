using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DatingApp2.API.Data.BaseRepository;
using DatingApp2.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp2.API.Data.ImpRepository
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext context;
        public DatingRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await context.Photos.FirstOrDefaultAsync(x => x.Id == id);
            return photo;
        }

        public async Task<Users> GetUser(int id)
        {
            var user = await context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<Photo> GetUserMainPhoto(int userId)
        {
            var photo = await context.Photos.Where(x => x.UsersId == userId).FirstOrDefaultAsync(p => p.IsMain);                
            return photo;
        }

        public async Task<IEnumerable<Users>> GetUsers()
        {
            var users = await context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}