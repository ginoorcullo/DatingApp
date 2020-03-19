using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DatingApp2.API.Data.BaseRepository;
using DatingApp2.API.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp2.API.Helpers;
using System;

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

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PageList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = context.Messages
                            .Include(m => m.Sender)
                                .ThenInclude(u => u.Photos)
                            .Include(m => m.Recipient)
                                .ThenInclude(u => u.Photos)
                        .AsQueryable();
            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(m => m.SenderId == messageParams.UserId && m.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDeleted == false && m.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(m => m.MessageSent);
            return await PageList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
            
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await context.Messages
                            .Include(m => m.Sender)
                                .ThenInclude(u => u.Photos)
                            .Include(m => m.Recipient)
                                .ThenInclude(u => u.Photos)
                            .Where(m => m.SenderId == recipientId && m.RecipientId == userId && m.RecipientDeleted == false ||
                                    m.SenderId == userId && m.RecipientId == recipientId && m.SenderDeleted == false)
                        .OrderByDescending(m => m.MessageSent)
                        .ToListAsync();
                                                            
            return messages;
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

        public async Task<PageList<Users>> GetUsers(UserParams userParams)
        {
            var users = context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId)
                        .Where(u => u.Gender == userParams.Gender);

            if (userParams.Likees)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likers)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDOB = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDOB = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateOfBirth >= minDOB && u.DateOfBirth <= maxDOB);
            }

            if (!String.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PageList<Users>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await context.Users
                .Include(x => x.Likers)
                .Include(x => x.Likees)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
            {
                return user.Likers.Where(u => u.LikeeId == id).Select(u => u.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(u => u.LikeeId);
            }

        }
    }
}