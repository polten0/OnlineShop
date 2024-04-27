using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_DataAccess.Repository.IRepository;
using OnlineShop_4M_Models;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M_DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == PathManager.CategoryName)
            {
                return context.Category.Select(x =>
                    new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });
            }

            if (obj == PathManager.CompanyName)
            {
                return context.Company.Select(x =>
                    new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });
            }

            return null;
        }

        public void Update(Product product)
        {
            context.Update(product);
        }
    }
}

