using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;

namespace MagicBoxShop.Controllers
{
    public class CheckoutController : Controller
    {
        PresentsEntities presentsDB = new PresentsEntities();
       
        public ActionResult AddressAndPayment()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Account");
            }
            var cart = ShoppingCart.GetCart(this.HttpContext);
            Session["total"] = cart.GetTotal().ToString();
            return View();
        }
        

        
        [HttpPost]
        public ActionResult AddressAndPayment(Order model)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var order = new Order();
                    TryUpdateModel(order);

                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    order.Status = "Необработанный";
                    order.OrderDetails = new List<OrderDetail>();
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                                            new {id = order.OrderId});

                }
                else return RedirectToAction("LogOn", "Account");
            }
           return View(model);

        }

        public ActionResult Complete(int id)
        {
           bool isValid = presentsDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

    }
}
