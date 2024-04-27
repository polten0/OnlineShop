using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_Models;
using OnlineShop_4M_Utility;
using OnlineShop_4M_DataAccess.Repository.IRepository;

namespace OnlineShop_4M.Controllers
{
	[Authorize(Roles = PathManager.AdminRole)]
	public class CategoryController : Controller
	{
		private ICategoryRepository categoryRepository;

		public CategoryController(ICategoryRepository categoryRepository)
		{
			this.categoryRepository = categoryRepository;
		}

        public IActionResult Index()
		{
            List<Category> categoryList = categoryRepository.GetAll().ToList();

			return View(categoryList);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category category)
		{
			if (ModelState.IsValid)
			{
				categoryRepository.Add(category);
				categoryRepository.Save();

                return RedirectToAction("Index");
            }

			return View(category);
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var category = categoryRepository.Find(id.GetValueOrDefault());

            return View(category);
		}

		[HttpPost]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid)
			{
				categoryRepository.Update(category);
				categoryRepository.Save();

				return RedirectToAction("Index");
			}

			return View(category);
		}

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

			var category = categoryRepository.Find(id.GetValueOrDefault());

            if (category == null)
			{
                return NotFound();
            }

            return View(category);
        }

		[HttpPost]
		public IActionResult Delete(Category category)
		{
			int id = category.Id;

			var categoryFromBase = categoryRepository.Find(id);

            if (categoryFromBase == null)
			{
				return NotFound();
			}

			categoryRepository.Remove(categoryFromBase);
			categoryRepository.Save();

			return RedirectToAction("Index");
		}
    }
}

