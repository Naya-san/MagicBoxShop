using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;
using System.Web.Security;


namespace MagicBoxShop.Controllers
{
    public class HomeController : Controller
    {

        private PresentsEntities presentsDB = new PresentsEntities();

        public ActionResult Index(int page = 1, int itemPerPage = 10)
        {
            var products = presentsDB.Products.ToList();
            var data = new PageableData<Product>(products.OrderByDescending(e => e.Popularity).Take(8).ToList() , page, itemPerPage);
            return View(data);

          
        }

        private List<Product> GetTopSellingProducts(int count)
        {
            return presentsDB.Products.Take(count).ToList();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Как нас найти:";
            return View();
        }
    }
}
