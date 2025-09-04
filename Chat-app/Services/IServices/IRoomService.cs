using Chat_app.Models;

namespace Chat_app.Services.IServices;

public interface IRoomService
{
	public IEnumerable<Room> GetAllRooms();

	public Room? GetRoomByName(string roomName);

	public Room? GetRoomById(int roomId);

	public void AddRoom(Room room);

	public void UpdateRoom(Room room);	

	public void DeleteRoom(int roomId);

	public bool RoomExists(string roomName);

	public bool RoomExists(int roomId);

	public void ClearMessages(int roomId);

	public void ClearMessages(string roomName);

	public void AddUserToRoom(string userName, string roomName);
}
