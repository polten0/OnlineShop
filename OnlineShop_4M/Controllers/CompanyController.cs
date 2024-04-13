using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_Models;
using OnlineShop_4M_Utility;

namespace OnlineShop_4M.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class CompanyController : Controller
    {
        private ApplicationDbContext context;

        public CompanyController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            List<Company> companyList = context.Company.ToList();

            return View(companyList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                context.Company.Add(company);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(company);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var company = context.Company.Find(id);

            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                context.Company.Update(company);   // !!! EF UPDATE
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(company);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var company = context.Company.Find(id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost]
        public IActionResult Delete(Company company)
        {
            int id = company.Id;

            var companyFromBase = context.Company.Find(id);

            if (companyFromBase == null)
            {
                return NotFound();
            }

            context.Company.Remove(companyFromBase);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

