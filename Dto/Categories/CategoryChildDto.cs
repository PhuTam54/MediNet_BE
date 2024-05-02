using System.ComponentModel.DataAnnotations;
using MediNet_BE.Dto.Orders;

namespace MediNet_BE.Dto.Categories
{
    public class CategoryChildDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
}
