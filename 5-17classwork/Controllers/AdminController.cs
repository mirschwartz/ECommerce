using _5_17classwork.Models;
using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _5_17classwork.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            IndexVM vm = new IndexVM
            {
                Categories = repo.GetAllCategories(),
                User = "Admin"
            };
            return View("../Home/Index", vm);
        }

        [Authorize]
        [HttpPost]
        public void AddCategories(string categoryName)
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            repo.AddCategory(new Category { Name = categoryName });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddProduct(Product product, HttpPostedFileBase image)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            image.SaveAs(Server.MapPath("~/Images/") + fileName);
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            product.ImageFile = fileName;
            repo.AddProduct(product);
            return Redirect("/admin/index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            UserManager manager = new UserManager(Properties.Settings.Default.ConStr);
            AdminUser user = manager.Login(email, password);
            if(user == null)
            {
                return Redirect("/admin/login");
            }
            FormsAuthentication.SetAuthCookie(email, true);
            return Redirect("/admin/index");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}