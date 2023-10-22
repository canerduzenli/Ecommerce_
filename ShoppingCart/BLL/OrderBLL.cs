using ShoppingCart.Data;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.BLL
{
    public class OrderBLL
    {
        private readonly IRepository<Products, Guid> _productRepo;
        private readonly IRepository<Cart, int> _cartRepo;
        private readonly IRepository<Country, int> _countryRepo;
        private readonly IRepository<Order, int> _orderRepo;

        public OrderBLL(IRepository<Products, Guid> productRepo, IRepository<Cart, int> cartRepo, IRepository<Country, int> countryRepo, IRepository<Order, int> orderRepo)
        {
            _productRepo = productRepo;
            _cartRepo = cartRepo;
            _countryRepo = countryRepo;
            _orderRepo = orderRepo;
        }


        public class PriceCalculationResult
        {
            public decimal ConversionRate { get; set; }
            public decimal ConvertedPrice { get; set; }
            public decimal TaxRate { get; set; }
            public decimal TotalPriceWithTaxes { get; set; }
        }
        public PriceCalculationResult convertedPrice(decimal price, string deliveryCountry)
        {
            var country = _countryRepo.GetAll().FirstOrDefault(c => c.CountryName == deliveryCountry);
            if (country == null)
            {
                throw new InvalidOperationException("Country not found.");
            }
            decimal convertedPrice = country.ConversionRate * price;
            decimal totalPriceWithTaxes = (decimal)country.TaxRate * convertedPrice + convertedPrice;

            return new PriceCalculationResult
            {
                ConversionRate = country.ConversionRate,
                ConvertedPrice = convertedPrice,
                TaxRate = (decimal)country.TaxRate,
                TotalPriceWithTaxes = totalPriceWithTaxes
            };
        }

        public void CreateOrder(string address, string mailingCode, string deliveryCountry, decimal totalPriceWithTaxes)
        {
            var cartItems = _cartRepo.GetAll();
            int maxOrderID = cartItems.Max(cartItem => cartItem.OrderID);

            var cartItemsWithMaxOrderID = cartItems.Where(cartItem => cartItem.OrderID == maxOrderID).ToList();

            int totalItemsNumInCart = cartItemsWithMaxOrderID.Sum(cartItem => cartItem.ItemsInCart);

            Order newOrder = new Order
            {
                Address = address,
                DeliveryCountry = deliveryCountry,
                MailingCode = mailingCode,
                TotalPriceWithTaxes = totalPriceWithTaxes,
            };
            newOrder.AllItemsNum = totalItemsNumInCart;
            _orderRepo.Create(newOrder);
        }

        public List<Order> GetAll()
        {
            List<Order> allOrders = _orderRepo.GetAll().ToList();

            return allOrders;
        }
    }
}
