﻿using MediNet_BE.DtoCreate.Users;

namespace MediNet_BE.DtoCreate.Doctors
{
	public class DoctorCreate : UserCreate
	{
		public string Full_Name { get; set; }
		public string Address { get; set; }
		public DateTime Date_Of_Birth { get; set; }
		public int Gender { get; set; }
		public string PhoneNumber { get; set; }
		public int SpecialistId { get; set; }
		public int ClinicId { get; set; }
	}
}
