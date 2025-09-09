using Chat_app.Models;

namespace Chat_app.Repository.IRepository;

public interface IRoomRepository: IRepository<Room>
{

	Task<IEnumerable<User>> GetUsersInRoomAsync(int roomId);

	Task<bool> AddUserToRoomAsync(string userName, string roomName);	

	Task<bool> RemoveUserFromRoomAsync(string userName, string roomName);
}
