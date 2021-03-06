﻿using Microsoft.EntityFrameworkCore;

namespace DatingApp2.API.Models
{
    /// <summary>
    /// Extends DataContext OnModelCreation override.
    /// </summary>
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Builds the Like model.
        /// </summary>
        /// <param name="builder"></param>
        public static void BuildLikes(this ModelBuilder builder)
        {
            builder.Entity<Like>()
                .HasKey(k => new {k.LikeeId, k.LikerId});
            
            builder.Entity<Like>()
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Builds the Message model.
        /// </summary>
        /// <param name="builder"></param>
        public static void BuildMessages(this ModelBuilder builder)
        {
            /* To customize primary key if other naming convention is preferred instead of 'Id'*/
            // builder.Entity<Message>()
            //     .HasKey(x => x.MessageId);
            
            builder.Entity<Message>()
                .Property(m => m.Content).HasColumnType("NVARCHAR(MAX)");

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
        
    }
}