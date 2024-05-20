using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Interfaces.Clinics
{
	public interface IStockOutRepo
	{
		public Task<List<StockOutDto>> GetAllStockOutAsync();
		public Task<StockOutDto> GetStockOutByIdAsync(int id);
		public Task<StockOutDto> GetStockOutByProductIdAndClinicIdAsync(int productId, int clinicId);
		public Task<StockOut> AddStockOutAsync(StockOutCreate stockOutCreate);
		public Task UpdateStockOutAsync(StockOutCreate stockOutCreate);
		public Task DeleteStockOutAsync(int id);
	}
}
