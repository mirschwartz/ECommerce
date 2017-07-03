using ECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5_17classwork
{
    public class LayoutPageActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ECommerceRepository repo = new ECommerceRepository(Properties.Settings.Default.ConStr);
            if (filterContext.HttpContext.Session.Contents["ShoppingCart"] != null)
            {
                filterContext.Controller.ViewBag.CartItems = repo.GetAmountOfItemsInCart((int)filterContext.HttpContext.Session.Contents["ShoppingCart"]);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}