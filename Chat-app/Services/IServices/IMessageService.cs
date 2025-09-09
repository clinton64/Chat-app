using Chat_app.Models;

namespace Chat_app.Services.IServices;

public interface IMessageService
{
	Task SaveMessageAsync(string roomName, Message message);
	Task <IEnumerable<Message>> GetMessagesForRoomAsync(string roomName, int limit = 50);	
	Task ClearMessagesAsync(string roomName);	
}
