using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data
{
    public class ECommerceRepository
    {
        private string _connection { get; set; }
        public ECommerceRepository(string connection)
        {
            _connection = connection;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                return context.Categories.ToList();
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Product>(c => c.Category);
                context.LoadOptions = loadOptions;
                return context.Products.ToList();
            }
        }

        public Product GetProduct(int Id)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                return context.Products.FirstOrDefault(p => p.Id == Id);
            }
        }

        public IEnumerable<ShoppingCartItem> GetAllShoppingCartItems(int cartId)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ShoppingCartItem>(s => s.Product);
                context.LoadOptions = loadOptions;
                return context.ShoppingCartItems.Where(s => s.CartId == cartId).ToList();
            }
        }

        public void AddCategory(Category category)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                context.Categories.InsertOnSubmit(category);
                context.SubmitChanges();
            }
        }

        public void AddProduct(Product product)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                context.Products.InsertOnSubmit(product);
                context.SubmitChanges();
            }
        }

        public int AddShoppingCart(ShoppingCart cart)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                context.ShoppingCarts.InsertOnSubmit(cart);
                context.SubmitChanges();
                return cart.Id;
            }
        }

        public void AddShoppingCartItem(ShoppingCartItem item)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                context.ExecuteCommand("IF EXISTS(SELECT * FROM ShoppingCartItems WHERE CartId = {0} AND ProductId = {1})" +
                    " UPDATE ShoppingCartItems SET Quantity += {2} WHERE CartId = {0} AND ProductId = {1}" +
                    " ELSE INSERT INTO ShoppingCartItems (CartId, ProductId, Quantity) VALUES ({0}, {1}, {2})",
                    item.CartId, item.ProductId, item.Quantity);
            }
        }

        public int GetAmountOfItemsInCart(int cartId)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                var cart = context.ShoppingCartItems.Where(s => s.CartId == cartId).ToList();
                return cart.Sum(c => c.Quantity);
            }
        }

        public void DeleteFromCart(int id)
        {
            using (var context = new ECommerceDataContext(_connection))
            {
                context.ExecuteCommand("DELETE FROM ShoppingCartItems WHERE Id = {0}", id);
            }
        }
    }
}