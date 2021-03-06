using HRM.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace HRM.Data
{
    public class Context : IdentityDbContext
    {
        //  Enable-Migrations -Context HRM.Data.ModelsEnable-Migrations -Context HRM.Data.Models
        public Context(DbContextOptions<Context> options) : base(options)
        { }

#pragma warning disable CS0114 // 'Context.Users' hides inherited member 'IdentityUserContext<IdentityUser, string, IdentityUserClaim<string>, IdentityUserLogin<string>, IdentityUserToken<string>>.Users'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public DbSet<User> Users { get; set; }
#pragma warning restore CS0114 // 'Context.Users' hides inherited member 'IdentityUserContext<IdentityUser, string, IdentityUserClaim<string>, IdentityUserLogin<string>, IdentityUserToken<string>>.Users'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Reservation> Reservations { get; set; } 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Reservation>()
            //   .H(x => new { x.CustomerId, x.RoomId });
            builder.Entity<Reservation>()
                .HasOne(x => x.Customer).WithMany(x => x.Reservations).HasForeignKey(x => x.CustomerId); 

            base.OnModelCreating(builder);
        }
    }
}   
