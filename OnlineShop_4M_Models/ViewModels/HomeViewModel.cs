using System;
using OnlineShop_4M.Models;

namespace OnlineShop_4M_Models.ViewModels
{
	public class HomeViewModel
	{
		public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}

