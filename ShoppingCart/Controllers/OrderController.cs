using Microsoft.AspNetCore.Mvc;
using ShoppingCart.BLL;
using System.Collections.Generic;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class OrderController : Controller
    {
        private readonly CartBLL _cartBLL;
        private readonly OrderBLL _orderBLL;

        public OrderController(CartBLL cartBLL, OrderBLL orderBLL)
        {
            _cartBLL = cartBLL;
            _orderBLL = orderBLL;
        }

        public IActionResult Index()
        {
            List<Order> orders = _orderBLL.GetAll();
            ViewBag.Orders = orders;
            return View();
        }

        public IActionResult Submit(string deliveryCountry)
        {
            ViewBag.DeliveryCountry = deliveryCountry;

            decimal totalPrice = _cartBLL.totalPrice();
            ViewData["TotalPrice"] = totalPrice;

            var convertedPriceInfo = _orderBLL.convertedPrice(totalPrice, deliveryCountry);

            ViewBag.ConversionRate = convertedPriceInfo.ConversionRate;
            ViewBag.ConvertedPrice = convertedPriceInfo.ConvertedPrice;
            ViewBag.TaxRate = convertedPriceInfo.TaxRate;
            ViewBag.TotalPriceWithTaxes = convertedPriceInfo.TotalPriceWithTaxes;

            return View();
        }

        [HttpPost]
        public IActionResult Create()
        {
            string address = Request.Form["Address"];
            string mailingCode = Request.Form["MailingCode"];
            string deliveryCountry = Request.Form["deliveryCountry"];
            decimal totalPriceWithTaxes = decimal.Parse(Request.Form["totalPriceWithTaxes"]);

            _orderBLL.CreateOrder(address, mailingCode, deliveryCountry, totalPriceWithTaxes);

            return RedirectToAction("Index");
        }
    }
}
