using MediNet_BE.Dto.Clinics;
using MediNet_BE.DtoCreate.Clinics;
using MediNet_BE.Models.Clinics;

namespace MediNet_BE.Interfaces.Clinics
{
    public interface ISupplyRepo
	{
		public Task<List<SupplyDto>> GetAllSupplyAsync();
		public Task<SupplyDto> GetSupplyByIdAsync(int id);
		public Task<SupplyDto> GetSupplyByProductIdAndClinicIdAsync(int productId,int clinicId);
		public Task<Supply> AddSupplyAsync(SupplyCreate supplyCreate);
		public Task UpdateSupplyAsync(SupplyCreate supplyCreate);
		public Task DeleteSupplyAsync(int id);
	}
}
