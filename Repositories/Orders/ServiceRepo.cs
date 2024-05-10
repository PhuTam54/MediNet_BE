﻿using AutoMapper;
using MediNet_BE.Data;
using MediNet_BE.Dto.Clinics;
using MediNet_BE.Dto.Orders.OrderServices;
using MediNet_BE.DtoCreate.Orders.OrderServices;
using MediNet_BE.Interfaces.Clinics;
using MediNet_BE.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace MediNet_BE.Repositories.Orders
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

        public async Task<List<ServiceDto>> GetAllServiceAsync()
        {
            var services = await _context.Services!
                .Include(d => d.Doctor)
                .ToListAsync();

			var servicesMap = _mapper.Map<List<ServiceDto>>(services);

			return servicesMap;
        }

        public async Task<ServiceDto> GetServiceByIdAsync(int id)
        {
            var service = await _context.Services!
				.Include(d => d.Doctor)
				.Include(os => os.OrderServices)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

			var serviceMap = _mapper.Map<ServiceDto>(service);

			return serviceMap;
        }

        public async Task<Service> AddServiceAsync(ServiceCreate serviceCreate)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(c => c.Id == serviceCreate.DoctorId);
            var serviceMap = _mapper.Map<Service>(serviceCreate);
            serviceMap.Doctor = doctor;

            _context.Services!.Add(serviceMap);
            await _context.SaveChangesAsync();
            return serviceMap;
        }

        public async Task UpdateServiceAsync(ServiceCreate serviceCreate)
        {
			var doctor = await _context.Doctors.FirstOrDefaultAsync(c => c.Id == serviceCreate.DoctorId);
			var serviceMap = _mapper.Map<Service>(serviceCreate);
			serviceMap.Doctor = doctor;

			_context.Services!.Update(serviceMap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await _context.Services!.FirstOrDefaultAsync(s => s.Id == id);
            _context.Services!.Remove(service);
            await _context.SaveChangesAsync();
        }
    }
}
