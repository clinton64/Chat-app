using Chat_app.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat_app.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Room> Rooms { get; set; } = null!;
	public DbSet<RoomUser> RoomUsers { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<RoomUser>()
			.HasKey(ru => new { ru.RoomId, ru.UserId });

		modelBuilder.Entity<RoomUser>()
			.HasOne(ru => ru.Room)
			.WithMany(r => r.RoomUsers)
			.HasForeignKey(ru => ru.RoomId);
		modelBuilder.Entity<RoomUser>()
			.HasOne(ru => ru.User)
			.WithMany(u => u.RoomUsers)
			.HasForeignKey(ru => ru.UserId);
	}
}
