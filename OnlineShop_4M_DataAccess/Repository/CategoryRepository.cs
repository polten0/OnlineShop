using System;
using OnlineShop_4M_Models;
using OnlineShop_4M_DataAccess.Repository.IRepository;
using OnlineShop_4M_DataAccess.Data;

namespace OnlineShop_4M_DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(Category category)
        {
            var objectCategory = context.Category.
                FirstOrDefault(x => x.Id == category.Id);

            if (objectCategory != null)
            {
                objectCategory.Name = category.Name;
                objectCategory.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}

