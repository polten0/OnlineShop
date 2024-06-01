using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_Models;
using OnlineShop_4M_Models.ViewModels;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M.Controllers;

public class HomeController : Controller
{
    private ApplicationDbContext context;

    public HomeController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        HomeViewModel viewModel = new HomeViewModel()
        {
            Products = context.Product.Include(x => x.Category),
            Categories = context.Category
        };

        return View(viewModel);
    }

    public IActionResult RemoveFromCart(int id)
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

        // получаем элемент из корзины
        var element = shoppingCartList.SingleOrDefault(x => x.ProductId == id);

        // убираем этот элемент из корзины
        if (element != null)
        {
            shoppingCartList.Remove(element);
        }

        // обновление сессии
        HttpContext.Session.Set(PathManager.SessionCart, shoppingCartList);


        return RedirectToAction("Index");
    }

    public IActionResult Details(int id)
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

        DetailsViewModel viewModel = new DetailsViewModel()
        {
            Product = context.Product.Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id),
            ExistsInCart = false
        };

        // получить все элементы из корзины
        foreach (var item in shoppingCartList)
        {
            if (item.ProductId == id)  // товар есть в корзине
            {
                viewModel.ExistsInCart = true;
            }
        }

        return View(viewModel);
    }

    [HttpPost, ActionName("Details")]
    public IActionResult DetailsPost(int id, DetailsViewModel detailsViewModel)
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

        // добавить id продукта в корзину
        shoppingCartList.Add(new ShoppingCart() { ProductId = id, Count = detailsViewModel.Product.TempCount });

        // установить сессиию - обновить
        HttpContext.Session.Set(PathManager.SessionCart, shoppingCartList);

        return RedirectToAction("Index");
    }
}