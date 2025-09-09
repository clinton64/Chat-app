using Chat_app.Models;
using Chat_app.Services.IServices;
using Microsoft.AspNetCore.SignalR;

namespace Chat_app.Hubs;

public record ConnectedUser(string Name, string Room);

public class ChatHub : Hub
{
	private static readonly Dictionary<string, ConnectedUser> _connections = new();
	private readonly IRoomService _roomService;
	private readonly IMessageService _messageService;

	public ChatHub(IRoomService roomService, IMessageService messageService)
	{
		_roomService = roomService;
		_messageService = messageService;
	}

	// Get list of rooms (SQL)
	public async Task<IEnumerable<Room>> GetRooms()
	{
		return await _roomService.GetAllRoomsAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		if (_connections.TryGetValue(Context.ConnectionId, out var user))
		{
			_connections.Remove(Context.ConnectionId);

			await _roomService.RemoveUserFromRoomAsync(user.Name, user.Room);
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Room);

			// Notify others
			await Clients.Group(user.Room).SendAsync("UserLeft", user.Name);

			// Update user list
			var usersInRoom = await _roomService.GetUsersInRoomAsync(
				(await _roomService.GetRoomByNameAsync(user.Room))!.Id
			);
			await Clients.Group(user.Room).SendAsync("UpdateUserList", usersInRoom.Select(u => u.Name));
		}

		await base.OnDisconnectedAsync(exception);
	}

	public async Task JoinRoom(string userName, string roomName)
	{
		await _roomService.AddUserToRoomAsync(userName, roomName);
		_connections[Context.ConnectionId] = new ConnectedUser(userName, roomName);

		await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

		// Notify others
		await Clients.Group(roomName).SendAsync("UserJoined", userName);

		// Updated user list
		var room = await _roomService.GetRoomByNameAsync(roomName);
		if (room != null)
		{
			var usersInRoom = await _roomService.GetUsersInRoomAsync(room.Id);
			await Clients.Group(roomName).SendAsync("UpdateUserList", usersInRoom.Select(u => u.Name));
		}
	}

	public async Task LeaveRoom()
	{
		if (_connections.TryGetValue(Context.ConnectionId, out var user))
		{
			_connections.Remove(Context.ConnectionId);

			await _roomService.RemoveUserFromRoomAsync(user.Name, user.Room);
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Room);

			// Notify others
			await Clients.Group(user.Room).SendAsync("UserLeft", user.Name);

			// Updated user list
			var room = await _roomService.GetRoomByNameAsync(user.Room);
			if (room != null)
			{
				var usersInRoom = await _roomService.GetUsersInRoomAsync(room.Id);
				await Clients.Group(user.Room).SendAsync("UpdateUserList", usersInRoom.Select(u => u.Name));
			}
		}
	}

	public async Task SendMessageToRoom(string roomName, string content)
	{
		if (_connections.TryGetValue(Context.ConnectionId, out var user))
		{
			var message = new Message(user.Name, content);

			// Save message in MongoDB
			await _messageService.SaveMessageAsync(roomName, message);

			// Broadcast
			await Clients.Group(roomName).SendAsync("ReceiveMessage", message);
		}
	}
}
