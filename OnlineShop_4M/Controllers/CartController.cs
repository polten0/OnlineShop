using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_DataAccess.Repository.IRepository;
using OnlineShop_4M_Models;
using OnlineShop_4M_Models.ViewModels;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M.Controllers
{
    [Authorize]   // только для авторизованных = кто ввел логин и пароль
	public class CartController : Controller
	{
        private IWebHostEnvironment hostEnvironment;
        private IEmailSender emailSender;

        private IProductRepository productRepository;
        private IApplicationUserRepository userRepository;
        private IInquiryHeaderRepository inquiryHeaderRepository;
        private IInquiryDetailRepository inquiryDetailRepository;

        public CartController(IProductRepository productRepository, IApplicationUserRepository userRepository,
            IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailRepository inquiryDetailRepository,
            IWebHostEnvironment hostEnvironment, IEmailSender emailSender)
        {
            this.inquiryDetailRepository = inquiryDetailRepository;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
            this.inquiryHeaderRepository = inquiryHeaderRepository;

            this.hostEnvironment = hostEnvironment;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            // создаем список для корзины покупок
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            // если сессия есть
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(PathManager.SessionCart).Count() > 0)
            {
                // сессия существует!
                // берем лист из сессии и добавляем в лист элемент
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(PathManager.SessionCart);
            }

            // получаем список идентификаторов
            List<int> productIdCart = shoppingCartList.Select(x => x.ProductId).ToList();

            // извлекаем продукты по id
            // IEnumerable<Product> productList =
            //     context.Product.Where(x => productIdCart.Contains(x.Id));

            IEnumerable<Product> productListTemp = productRepository.GetAll(x => productIdCart.Contains(x.Id));


            // нужно, так как TempCount передается по умолчанию как 1, а значение
            // хранится в shoppingCartList
            List<Product> productList = new List<Product>();

            foreach (var item in shoppingCartList)
            {
                Product productTemp = productListTemp.FirstOrDefault(x => x.Id == item.ProductId);
                productTemp.TempCount = item.Count;

                productList.Add(productTemp);
            }
           

            return View(productListTemp);
        }

        public IActionResult Remove(int id)
        {
            // создаем списка для его заполнения из сессии
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            // если сессия есть
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(PathManager.SessionCart).Count() > 0)
            {
                // сессия существует!
                // заполнение листа из сессии
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(PathManager.SessionCart);
            }

            ShoppingCart item = shoppingCartList.FirstOrDefault(x => x.ProductId == id);

            shoppingCartList.Remove(item);

            // установить обновленный список для сессии
            HttpContext.Session.Set(PathManager.SessionCart, shoppingCartList);

            return RedirectToAction("Index");
        }

        public IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;

            // если пользователь вошел в систему, то определяе объект
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // создаем список для корзины покупок
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            // если сессия есть
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(PathManager.SessionCart).Count() > 0)
            {
                // сессия существует!
                // берем лист из сессии и добавляем в лист элемент
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(PathManager.SessionCart);
            }

            // получаем список идентификаторов всех продуктов
            List<int> productIdCart = shoppingCartList.Select(x => x.ProductId).ToList();

            // извлекаем продукты по id
            // IEnumerable<Product> productList = 
            //     context.Product.Where(x => productIdCart.Contains(x.Id));
            IEnumerable<Product> productListTemp = productRepository.GetAll(x => productIdCart.Contains(x.Id));

            List<Product> productList = new List<Product>();

            foreach (var item in shoppingCartList)
            {
                Product productTemp = productListTemp.FirstOrDefault(x => x.Id == item.ProductId);
                productTemp.TempCount = item.Count;

                productList.Add(productTemp);
            }


            ProductUserViewModel viewModel = new ProductUserViewModel()
            {
                ApplicationUser = userRepository.FirstOrDefault(x => x.Id == claim.Value),
                ProductList = productList.ToList()
            };

            return View(viewModel);
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SummaryPost(
            ProductUserViewModel productUserViewModel
        )
        {
            // Work with user
            var indetyClaims = (ClaimsIdentity)User.Identity;
            var claim = indetyClaims?.FindFirst(ClaimTypes.NameIdentifier);

            var pathTemplate = hostEnvironment.WebRootPath +
                PathManager.TemplatesPath + "inquiry.html";

            var bodyHtml = await System.IO.File.ReadAllTextAsync(pathTemplate);

            var textProducts = "";

            foreach (var product in productUserViewModel.ProductList)
            {
                textProducts += $"- Name: {product.Name} " +
                    $"<span style='font-size: 14px; color: green'>(ID: {product.Id})</span>" +
                    "<br>";
            }

            var body = string.Format(
                bodyHtml,
                productUserViewModel.ApplicationUser.FullName,
                productUserViewModel.ApplicationUser.PhoneNumber,
                textProducts
            );

            await emailSender.SendEmailAsync(
                productUserViewModel.ApplicationUser.Email,
                "Новый заказ",
                body
            );

            var inquiryHeader = new InquiryHeader()
            {
                ApplicationUserId = claim.Value,
                ApplicationUser = userRepository.FirstOrDefault(x => x.Id == claim.Value),
                FullName = productUserViewModel.ApplicationUser.FullName,
                Email = productUserViewModel.ApplicationUser.Email,
                PhoneNumber = productUserViewModel.ApplicationUser.PhoneNumber ?? "+70000000000",
                InquiryDate = DateTime.Now
            };

            inquiryHeaderRepository.Add(inquiryHeader);
            inquiryHeaderRepository.Save();

            foreach (var product in productUserViewModel.ProductList)
            {
                var inquiryDetail = new InquiryDetail()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = product.Id
                };

                inquiryDetailRepository.Add(inquiryDetail);
            }

            inquiryDetailRepository.Save();

            return RedirectToAction(nameof(InquiryConfirmation));
        }
    }
}

