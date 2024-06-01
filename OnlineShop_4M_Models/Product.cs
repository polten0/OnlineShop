using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop_4M_Models
{
	public class Product
	{
		public Product()
		{
			TempCount = 1;
        }

		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

        public string ShortDescription { get; set; }

        [Required]
		[Range(0, int.MaxValue)]
		public double Price { get; set; }

		public string Image { get; set; }

		// внешний ключ
		public int CategoryId { get; set; }

		// навигационное свойства
		[ForeignKey("CategoryId")]
		public Category Category { get; set; }

		[NotMapped]
		[Range(1,10000)]
		public int TempCount { get; set; }
	}
}

