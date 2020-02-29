using DatingApp2.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatingApp2.API.Data
{
    public class DataContext : DbContext
    {
       
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {            
        }

        #region ALTERNATIVE CONNECTION STRING DECLARATION

        /* 
            Alternative to constructor setup of connection string which I assume to handle the default connection string in app settings
            This allows you to select the name of your connection string.
         */
        //private readonly IConfiguration config;

        // public DataContext(IConfiguration config)
        // {
        //     this.config = config;
        // }
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer(config.GetSection("ConnectionStrings:FakeConnection").Value);
        // }

        #endregion

        public DbSet<Values> Values { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}