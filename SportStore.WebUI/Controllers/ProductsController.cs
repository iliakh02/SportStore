using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(Product product)
        {
            // TODO: Add Code to create new product.
            throw new Exception();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            // TODO: Add code to input new information about selected product.
            throw new Exception();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(Product product)
        {
            // TODO: Add code to save new information about selected product.
            throw new Exception();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            // TODO: Add code to delete selected product.
            throw new Exception();
        }
    }
}
