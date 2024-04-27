using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_Models;
using OnlineShop_4M_Utility;

using OnlineShop_4M_DataAccess.Repository;
using OnlineShop_4M_DataAccess.Repository.IRepository;

namespace OnlineShop_4M.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class CompanyController : Controller
    {
        private ApplicationDbContext context;

        private ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public IActionResult Index()
        {
            List<Company> companyList = companyRepository.GetAll().ToList();

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
                companyRepository.Add(company);
                companyRepository.Save();

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

            var company = companyRepository.Find(id.GetValueOrDefault());

            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                companyRepository.Update(company);
                companyRepository.Save();

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

            var company = companyRepository.Find(id.GetValueOrDefault());

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

            var companyFromBase = companyRepository.Find(id);

            if (companyFromBase == null)
            {
                return NotFound();
            }

            companyRepository.Remove(companyFromBase);
            companyRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

