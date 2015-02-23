using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;
using System.Windows.Forms;

namespace MagicBoxShop.Controllers
{
    public class OrderController : Controller
    {
        private PresentsEntities db = new PresentsEntities();

         // [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(db.Orders.ToList().OrderByDescending(e => e.OrderDate));
        }

         // [Authorize(Roles = "Admin")]
        public ActionResult IndexConrete(string status)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            ViewBag.Status = status;
            return View(db.Orders.Where(e=> e.Status.Equals(status)).ToList().OrderBy(e => e.OrderDate));

        }

       // [Authorize(Roles = "Admin")]
        public ActionResult Details(int id = 0)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

       // [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (order.Status.Equals("Выполненный") || order.Status.Equals("Отменен"))
            {
                MessageBox.Show("Это состаяние является конечным. Его нельзя изменить.", "Невозможно изменить");
                return RedirectToAction("Index");
            }
            var types = new List<string>() { "Необработанный", "Обработанный", "Выполненный", "Отменен" };
            ViewBag.StatusList = types;
            return View(order);
        }

       // [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order ordern, int id )
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Order order = db.Orders.Find(id);
           
            string eS = order.Status;
            order.Status = Request["statusNew"];
            if (order.Status.Equals("Выполненный") && !eS.Equals(order.Status))
            {
                foreach (var od in order.OrderDetails)
                {
                    if (od.Product.Quantity - od.Quantity < 0)
                    {
                       MessageBox.Show("Для выполнения заказа на складе не хватает " + od.Product.Name + ". Необходимо еще " + (od.Quantity - od.Product.Quantity) + " экземпляров", "Не достаточно товара");
                       return RedirectToAction("IndexConrete", new { status = eS });
                    }

                }
                foreach(var od in order.OrderDetails)
                {

                        od.Product.Quantity -= od.Quantity;
                        od.Product.Popularity += od.Quantity;

                }
            }
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexConrete", new {status = eS});
        }

      
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

      //  [Authorize(Roles = "User")]
        public ActionResult PersonalOrders(){
            if (!User.IsInRole("User"))
            {
                return HttpNotFound();
            }
            List<Order> orders = db.Orders.Where(e => e.Username == User.Identity.Name).ToList();

            return View(orders.OrderByDescending(e => e.OrderDate));
        }
    }
}