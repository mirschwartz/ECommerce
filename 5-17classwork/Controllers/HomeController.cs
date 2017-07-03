using _5_17classwork.Models;
using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _5_17classwork.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            if (Session["ShoppingCart"] == null)
            {
                ShoppingCart cart = new ShoppingCart
                {
                    DateCreated = DateTime.Now
                };
                Session["ShoppingCart"] = repo.AddShoppingCart(cart);
                ViewBag.CartId = (int)Session["ShoppingCart"];
                ViewBag.CartItems = 0;
            }
            IndexVM vm = new IndexVM
            {
                Categories = repo.GetAllCategories(),
                User = "Home"
            };
            return View(vm);
        }

        public ActionResult GetCategories()
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            return Json(repo.GetAllCategories().Select(c => new {Id = c.Id, Name = c.Name }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProducts(int? categoryId)
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            var products = repo.GetAllProducts();
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }
            var result = products.Select(p => 
                new { Id = p.Id, Title = p.Title, Description = p.Description, Price = p.Price, ImageFile = p.ImageFile });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewDetail(int productId)
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            ViewDetailVM vm = new ViewDetailVM
            {
                Product = repo.GetProduct(productId)
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddToCart(ShoppingCartItem item)
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            item.CartId = (int)Session["ShoppingCart"];
            repo.AddShoppingCartItem(item);
            return Redirect("/");
        }

        public ActionResult ShoppingCart()
        {
            return View();
        }

        public ActionResult GetShoppingCartItems()
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            var cartId = (int)Session["ShoppingCart"];
            var items = repo.GetAllShoppingCartItems(cartId).Select(c => new
            {
                Id = c.Id,
                Quantity = c.Quantity,
                FileName = c.Product.ImageFile,
                Title = c.Product.Title,
                Description = c.Product.Description,
                Price = c.Product.Price,
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteFromCart(int id)
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            repo.DeleteFromCart(id);
        }
    }
}