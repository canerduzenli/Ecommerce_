using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingCart.BLL;
using ShoppingCart.Data;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestShoppingCart
{
    [TestClass]
    public class UnitTestShoppingCart
    {
        private Mock<IRepository<Products, Guid>> _mockProductRepo = new Mock<IRepository<Products, Guid>>();
        private Mock<IRepository<Cart, int>> _mockCartRepo = new Mock<IRepository<Cart, int>>();
        private Mock<IRepository<Order, int>> _mockOrderRepo = new Mock<IRepository<Order, int>>();
        private Mock<IRepository<Country, int>> _mockCountryRepo = new Mock<IRepository<Country, int>>();

        [TestMethod]
        public void CartBLL_GetAllCarts_ReturnsCartsWithMaxOrderID()
        {
            // Arrange
            _mockOrderRepo.Setup(repo => repo.GetAll()).Returns(new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } });
            _mockCartRepo.Setup(repo => repo.GetAll()).Returns(new List<Cart> { new Cart { OrderID = 3 }, new Cart { OrderID = 2 } });

            var bll = new CartBLL(_mockProductRepo.Object, _mockCartRepo.Object, _mockCountryRepo.Object, _mockOrderRepo.Object);

            // Act
            var result = bll.GetAllCarts();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3, result.First().OrderID);
        }
        [TestMethod]
        public void CartBLL_RemoveFromCart_DecreasesItemInCart_UpdatesOrDeletesCart()
        {
            //Arrange 
            const int testCartItemId = 123;
            const string testProductName = "TestProduct";
            var testCart = new Cart { Id = testCartItemId, ItemsInCart = 2, ProductName = testProductName };
            var testProduct = new Products { Name = testProductName, ProductId = Guid.NewGuid(), AvailableQuantity = 9 };

            _mockCartRepo.Setup(repo => repo.Get(testCartItemId)).Returns(testCart);
            _mockProductRepo.Setup(repo => repo.GetAll()).Returns(new List<Products> { testProduct });
            _mockProductRepo.Setup(repo => repo.Get(testProduct.ProductId)).Returns(testProduct);

            var bll = new CartBLL(_mockProductRepo.Object, _mockCartRepo.Object, _mockCountryRepo.Object, _mockOrderRepo.Object);

            // Act
            bll.RemoveFromCart(testCartItemId);

            // Assert
            Assert.AreEqual(1, testCart.ItemsInCart);
            Assert.AreEqual(10, testProduct.AvailableQuantity);

            _mockCartRepo.Verify(repo => repo.Update(It.Is<Cart>(c => c.Id == testCartItemId)), Times.Once);
            _mockCartRepo.Verify(repo => repo.Delete(It.Is<Cart>(c => c.Id == testCartItemId)), Times.Never);
            _mockProductRepo.Verify(repo => repo.Update(It.Is<Products>(p => p.Name == testProductName)), Times.Once);
        }
        [TestMethod]
        public void CartBLL_TotalPrice_CalculatesTotalPriceForCurrentOrder()
        {
            // Arrange
            const decimal productPrice = 50m;
            const string testProductName = "TestProduct";
            var testProduct = new Products { Name = testProductName, ProductId = Guid.NewGuid(), PriceCAD = productPrice };
            var testCart1 = new Cart { OrderID = 2, ProductName = testProductName, ItemsInCart = 2 };
            var testCart2 = new Cart { OrderID = 2, ProductName = testProductName, ItemsInCart = 3 };

            _mockOrderRepo.Setup(repo => repo.GetAll()).Returns(new List<Order> { new Order { Id = 1 } });
            _mockCartRepo.Setup(repo => repo.GetAll()).Returns(new List<Cart> { testCart1, testCart2 });
            _mockProductRepo.Setup(repo => repo.GetAll()).Returns(new List<Products> { testProduct });
            _mockProductRepo.Setup(repo => repo.Get(testProduct.ProductId)).Returns(testProduct);

            var bll = new CartBLL(_mockProductRepo.Object, _mockCartRepo.Object, _mockCountryRepo.Object, _mockOrderRepo.Object);

            // Act
            decimal result = bll.totalPrice();

            // Assert
            decimal expectedTotalPrice = (testCart1.ItemsInCart + testCart2.ItemsInCart) * productPrice;
            Assert.AreEqual(expectedTotalPrice, result);
        }
        [TestMethod]
        public void CartBLL_GetAllCountries_ReturnsAllCountriesFromRepository()
        {
            // Arrange
            var testCountries = new List<Country>
    {
        new Country { Id = 1, CountryName = "CountryA" },
        new Country { Id = 2, CountryName = "CountryB" },
        new Country { Id = 3, CountryName = "CountryC" }
    };

            _mockCountryRepo.Setup(repo => repo.GetAll()).Returns(testCountries);

            var bll = new CartBLL(_mockProductRepo.Object, _mockCartRepo.Object, _mockCountryRepo.Object, _mockOrderRepo.Object);

            // Act
            var result = bll.GetAllCountries().ToList();

            // Assert
            Assert.AreEqual(testCountries.Count, result.Count);
            for (int i = 0; i < testCountries.Count; i++)
            {
                Assert.AreEqual(testCountries[i].Id, result[i].Id);
                Assert.AreEqual(testCountries[i].CountryName, result[i].CountryName);
            }
        }

    }
}
