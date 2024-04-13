using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_Models;
using OnlineShop_4M_Models.ViewModels;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class ProductController : Controller
    {
        private ApplicationDbContext context;

        private IWebHostEnvironment webHostEnvironment;

        public ProductController(ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;

            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = context.Product;

            return View(productList);
        }

        [HttpPost]
        public IActionResult UpdateCreate(ProductViewModel productViewModel)
        {
            // валидация на сервере
            //if (ModelState.IsValid)
            {
                // получаем картинку
                var file = HttpContext.Request.Form.Files;

                // получаем путь к папке root
                string webRootPath = webHostEnvironment.WebRootPath;

                // проверяем есть ли изображение
                if (productViewModel.Product.Id == 0)   // нет изображения
                {
                    // work with path
                    string uploadPath = webRootPath + PathManager.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file[0].FileName);

                    string path = Path.Combine(uploadPath, fileName) + extension;

                    // копируем файл на сервер
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = fileName + extension;

                    context.Product.Add(productViewModel.Product);
                }
                else
                {
                    // обновление изображения

                    var product = context.Product.
                        AsNoTracking().                     // отключаем отслеживание за сущностью
                        FirstOrDefault(x => x.Id == productViewModel.Product.Id);

                    if (file.Count > 0)   // если юзер загрул файл
                    {
                        // отправляем файл на сервер заменяя прошлый файл
                        // work with path
                        string uploadPath = webRootPath + PathManager.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(file[0].FileName);
                        string path = Path.Combine(uploadPath, fileName) + extension;

                        // получаем ссылку на старую фотку
                        var oldFile = uploadPath + product.Image;

                        // проверяем, что файл существует. Если существует - удаляем
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        // заливаем фото на сервер
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            file[0].CopyTo(fileStream);
                        }

                        productViewModel.Product.Image = fileName + extension;
                    }
                    else // не обновляет фото
                    {
                        productViewModel.Product.Image = product.Image;
                    }

                    context.Product.Update(productViewModel.Product);
                }

                // сохранить в бд
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }


        public IActionResult UpdateCreate(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoryDropDown = context.Category.
                    Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
            };

            if (id == null)
            {
                // create

                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = context.Product.Find(id);   // ef поиск по id

                if (productViewModel.Product == null)
                {
                    return NotFound();
                }

                return View(productViewModel);
            }
        }


        // GET - Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // удаление фотки с сервера
            // удаление товара из бд

            Product product = context.Product.Include(nav => nav.Category).
                FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var product = context.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            // удаление картинки
            string folderPath = webHostEnvironment.WebRootPath + PathManager.ImagePath;
            string filePath = folderPath + product.Image;

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            context.Product.Remove(product);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

