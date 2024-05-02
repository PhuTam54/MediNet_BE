using MediNet_BE.Dto.Clinics;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Interfaces.Clinics
{
    public interface ISupplyRepo
	{
		public Task<List<Supply>> GetAllSupplyAsync();
		public Task<Supply> GetSupplyByIdAsync(int id);
		public Task<Supply> GetSupplyByProductIdAndClinicIdAsync(int productId,int clinicId);
		public Task<Supply> AddSupplyAsync(SupplyDto supplyDto);
		public Task UpdateSupplyAsync(SupplyDto supplyDto);
		public Task DeleteSupplyAsync(int id);
	}
}
