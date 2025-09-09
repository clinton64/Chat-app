using Chat_app.Data;
using Chat_app.Models;
using Chat_app.Repository;
using Chat_app.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Chat_app.Services;

public class RoomRepository : Repository<Room>, IRoomRepository
{
	private readonly AppDbContext _db;

	public RoomRepository(AppDbContext db) : base(db)
	{
		_db = db;
	}

	public async Task<IEnumerable<User>> GetUsersInRoomAsync(int roomId)
	{
		try
		{
			return await _db.RoomUsers
				.Where(ru => ru.RoomId == roomId)
				.Select(ru => ru.User)
				.ToListAsync();
		}
		catch (Exception ex)
		{
			throw new Exception($"Error fetching users for room {roomId}: {ex.Message}", ex);
		}
	}

	public async Task<bool> AddUserToRoomAsync(string userName, string roomName)
	{
		try
		{
			var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);
			if (room == null) return false;

			var user = await _db.Users.FirstOrDefaultAsync(u => u.Name == userName);
			if (user == null)
			{
				user = new User { Name = userName };
				await _db.Users.AddAsync(user);
				await _db.SaveChangesAsync();
			}

			bool alreadyExists = await _db.RoomUsers.AnyAsync(ru => ru.RoomId == room.Id && ru.UserId == user.Id);
			if (alreadyExists) return false;

			_db.RoomUsers.Add(new RoomUser { RoomId = room.Id, UserId = user.Id });
			await _db.SaveChangesAsync();

			return true;
		}
		catch (Exception ex)
		{
			throw new Exception($"Error adding user '{userName}' to room '{roomName}': {ex.Message}", ex);
		}
	}

	public async Task<bool> RemoveUserFromRoomAsync(string userName, string roomName)
	{
		try
		{
			var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);
			if (room == null) return false;

			var user = await _db.Users.FirstOrDefaultAsync(u => u.Name == userName);
			if (user == null) return false;

			var roomUser = await _db.RoomUsers.FirstOrDefaultAsync(ru => ru.RoomId == room.Id && ru.UserId == user.Id);
			if (roomUser == null) return false;

			_db.RoomUsers.Remove(roomUser);
			await _db.SaveChangesAsync();

			return true;
		}
		catch (Exception ex)
		{
			throw new Exception($"Error removing user '{userName}' from room '{roomName}': {ex.Message}", ex);
		}
	}
}
