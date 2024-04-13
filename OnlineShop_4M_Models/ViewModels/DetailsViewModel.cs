using System;

namespace OnlineShop_4M_Models.ViewModels
{
	public class DetailsViewModel
	{
		public Product Product { get; set; }
		public bool ExistsInCart { get; set; }

		public DetailsViewModel()
		{
			Product = new Product();
		}
	}
}

