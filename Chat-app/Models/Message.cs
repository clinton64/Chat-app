using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat_app.Models;

public class Message
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

	public string RoomName { get; set; } = string.Empty;
	public string User { get; set; } = string.Empty;
	public string Content { get; set; } = string.Empty;
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;

	public Message() { }

	public Message(string user, string content)
	{
		User = user;
		Content = content;
		Timestamp = DateTime.UtcNow;
	}
}
