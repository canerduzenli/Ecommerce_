using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.BLL;
using System.Collections.Generic;
using System.Linq;

namespace TestShoppingCart
{
    [TestClass]
    public class OrderBllTest
    {
        private Mock<IRepository<Products, Guid>> _mockProductRepo;
        private Mock<IRepository<Cart, int>> _mockCartRepo;
        private Mock<IRepository<Country, int>> _mockCountryRepo;
        private Mock<IRepository<Order, int>> _mockOrderRepo;
        private OrderBLL _orderBll;

        [TestInitialize]
        public void Setup()
        {
            _mockProductRepo = new Mock<IRepository<Products, Guid>>();
            _mockCartRepo = new Mock<IRepository<Cart, int>>();
            _mockCountryRepo = new Mock<IRepository<Country, int>>();
            _mockOrderRepo = new Mock<IRepository<Order, int>>();

            _orderBll = new OrderBLL(_mockProductRepo.Object, _mockCartRepo.Object, _mockCountryRepo.Object, _mockOrderRepo.Object);
        }

        [TestMethod]
        public void ConvertedPrice_GivenValidCountry_ReturnsCorrectPriceCalculationResult()
        {
            // Arrange
            decimal price = 100;
            string deliveryCountry = "USA";
            var countryData = new Country { CountryName = "USA", ConversionRate = 1.2m, TaxRate = 0.05 };

            _mockCountryRepo.Setup(repo => repo.GetAll()).Returns(new List<Country> { countryData });

            // Act
            var result = _orderBll.convertedPrice(price, deliveryCountry);

            // Assert
            Assert.AreEqual(1.2m, result.ConversionRate);
            Assert.AreEqual(120m, result.ConvertedPrice);
            Assert.AreEqual(0.05m, result.TaxRate);
            Assert.AreEqual(126m, result.TotalPriceWithTaxes);
        }

        [TestMethod]
        public void CreateOrder_GivenValidParameters_CreatesOrderCorrectly()
        {
            // Arrange
            var cartItems = new List<Cart>
            {
                new Cart { OrderID = 1, ItemsInCart = 2 },
                new Cart { OrderID = 1, ItemsInCart = 3 },
                new Cart { OrderID = 2, ItemsInCart = 5 }
            };
            _mockCartRepo.Setup(repo => repo.GetAll()).Returns(cartItems);

            // Act
            _orderBll.CreateOrder("123 Street", "12345", "USA", 150m);

            // Assert
            _mockOrderRepo.Verify(repo => repo.Create(It.IsAny<Order>()), Times.Once);
        }

        [TestMethod]
        public void GetAll_GivenOrdersExist_ReturnsAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order(),
                new Order(),
                new Order()
            };
            _mockOrderRepo.Setup(repo => repo.GetAll()).Returns(orders);

            // Act
            var result = _orderBll.GetAll();

            // Assert
            Assert.AreEqual(3, result.Count);
        }
    }
}
