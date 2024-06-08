using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_DataAccess.Repository.IRepository;
using OnlineShop_4M_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop_4M_DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            context.OrderHeader.Update(orderHeader);
        }
    }
}
