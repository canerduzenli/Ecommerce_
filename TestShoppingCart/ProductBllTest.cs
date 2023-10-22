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
    public class ProductBllTest
    {
        private Mock<IRepository<Products, Guid>> _mockProductRepo;
        private Mock<IRepository<Cart, int>> _mockCartRepo;
        private Mock<IRepository<Order, int>> _mockOrderRepo;
        private ProductBLL _productBll;

        [TestInitialize]
        public void Setup()
        {
            _mockProductRepo = new Mock<IRepository<Products, Guid>>();
            _mockCartRepo = new Mock<IRepository<Cart, int>>();
            _mockOrderRepo = new Mock<IRepository<Order, int>>();

            _productBll = new ProductBLL(_mockProductRepo.Object, _mockCartRepo.Object, _mockOrderRepo.Object);
        }

        [TestMethod]
        public void GetAllProducts_ReturnsProductsOrderedByName()
        {
            // Arrange
            var products = new List<Products>
            {
                new Products { Name = "Z-Product" },
                new Products { Name = "A-Product" },
                new Products { Name = "M-Product" }
            };
            _mockProductRepo.Setup(repo => repo.GetAll()).Returns(products);

            // Act
            var result = _productBll.GetAllProducts();

            // Assert
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("A-Product", result.ElementAt(0).Name);
            Assert.AreEqual("M-Product", result.ElementAt(1).Name);
            Assert.AreEqual("Z-Product", result.ElementAt(2).Name);
        }

        [TestMethod]
        public void AddToCart_ProductExistsInCart_UpdatesCartAndDecreasesProductQuantity()
        {
            // Arrange
            Guid testProductId = Guid.NewGuid();
            var product = new Products { ProductId = testProductId, Name = "Test-Product", AvailableQuantity = 5 };
            var cartItems = new List<Cart> { new Cart { ProductName = "Test-Product", OrderID = 1, ItemsInCart = 1 } };
            _mockProductRepo.Setup(repo => repo.Get(testProductId)).Returns(product);
            _mockOrderRepo.Setup(repo => repo.GetAll()).Returns(new List<Order>());
            _mockCartRepo.Setup(repo => repo.GetAll()).Returns(cartItems);

            // Act
            _productBll.AddToCart(testProductId);

            // Assert
            _mockProductRepo.Verify(repo => repo.Update(It.Is<Products>(p => p.AvailableQuantity == 4)));
            _mockCartRepo.Verify(repo => repo.Update(It.Is<Cart>(c => c.ItemsInCart == 2 && c.ProductName == "Test-Product")));
        }

        [TestMethod]
        public void Search_ValidSearchTerm_ReturnsFilteredProducts()
        {
            // Arrange
            var products = new List<Products>
            {
                new Products { Name = "Alpha", Description = "Product A" },
                new Products { Name = "Beta", Description = "Product B" },
                new Products { Name = "Gamma", Description = "Product A again" }
            };
            _mockProductRepo.Setup(repo => repo.GetAll()).Returns(products);

            // Act
            var result = _productBll.Search("Alpha");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Alpha", result.First().Name);
        }
    }
}
