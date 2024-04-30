using System.ComponentModel.DataAnnotations;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Models.Categories
{
    public class CategoryChild
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
