using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop_4M_DataAccess.Repository.IRepository;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class InquiryController : Controller
    {
        private IInquiryHeaderRepository inquiryHeaderRepository;
        private IInquiryDetailRepository inquiryDetailRepository;

        public InquiryController(
            IInquiryHeaderRepository inquiryHeaderRepository,
            IInquiryDetailRepository inquiryDetailRepository
        )
        {
            this.inquiryHeaderRepository = inquiryHeaderRepository;
            this.inquiryDetailRepository = inquiryDetailRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetInquiryList()
        {
            var result = Json(new { data = inquiryHeaderRepository.GetAll() });

            return result;
        }
    }
}
