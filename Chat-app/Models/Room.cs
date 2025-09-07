using System.Collections.Concurrent;

namespace Chat_app.Models;

public class Room
{
	public int Id { get; set; }
	public string Name { get; set; }

	private ConcurrentDictionary<string, bool> _users = new();
	private ConcurrentQueue<Message> _messages = new();

	public Room(int id, string name)
	{
		Id = id;
		Name = name;
	}

	// Users
	public IEnumerable<string> Users => _users.Keys;

	public bool AddUser(string userName) => _users.TryAdd(userName, true);

	public bool RemoveUser(string userName) => _users.TryRemove(userName, out _);

	// Messages
	public IEnumerable<Message> Messages => _messages.ToList();

	public void AddMessage(Message message) => _messages.Enqueue(message);

	public void ClearMessages()
	{
		while (_messages.TryDequeue(out _)) { }
	}

	public override string ToString()
	{
		return $"Room: {Name}, Users: {string.Join(", ", _users.Keys)}, Messages: {string.Join("; ", _messages)}";
	}
}
