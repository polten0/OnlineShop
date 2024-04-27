using System;
using OnlineShop_4M_Models;

namespace OnlineShop_4M_DataAccess.Repository.IRepository
{
	public interface IInquiryHeaderRepository : IRepository<InquiryHeader>
	{
		void Update(InquiryHeader obj);
	}
}

