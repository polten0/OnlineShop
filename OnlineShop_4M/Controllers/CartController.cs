using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_Models;
using OnlineShop_4M_Models.ViewModels;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M.Controllers
{
    [Authorize]   // только для авторизованных = кто ввел логин и пароль
	public class CartController : Controller
	{
        private ApplicationDbContext context;

        public CartController(ApplicationDbContext context)
        {
            this.context = context;
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
            IEnumerable<Product> productList =
                context.Product.Where(x => productIdCart.Contains(x.Id));

            return View(productList);
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
            IEnumerable<Product> productList =
                context.Product.Where(x => productIdCart.Contains(x.Id));

            ProductUserViewModel viewModel = new ProductUserViewModel()
            {
                ApplicationUser = context.ApplicationUser.FirstOrDefault(x => x.Id == claim.Value),
                ProductList = productList.ToList()
            };

            return View(viewModel);
        }
    }
}

