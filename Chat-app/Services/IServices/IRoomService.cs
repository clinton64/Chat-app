using Chat_app.Models;

namespace Chat_app.Services.IServices;

public interface IRoomService
{
	// Rooms
	Task<IEnumerable<Room>> GetAllRoomsAsync();
	Task<Room?> GetRoomByIdAsync(int roomId);
	Task<Room?> GetRoomByNameAsync(string roomName);
	Task<bool> RoomExistsAsync(string roomName);
	Task<Room> AddRoomAsync(Room room);
	Task DeleteRoomAsync(int roomId);

	// Users in rooms
	Task<bool> AddUserToRoomAsync(string userName, string roomName);
	Task<bool> RemoveUserFromRoomAsync(string userName, string roomName);
	Task<IEnumerable<User>> GetUsersInRoomAsync(int roomId);
}
