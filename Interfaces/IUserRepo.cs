using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Interfaces
{
    public interface IUserRepo<T, TDto> where T : User where TDto : UserDto
	{
		public Task<List<T>> GetAllUserAsync();
		public Task<T> GetUserByIdAsync(int id);
		public Task<T> GetUserByEmailAsync(string email);
		public Task<T> AddUserAsync(TDto userDto);
		public Task UpdateUserAsync(TDto userDto);
		public Task DeleteUserAsync(int userid);
	}
}
