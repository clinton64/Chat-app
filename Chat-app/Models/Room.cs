using System.ComponentModel.DataAnnotations;

namespace Chat_app.Models;

public class Room
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public ICollection<RoomUser> RoomUsers { get; set; } = new List<RoomUser>();
}
