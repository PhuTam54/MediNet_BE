﻿namespace MediNet_BE.DtoCreate.Clinics
{
	public class StockOutCreate
	{
		public int Id { get; set; }
		public int ClinicId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public DateTime DateOut { get; set; }
		public string Reason { get; set; }
	}
}
