namespace Chat_app.Models;

public class Room
{
	public int Id { get; set; }
	public string Name { get; set; }

	public List<string> Users { get; set; } = new List<string>();

	public List<string> Messages { get; set; } = new List<string>();

	public Room(int id, string name)
	{
		Id = id;
		Name = name;
	}

	public void AddUser(string userName)
	{
		if (!Users.Contains(userName))
		{
			Users.Add(userName);
		}
	}

	public void RemoveUser(string userName)
	{
		if (Users.Contains(userName))
		{
			Users.Remove(userName);
		}
	}

	public void AddMessage(string message)
	{
		Messages.Add(message);
	}

	public void ClearMessages()
	{
		Messages.Clear();
	}
	public override string ToString()
	{
		return $"Room: {Name}, Users: {string.Join(", ", Users)}, Messages: {string.Join("; ", Messages)}";
	}
}
