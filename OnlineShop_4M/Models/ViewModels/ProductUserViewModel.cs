using System;

namespace OnlineShop_4M.Models.ViewModels
{
	public class ProductUserViewModel
	{
		public ApplicationUser ApplicationUser { get; set; }
		public List<Product> ProductList { get; set; }
	}
}

