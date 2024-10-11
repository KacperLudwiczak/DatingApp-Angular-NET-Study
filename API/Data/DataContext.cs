using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserLike>()
            .HasKey(key => new {key.SourceUserId, key.TargetUserId});
        builder.Entity<UserLike>()
            .HasOne(source => source.SourceUser)
            .WithMany(liked => liked.LikedUsersByOurUser)
            .HasForeignKey(source => source.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<UserLike>()
            .HasOne(source => source.TargetUser)
            .WithMany(liked => liked.UsersWhoLikedOurUser)
            .HasForeignKey(source => source.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}