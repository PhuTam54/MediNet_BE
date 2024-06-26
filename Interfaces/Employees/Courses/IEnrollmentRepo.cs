﻿using MediNet_BE.Dto.Employees.Courses;
using MediNet_BE.DtoCreate.Employees.Courses;
using MediNet_BE.Models.Employees.Courses;

namespace MediNet_BE.Interfaces.Employees.Courses
{
    public interface IEnrollmentRepo
    {
        public Task<List<EnrollmentDto>> GetAllEnrollmentAsync();
        public Task<EnrollmentDto> GetEnrollmentByIdAsync(int id);
		public Task<List<EnrollmentDto>> GetEnrollmentsByUserIdAsync(int employeeId);
		public Task<Enrollment> AddEnrollmentAsync(EnrollmentCreate enrollmentCreate);
        public Task UpdateEnrollmentAsync(EnrollmentCreate enrollmentCreate);
        public Task DeleteEnrollmentAsync(int id);
    }
}
