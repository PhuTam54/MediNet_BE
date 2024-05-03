﻿using MediNet_BE.Dto.Users;
using MediNet_BE.Models.Orders;
using MediNet_BE.Dto.Orders.OrderProducts;
using System.ComponentModel.DataAnnotations.Schema;
using MediNet_BE.Dto.Orders.OrderServices;

namespace MediNet_BE.Dto.Orders
{
    public class OrderReturnDto
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public string Shipping_method { get; set; }
        public string Payment_method { get; set; }
        public bool Is_paid { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public int CustomerId { get; set; }
        public CustomerReturnDto Customer { get; set; }
        public ICollection<OrderProductReturnDto>? OrderProducts { get; set; }
        public ICollection<OrderServiceReturnDto>? OrderServices { get; set; }
    }
}
