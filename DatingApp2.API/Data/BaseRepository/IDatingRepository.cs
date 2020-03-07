using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp2.API.Models;

namespace DatingApp2.API.Data.BaseRepository
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();

         Task<IEnumerable<Users>> GetUsers();
         Task<Users> GetUser(int id);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetUserMainPhoto(int userId);
    }    
}