using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories
{
	public class ServiceRepo : IServiceRepo
	{
		private readonly MediNetContext _context;
		private readonly IMapper _mapper;

		public ServiceRepo(MediNetContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<Service>> GetAllServiceAsync()
		{
			var services = await _context.Services!.ToListAsync();
			return services;
		}

		public async Task<Service> GetServiceByIdAsync(int id)
		{
			var service = await _context.Services!.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
			return service;
		}

		public async Task<Service> AddServiceAsync(ServiceDto serviceDto)
		{
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == serviceDto.ClinicId);
			var serviceMap = _mapper.Map<Service>(serviceDto);
			serviceMap.Clinic = clinic;

			_context.Services!.Add(serviceMap);
			await _context.SaveChangesAsync();
			return serviceMap;
		}

		public async Task UpdateServiceAsync(ServiceDto serviceDto)
		{
			var clinic = await _context.Clinics!.FirstOrDefaultAsync(c => c.Id == serviceDto.ClinicId);
			var serviceMap = _mapper.Map<Service>(serviceDto);
			serviceMap.Clinic = clinic;

			_context.Services!.Update(serviceMap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteServiceAsync(Service service)
		{
			_context.Services!.Remove(service);
			await _context.SaveChangesAsync();
		}
	}
}
