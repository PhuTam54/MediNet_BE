using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Interfaces.Clinics
{
    public interface IInStockRepo
	{
		public Task<List<InStockDto>> GetAllInStockAsync();
		public Task<InStockDto> GetInStockByIdAsync(int id);
		public Task<InStockDto> GetInStockByProductIdAndClinicIdAsync(int productId,int clinicId);
		public Task<List<InStockDto>> GetInStockByProductIdAsync(int productId);
		public Task<InStock> AddInStockAsync(InStockCreate inStockCreate);
		public Task UpdateInStockAsync(InStockCreate inStockCreate);
		public Task DeleteInStockAsync(int id);
	}
}
