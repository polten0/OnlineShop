using Microsoft.AspNetCore.Mvc;
using OnlineShop_4M_DataAccess.Repository;
using OnlineShop_4M_DataAccess.Repository.IRepository;
using OnlineShop_4M_Models;
using OnlineShop_4M_Models.ViewModels;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M.Controllers
{
    public class OrderController : Controller
    {
        private IOrderDetailRepository orderDetailRepository;
        private IOrderHeaderRepository orderHeaderRepository;

        public OrderController(IOrderHeaderRepository orderHeaderRepository, IOrderDetailRepository orderDetailRepository)
        {
            this.orderHeaderRepository = orderHeaderRepository;
            this.orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            OrderViewModel orderViewModel = new OrderViewModel()
            {
                OrderHeader = orderHeaderRepository.FirstOrDefault(x => x.Id == id),
                OrderDetails = orderDetailRepository.GetAll(x => x.OrderHeaderId == id,
                                 includeProperties: "Product")
            };

            return View(orderViewModel);
        }

        [HttpPost]
        public IActionResult ApproveOrder(int id)
        {
            OrderHeader orderHeader = orderHeaderRepository.FirstOrDefault(x => x.Id == id);
            orderHeader.OrderStatus = PathManager.StatusApproved;
            orderHeaderRepository.Update(orderHeader);
            orderHeaderRepository.Save();

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult StartWorkAtOrder(int id)
        {
            OrderHeader orderHeader = orderHeaderRepository.FirstOrDefault(x => x.Id == id);
            orderHeader.OrderStatus = PathManager.StatusAtWork;
            orderHeaderRepository.Update(orderHeader);
            orderHeaderRepository.Save();

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult ShipOrder(int id)
        {
            OrderHeader orderHeader = orderHeaderRepository.FirstOrDefault(x => x.Id == id);
            orderHeader.OrderStatus = PathManager.StatusShipped;
            orderHeaderRepository.Update(orderHeader);
            orderHeaderRepository.Save();

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult CancelOrder(int id)
        {
            OrderHeader orderHeader = orderHeaderRepository.FirstOrDefault(x => x.Id == id);
            orderHeader.OrderStatus = PathManager.StatusCancelled;
            orderHeaderRepository.Update(orderHeader);
            orderHeaderRepository.Save();

            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public IActionResult GetOrderList()
        {
            var result = Json(new { data = orderHeaderRepository.GetAll() });

            return result;
        }
    }
}
