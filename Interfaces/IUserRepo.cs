using MediNet_BE.Dto.Users;
using MediNet_BE.DtoCreate.Users;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Interfaces
{
    public interface IUserRepo<T, TDto, TCreate> where T : User where TDto : UserDto where TCreate : UserCreate
	{
		public Task<List<TDto>> GetAllUserAsync();
		public Task<TDto> GetUserByIdAsync(int id);
		public Task<List<TDto>> GetUserByTotalAmountOrderAsync();
		public Task<TDto> GetUserByEmailAsync(string email);
		public Task<T> AddUserAsync(TCreate userCreate);
		public Task UpdateUserAsync(TCreate userCreate);
		public Task DeleteUserAsync(int userid);
	}
}
