using Chat_app.Models;
using Chat_app.Services.IServices;
using System.Collections.Concurrent;

namespace Chat_app.Services;

public class RoomService : IRoomService
{
	private ConcurrentDictionary<int, Room> _rooms = new();
	public RoomService()
		{
		var seedRooms = new List<Room> {
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
		foreach(var r in seedRooms)
			_rooms.TryAdd(r.Id, r);
	}
	public void AddRoom(Room room)
	{
		if (RoomExists(room.Name))
			return;

		_rooms.TryAdd(room.Id, room);
	}

	public void AddUserToRoom(string userName, string roomName)
	{
		var room = GetRoomByName(roomName);

		room?.AddUser(userName);
	}

	public void ClearMessages(int roomId)
	{
		if (_rooms.TryGetValue(roomId, out var room))
			room.ClearMessages();
	}

	public void ClearMessages(string roomName)
	{
		var room = GetRoomByName(roomName);
		room?.ClearMessages();
	}

	public void DeleteRoom(int roomId)
	{
		_rooms.TryRemove(roomId, out _);
	}

	public IEnumerable<Room> GetAllRooms() => _rooms.Values;

	public Room? GetRoomById(int roomId)
		=>  _rooms.TryGetValue(roomId, out var room) ? room : null;
	

	public Room? GetRoomByName(string roomName)
		=> _rooms.Values.FirstOrDefault(r => r.Name.Equals(roomName, StringComparison.OrdinalIgnoreCase));

	public bool RoomExists(string roomName)
		=> _rooms.Values.Any(r => r.Name.Equals(roomName, StringComparison.OrdinalIgnoreCase));
	

	public bool RoomExists(int roomId)
		=> _rooms.ContainsKey(roomId);

	public void UpdateRoom(Room room)
	{
		_rooms.AddOrUpdate(room.Id, room, (id, existing) => room);
	}
}
