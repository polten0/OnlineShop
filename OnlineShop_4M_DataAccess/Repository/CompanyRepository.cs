using System;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_DataAccess.Repository.IRepository;
using OnlineShop_4M_Models;

namespace OnlineShop_4M_DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(Company company)
        {
            var objectCompany = base.FirstOrDefault(x => x.Id == company.Id);

            if (objectCompany != null)
            {
                objectCompany.Name = company.Name;
                objectCompany.Country = company.Country;
            }
        }
    }
}

