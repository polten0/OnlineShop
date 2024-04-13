using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_Models;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M.Controllers
{
	[Authorize(Roles = PathManager.AdminRole)]
	public class CategoryController : Controller
	{
		private ApplicationDbContext context;

		public CategoryController(ApplicationDbContext context)
		{
			this.context = context;
		}

        public IActionResult Index()
		{
			List<Category> categoryList = context.Category.ToList();

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
                context.Category.Add(category);
                context.SaveChanges();

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

			var category = context.Category.Find(id);

			return View(category);
		}

		[HttpPost]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid)
			{
				context.Category.Update(category);   // !!! EF UPDATE
				context.SaveChanges();

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

            var category = context.Category.Find(id);

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

			var categoryFromBase = context.Category.Find(id);

			if (categoryFromBase == null)
			{
				return NotFound();
			}

			context.Category.Remove(categoryFromBase);
			context.SaveChanges();

			return RedirectToAction("Index");
		}
    }
}

