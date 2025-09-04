using Chat_app.Models;
using Chat_app.Services.IServices;

namespace Chat_app.Services;

public class RoomService : IRoomService
{
	private readonly IList<Room> _rooms = new List<Room>
		{
			new Room(1, "General"),
			new Room(2, "Sports"),
			new Room(3, "Technology"),
			new Room(4, "Music"),
			new Room(5, "Movies"),
			new Room(6, "Gaming"),
			new Room(7, "Travel"),
			new Room(8, "Food"),
			new Room(9, "Health"),
			new Room(10, "Science"),
			new Room(11, "Art"),
			new Room(12, "History")
		};
	public void AddRoom(Room room)
	{
		throw new NotImplementedException();
	}

	public void AddUserToRoom(string userName, string roomName)
	{
		throw new NotImplementedException();
	}

	public void ClearMessages(int roomId)
	{
		throw new NotImplementedException();
	}

	public void ClearMessages(string roomName)
	{
		throw new NotImplementedException();
	}

	public void DeleteRoom(int roomId)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<Room> GetAllRooms()
	{
		return _rooms;
	}

	public Room? GetRoomById(int roomId)
	{
		return _rooms.FirstOrDefault(r => r.Id == roomId);
	}

	public Room? GetRoomByName(string roomName)
	{
		return _rooms.FirstOrDefault(r => r.Name == roomName);
	}

	public bool RoomExists(string roomName)
	{
		return _rooms.Any(r => r.Name == roomName);
	}

	public bool RoomExists(int roomId)
	{
		return _rooms.Any(r => r.Id == roomId);
	}

	public void UpdateRoom(Room room)
	{
		throw new NotImplementedException();
	}
}
