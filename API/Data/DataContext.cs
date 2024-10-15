using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int,
IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
IdentityUserToken<int>>(options)
{
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>()
            .HasMany(userRole => userRole.UserRoles)
            .WithOne(user => user.User)
            .HasForeignKey(userRole => userRole.UserId)
            .IsRequired();
        builder.Entity<AppRole>()
            .HasMany(userRole => userRole.UserRoles)
            .WithOne(user => user.Role)
            .HasForeignKey(userRole => userRole.RoleId)
            .IsRequired();
        builder.Entity<UserLike>()
            .HasKey(key => new { key.SourceUserId, key.TargetUserId });
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
        builder.Entity<Message>()
            .HasOne(x => x.Recipient)
            .WithMany(x => x.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
    }
}