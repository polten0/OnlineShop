using System;

namespace OnlineShop_4M_Models.ViewModels
{
	public class ProductUserViewModel
	{
		public ApplicationUser ApplicationUser { get; set; }
		public List<Product> ProductList { get; set; }
	}
}

