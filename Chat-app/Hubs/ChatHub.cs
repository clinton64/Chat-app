using Chat_app.Models;
using Chat_app.Services.IServices;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Chat_app.Hubs;

public record User(string Name, string Room);
public record Message(string User, string Content);

public class ChatHub : Hub
{
	private static ConcurrentDictionary<string, User> _users = new();
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
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Room);
			await Clients.Group(user.Room).SendAsync("UserLeft", user.Name);
		}
	}

	public async Task JoinRoom(string userName, string roomName)
	{
		_users.TryAdd(Context.ConnectionId, new User(userName, roomName));
		await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
		await Clients.Group(roomName).SendAsync("UserJoined", userName);
	}

	public async Task SendMessageToRoom(string roomName, string content)
	{
		var message = new Message(_users[Context.ConnectionId].Name, content);
		await Clients.Group(roomName).SendAsync("ReceiveMessage", message);
	}
}