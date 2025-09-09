using Chat_app.Models;
using Chat_app.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chat_app.Pages;

public class RoomModel : PageModel
{
	private readonly IRoomService _roomService;
	private readonly IMessageService _messageService;

	public RoomModel(IRoomService roomService, IMessageService messageService)
	{
		_roomService = roomService;	
		_messageService = messageService;
	}

	[BindProperty(SupportsGet =true)]
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public List<string> Users { get; set; } = new ();
	public List<Message> Messages { get; set; } = new ();


	public async Task<IActionResult> OnGetAsync()
	{
		var room = await _roomService.GetRoomByIdAsync(Id);
		if (room == null)
			return NotFound();
		
		Id = room.Id;
		Name = room.Name;
		Users = _roomService.GetUsersInRoomAsync(room.Id).Result.Select(u => u.Name).ToList();
		Messages = _messageService.GetMessagesForRoomAsync(Name).Result.ToList();
		return Page();	
	}
}
