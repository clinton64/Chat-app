using Chat_app.Models;
using Chat_app.Services.IServices;
using MongoDB.Driver;

namespace Chat_app.Services;

public class MessageService : IMessageService
{
	private readonly IMongoCollection<Message> _messages;

	public MessageService(IConfiguration config)
	{
		var client = new MongoClient(config.GetConnectionString("MongoDb"));
		var database = client.GetDatabase("ChatAppDb");

		_messages = database.GetCollection<Message>("Messages");
	}

	public async Task SaveMessageAsync(string roomName, Message message)
	{
		message.RoomName = roomName;
		await _messages.InsertOneAsync(message);
	}

	public async Task<IEnumerable<Message>> GetMessagesForRoomAsync(string roomName, int limit = 50)
	{
		return await _messages
			.Find(m => m.RoomName == roomName)
			.SortBy(m => m.Timestamp)
			.Limit(limit)
			.ToListAsync();
	}

	public async Task ClearMessagesAsync(string roomName)
	{
		await _messages.DeleteManyAsync(m => m.RoomName == roomName);
	}
}
