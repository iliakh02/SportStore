using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using SportStore.WebUI.Interfaces;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SportStore.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUrlService _urlService;
        public int PageSize { get; } = 4;

        public CategoriesController(ICategoryRepository categoryRepository, IUrlService urlService)
        {
            _categoryRepository = categoryRepository;
            _urlService = urlService;
        }

        [HttpGet]
        public IActionResult Index(string searchString, int page = 1, CategoriesSortState sortOrder = CategoriesSortState.IdAsc)
        {
            ViewData["IdSort"] = sortOrder == CategoriesSortState.IdAsc ? CategoriesSortState.IdDesc : CategoriesSortState.IdAsc;
            ViewData["NameSort"] = sortOrder == CategoriesSortState.NameAsc ? CategoriesSortState.NameDesc : CategoriesSortState.NameAsc;

            var categories = _categoryRepository.GetAll();

            if(!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(n => n.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

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
                PageModel = new PageViewModel(categories.Count(), page, PageSize),
                SearchString = searchString
            };
            return View(categoriesViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category == null)
                return NotFound();

            _categoryRepository.Add(category);
            _categoryRepository.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category == null)
                return NotFound();

            _categoryRepository.Update(category);
            _categoryRepository.Commit();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id, int pageSize, string queries)
        {
            string redirectUrl = _urlService.ReditectUrlForDelete(id, pageSize, queries);

            var category = _categoryRepository.GetById(id);
            if (category == null)
                return NotFound();

            _categoryRepository.Delete(category);
            _categoryRepository.Commit();

            return Redirect(redirectUrl);
        }
    }
}
