using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop_4M_Models;

namespace OnlineShop_4M_DataAccess.Repository.IRepository
{
	public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);

        IEnumerable<SelectListItem> GetAllDropDownList(string obj);
    }
}

