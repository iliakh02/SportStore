using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using SportStore.WebUI.Interfaces;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SportStore.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUrlService _urlService;

        public int PageSize { get; } = 4;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment, IUrlService urlService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _urlService = urlService;
        }

        public IActionResult Index(string searchString, int page = 1, ProductsSortState sortOrder = ProductsSortState.IdAsc)
        {
            ViewData["IdSort"] = sortOrder == ProductsSortState.IdAsc ? ProductsSortState.IdDesc : ProductsSortState.IdAsc;
            ViewData["NameSort"] = sortOrder == ProductsSortState.NameAsc ? ProductsSortState.NameDesc : ProductsSortState.NameAsc;
            ViewData["ProducerSort"] = sortOrder == ProductsSortState.ProducerAsc ? ProductsSortState.ProducerDesc : ProductsSortState.ProducerAsc;
            ViewData["PriceSort"] = sortOrder == ProductsSortState.PriceAsc ? ProductsSortState.PriceDesc : ProductsSortState.PriceAsc;

            var products = _productRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(n => n.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            List<Product> sortProducts = (sortOrder switch
            {
                ProductsSortState.IdDesc => products.OrderByDescending(n => n.Id),
                ProductsSortState.NameAsc => products.OrderBy(n => n.Name),
                ProductsSortState.NameDesc => products.OrderByDescending(n => n.Name),
                ProductsSortState.ProducerAsc => products.OrderBy(n => n.Producer),
                ProductsSortState.ProducerDesc => products.OrderByDescending(n => n.Producer),
                ProductsSortState.PriceAsc => products.OrderBy(n => n.Price),
                ProductsSortState.PriceDesc => products.OrderByDescending(n => n.Price),
                _ => products.OrderBy(n => n.Id),
            }).ToList();

            List<Product> productsPerPage = sortProducts.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var productsViewModel = new ProductsViewModel
            {
                Products = productsPerPage,
                SortViewModel = new ProductsSortViewModel(sortOrder),
                PageModel = new PageViewModel(products.Count(), page, PageSize),
                SearchString = searchString
            };
            return View("AdminIndex", productsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            var categories = _categoryRepository.GetAll();
            var productCreateViewModel = new ProductCreateViewModel
            {
                Categories = new List<SelectListItem>(
                    categories.Select(n => new SelectListItem {
                        Value = n.Id.ToString(), Text = n.Name 
                    }))
            };
            return View(productCreateViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(ProductCreateViewModel productCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                string path = $"/images/{productCreateViewModel.Image.FileName}";

                using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
                {
                    productCreateViewModel.Image.CopyTo(fileStream);
                }

                var product = new Product
                {
                    Name = productCreateViewModel.Name,
                    Producer = productCreateViewModel.Producer,
                    Image = path,
                    Amount = productCreateViewModel.Amount,
                    CategoryId = productCreateViewModel.CategoryId,
                    Description = productCreateViewModel.Description,
                    Discount = productCreateViewModel.Discount,
                    Price = productCreateViewModel.Price
                };

                _productRepository.Add(product);
                _productRepository.Commit();

                return RedirectToAction("Index");
            }

            productCreateViewModel.Categories = new List<SelectListItem>(
                _categoryRepository.GetAll().Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }
            ));

            return View(productCreateViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
                return NotFound();

            var categories = _categoryRepository.GetAll();
            var productEditViewModel = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Amount = product.Amount,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Discount = product.Discount,
                Price = product.Price,
                Producer = product.Producer,
                ImagePath = product.Image,
                Categories = new List<SelectListItem>(
                    categories.Select(n => new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }))
            };
            return View(productEditViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(ProductEditViewModel productEditViewModel)
        {
            if (ModelState.IsValid)
            {
                string path = productEditViewModel.ImagePath;
                if (productEditViewModel.Image != null)
                {
                    path = $"/images/{productEditViewModel.Image.FileName}";

                    using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        productEditViewModel.Image.CopyTo(fileStream);
                    }
                }
                var product = new Product
                {
                    Id = productEditViewModel.Id,
                    Name = productEditViewModel.Name,
                    Producer = productEditViewModel.Producer,
                    Image = path,
                    Amount = productEditViewModel.Amount,
                    CategoryId = productEditViewModel.CategoryId,
                    Description = productEditViewModel.Description,
                    Discount = productEditViewModel.Discount,
                    Price = productEditViewModel.Price
                };

                _productRepository.Update(product);
                _productRepository.Commit();

                return RedirectToAction("Index");
            }

            productEditViewModel.Categories = new List<SelectListItem>(
                _categoryRepository.GetAll().Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }
            ));
            productEditViewModel.ImagePath = productEditViewModel.ImagePath;

            return View(productEditViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id, int pageSize, string queries)
        {
            string redirectUrl = _urlService.ReditectUrlForDelete(pageSize, queries, "Categories");

            var product = _productRepository.GetById(id);

            if (product == null)
                return NotFound();

            if(System.IO.File.Exists(_webHostEnvironment.WebRootPath + product.Image))
            {
                System.IO.File.Delete(_webHostEnvironment.WebRootPath + product.Image);
            }

            _productRepository.Delete(product);
            _productRepository.Commit();

            return Redirect(redirectUrl);
        }
    }
}
