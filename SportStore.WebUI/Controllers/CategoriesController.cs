using Microsoft.AspNetCore.Mvc;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public int PageSize { get; } = 4;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, CategoriesSortState sortOrder = CategoriesSortState.IdAsc)
        {
            ViewData["IdSort"] = sortOrder == CategoriesSortState.IdAsc ? CategoriesSortState.IdDesc : CategoriesSortState.IdAsc;
            ViewData["NameSort"] = sortOrder == CategoriesSortState.NameAsc ? CategoriesSortState.NameDesc : CategoriesSortState.NameAsc;

            var categories = _categoryRepository.GetAll();

            List<Category> sortCategories = (sortOrder switch
            {
                CategoriesSortState.IdDesc => categories.OrderByDescending(n => n.Id),
                CategoriesSortState.NameAsc => categories.OrderBy(n => n.Name),
                CategoriesSortState.NameDesc => categories.OrderByDescending(n => n.Name),
                _ => categories.OrderBy(n => n.Id),
            }).ToList();

            List<Category> categoriesPerPage = sortCategories.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var categoriesViewModel = new CategoriesViewModel
            {
                Categories = categoriesPerPage,
                SortViewModel = new CategoriesSortViewModel(sortOrder),
                PageModel = new PageViewModel(categories.Count(), page, PageSize)
            };
            return View(categoriesViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            return RedirectToAction("Index");
        }
    }
}
