using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Interfaces.Clinics
{
	public interface IStockInRepo
	{
		public Task<List<StockInDto>> GetAllStockInAsync();
		public Task<StockInDto> GetStockInByIdAsync(int id);
		public Task<StockInDto> GetStockInByProductIdAndClinicIdAsync(int productId, int clinicId);
		public Task<StockIn> AddStockInAsync(StockInCreate stockInCreate);
		public Task UpdateStockInAsync(StockInCreate stockInCreate);
		public Task DeleteStockInAsync(int id);
	}
}
