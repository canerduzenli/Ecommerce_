using ShoppingCart.Data;
using ShoppingCart.Models;



namespace ShoppingCart.BLL
{
    public class CartBLL
    {
        private readonly IRepository<Products, Guid> _productRepo;
        private readonly IRepository<Cart, int> _cartRepo;
        private readonly IRepository<Country, int> _countryRepo;
        private readonly IRepository<Order, int> _orderRepo;
        public CartBLL(IRepository<Products, Guid> productRepo, IRepository<Cart, int> cartRepo, IRepository<Country, int> countryRepo, IRepository<Order, int> orderRepo)
        {
            _productRepo = productRepo;
            _cartRepo = cartRepo;
            _countryRepo = countryRepo;
            _orderRepo = orderRepo;
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            int orderCount = _orderRepo.GetAll().Count();

            var cartItemsWithMaxOrderID = _cartRepo.GetAll().Where(cartItem => cartItem.OrderID == orderCount + 1).ToList();

            return cartItemsWithMaxOrderID;
        }

        public void RemoveFromCart(int cartItemId)
        {
            ICollection<Products> AllProducts = _productRepo.GetAll();
            if (AllProducts == null) return; // Return if no products found

            var cart = _cartRepo.Get(cartItemId);
            if (cart != null)
            {
                cart.ItemsInCart--;
                if (cart.ItemsInCart == 0)
                {
                    _cartRepo.Delete(cart);
                }
                else
                {
                    _cartRepo.Update(cart);
                }

                var matchingProduct = AllProducts.FirstOrDefault(p => p.Name == cart.ProductName);
                if (matchingProduct != null)
                {
                    var productToUpdate = _productRepo.Get(matchingProduct.ProductId);
                    if (productToUpdate != null)
                    {
                        productToUpdate.AvailableQuantity++;
                        _productRepo.Update(productToUpdate);
                    }
                }
            }
        }


        public decimal totalPrice()
        {
            int orderCount = _orderRepo.GetAll().Count();
            var cartItems = _cartRepo.GetAll().Where(cartItem => cartItem.OrderID == orderCount + 1).ToList();

            decimal totalPrice = 0m;

            foreach (var cartItem in cartItems)
            {
                var product = _productRepo.Get(_productRepo.GetAll().FirstOrDefault(p => p.Name == cartItem.ProductName).ProductId);

                if (product != null)
                {
                    totalPrice += product.PriceCAD * cartItem.ItemsInCart;
                }
            }

            return totalPrice;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _countryRepo.GetAll();
        }

    }
}