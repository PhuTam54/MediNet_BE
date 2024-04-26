using System.ComponentModel.DataAnnotations;

namespace MediNet_BE.Dto
{
	public class CategoryChildDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }

	}
}
