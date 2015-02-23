using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;
using MagicBoxShop.ViewModels;

namespace MagicBoxShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        PresentsEntities presentsDB = new PresentsEntities();
       
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string productName = presentsDB.Carts
                .Single(item => item.RecordId == id).Product.Name;
            int itemCount = cart.RemoveFromCart(id);
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) +
                    " был удален с корзины.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results, JsonRequestBehavior.AllowGet);
         }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        public ActionResult  ChengeQuontityCart(int id, int q)
       {
           var cart = ShoppingCart.GetCart(this.HttpContext);
           int itemCount = cart.ChangeCart(id, q);
           var results = new ShoppingCartChangeViewModel()
           {
               CartTotal = cart.GetTotal(),
               CartCount = cart.GetCount(),
               ItemCount = itemCount,
               ChangeId = id
          };
           return Json(results, JsonRequestBehavior.AllowGet);
       }
    }
}
