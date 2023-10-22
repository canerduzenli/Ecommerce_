using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductBLL _productBLL;

        public ProductController(ProductBLL productBLL)
        {
            _productBLL = productBLL;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchTerm)
        {
            if (searchTerm == null)
            {
                var products = _productBLL.GetAllProducts();
                return View(products);
            }
            else
            {
                var searchResults = _productBLL.Search(searchTerm);
                return View(searchResults);
            }
        }

        [HttpGet]
        public IActionResult AddToCart(Guid productId)
        {
            _productBLL.AddToCart(productId);
            return RedirectToAction("Index");
        }

        public IActionResult Search()
        {
            string searchTerm = Request.Query["searchTerm"];

            return RedirectToAction("Index", new { searchTerm = searchTerm });
        }
    }
}