using Chat_app.Models;
using Chat_app.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chat_app.Pages;

public class RoomModel : PageModel
{
	private readonly IRoomService _roomService;

	public RoomModel(IRoomService roomService)
	{
		_roomService = roomService;	
	}

	[BindProperty(SupportsGet =true)]
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public List<string> Users { get; set; } = new ();
	public List<Message> Messages { get; set; } = new ();


	public IActionResult OnGet()
	{
		var room = _roomService.GetRoomById(Id);
		if (room == null)
			return NotFound();
		
		Id = room.Id;
		Name = room.Name;
		Users = room.Users.ToList();
		Messages = room.Messages.ToList();
		return Page();	
	}
}
