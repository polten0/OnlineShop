using System;
using OnlineShop_4M_Models;

namespace OnlineShop_4M_DataAccess.Repository.IRepository
{
	public interface IInquiryDetailRepository : IRepository<InquiryDetail>
    {
        void Update(InquiryDetail obj);
    }
}

