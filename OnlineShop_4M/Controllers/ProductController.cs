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
using OnlineShop_4M_DataAccess.Repository.IRepository;

namespace OnlineShop_4M.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class ProductController : Controller
    {
        private IProductRepository productRepository;
        private IWebHostEnvironment webHostEnvironment;

        public ProductController(IWebHostEnvironment webHostEnvironment,
            IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = productRepository.
                GetAll(includeProperties: "Category");

            //IEnumerable<Product> productList = productRepository.
            //    GetAll(includeProperties: "Category,Company");

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

                    productRepository.Add(productViewModel.Product);
                    //context.Product.Add(productViewModel.Product);
                }
                else
                {
                    // обновление изображения

                    //var product = context.Product.
                    //    AsNoTracking().                     // отключаем отслеживание за сущностью
                    //    FirstOrDefault(x => x.Id == productViewModel.Product.Id);

                    var product = productRepository.FirstOrDefault(
                        x => x.Id == productViewModel.Product.Id, isTracking: false);

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

                    productRepository.Update(productViewModel.Product);
                }

                // сохранить в бд
                productRepository.Save();

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult UpdateCreate(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoryDropDown = productRepository.GetAllDropDownList(PathManager.CategoryName)
            };

            if (id == null)
            {
                // create

                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = productRepository.Find(id.GetValueOrDefault());
                //productViewModel.Product = context.Product.Find(id);   // ef поиск по id

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
            Product product = productRepository.FirstOrDefault(
                x => x.Id == id, includeProperties: "Category");

            //Product product = productRepository.FirstOrDefault(
            //    x => x.Id == id, includeProperties: "Category,Company");

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var product = productRepository.Find(id.GetValueOrDefault());

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

            productRepository.Remove(product);
            productRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

