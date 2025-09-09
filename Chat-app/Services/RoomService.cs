using Chat_app.Models;
using Chat_app.Repository.IRepository;
using Chat_app.Services.IServices;

namespace Chat_app.Services;

public class RoomService : IRoomService
{
	private readonly IRoomRepository _roomRepository;

	public RoomService(IRoomRepository roomRepository)
	{
		_roomRepository = roomRepository;
	}

	// --- ROOMS ---
	public async Task<IEnumerable<Room>> GetAllRoomsAsync()
	{
		return await _roomRepository.GetAll<Room>();
	}

	public async Task<Room?> GetRoomByIdAsync(int roomId)
	{
		return await _roomRepository.GetFirstOrDefault(filter: r => r.Id == roomId, includeProperties: "RoomUsers");
	}

	public async Task<Room?> GetRoomByNameAsync(string roomName)
	{
		return await _roomRepository.GetFirstOrDefault(filter: r => r.Name.ToLower() == roomName.ToLower(), includeProperties: "RoomUsers");
	}

	public async Task<bool> RoomExistsAsync(string roomName)
	{
		return await _roomRepository.Exists(filter: r => r.Name.ToLower() == roomName.ToLower());
	}

	public async Task<Room> AddRoomAsync(Room room)
	{
		return await _roomRepository.Add(room);
	}

	public async Task DeleteRoomAsync(int roomId)
	{
		var room = await GetRoomByIdAsync(roomId);
		if(room == null) return;

		await _roomRepository.Remove(room);
	}

	// --- USERS IN ROOMS ---
	public async Task<bool> AddUserToRoomAsync(string userName, string roomName)
	{
		return await _roomRepository.AddUserToRoomAsync(userName, roomName);
	}

	public async Task<bool> RemoveUserFromRoomAsync(string userName, string roomName)
	{
		return await _roomRepository.RemoveUserFromRoomAsync(userName, roomName);	
	}

	public async Task<IEnumerable<User>> GetUsersInRoomAsync(int roomId)
	{
		return await _roomRepository.GetUsersInRoomAsync(roomId);
	}
}
