using Chat_app.Models;
using Chat_app.Services.IServices;
using Microsoft.AspNetCore.SignalR;

namespace Chat_app.Hubs;

public record User(string Name, string Room);

public class ChatHub : Hub
{
	private static readonly Dictionary<string, User> _users = new();
	private readonly IRoomService _roomService;

	public ChatHub(IRoomService roomService)
	{
		_roomService = roomService;
	}

	public async Task<IEnumerable<Room>> GetRooms()
	{
		return await Task.FromResult(_roomService.GetAllRooms());
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		if (_users.TryGetValue(Context.ConnectionId, out var user))
		{
			_users.Remove(Context.ConnectionId);

			var room = _roomService.GetRoomByName(user.Room);
			room?.RemoveUser(user.Name);

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Room);

			// Notify Others
			await Clients.Group(user.Room).SendAsync("UserLeft", user.Name);

			// Update user list
			var usersInRoom = _roomService.GetRoomByName(user.Room)?.Users ?? Enumerable.Empty<string>();
			await Clients.Group(user.Room).SendAsync("UpdateUserList", usersInRoom);
		}
		await base.OnDisconnectedAsync(exception);
	}

	public async Task JoinRoom(string userName, string roomName)
	{
		_roomService.AddUserToRoom(userName, roomName);
		_users[Context.ConnectionId] = new User(userName, roomName);

		await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

		// Notify others in the room
		await Clients.Group(roomName).SendAsync("UserJoined", userName);

		// Send updated user list to everyone
		var usersInRoom = _roomService.GetRoomByName(roomName)?.Users ?? Enumerable.Empty<string>();
		await Clients.Group(roomName).SendAsync("UpdateUserList", usersInRoom);
	}

	public async Task LeaveRoom()
	{
		if(_users.TryGetValue(Context.ConnectionId, out var user))
		{
			_users.Remove(Context.ConnectionId);

			var room = _roomService.GetRoomByName(user.Room);
			room?.RemoveUser(user.Name);

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Room);
			
			//Notify Others
			await Clients.Group(user.Room).SendAsync("UserLeft", user.Name);

			// Send updated User list
			var usersInRoom = _roomService.GetRoomByName(user.Room)?.Users ?? Enumerable.Empty<string>();
			await Clients.Group(user.Room).SendAsync("UpdateUserList", usersInRoom);
		}
	}

	public async Task SendMessageToRoom(string roomName, string content)
	{
		if (_users.TryGetValue(Context.ConnectionId, out var user))
		{
			var message = new Message(user.Name, content);

			var room = _roomService.GetRoomByName(roomName);
			room?.AddMessage(message);

			await Clients.Group(roomName).SendAsync("ReceiveMessage", message);
		}
	}
}