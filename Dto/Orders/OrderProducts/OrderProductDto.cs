﻿using MediNet_BE.Dto.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediNet_BE.Dto.Orders.OrderProducts
{
    public class OrderProductDto
    {
		public int Id { get; set; }
		public int Quantity { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal PriceSale { get; set; }
		public int ProductId { get; set; }
		public int OrderId { get; set; }
		public ProductDto Product { get; set; }
		public OrderDto Order { get; set; }
	}
}
