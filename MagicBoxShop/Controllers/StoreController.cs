using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;

namespace MagicBoxShop.Controllers
{
    public class StoreController : Controller
    {
        PresentsEntities presentsDB = new PresentsEntities();

        public ActionResult Index()
        {
            var events = presentsDB.Subcategories.ToList();
            return View(events);
        }

        public ActionResult Browse(int id, bool isCategory, int page = 1, int itemPerPage = 10, int sort = 0)
        {
            
            ViewBag.categoryId = id;
            ViewBag.sort = sort;
            ViewBag.IsCategory = isCategory;
            List<Product> products = new List<Product>();
            if (isCategory)
            {
                var categoryName = presentsDB.Categories.FirstOrDefault(e => e.CategoryId == id).Name;
                ViewBag.categoryName = categoryName;               
                Category category = presentsDB.Categories.Find(id);
                foreach (var sub in category.Subcategories)
                {
                    products.AddRange(sub.Products);
                }
            }
            else
            {
                var categoryName = presentsDB.Subcategories.FirstOrDefault(e => e.SubcategoryId == id).Name;
                ViewBag.categoryName = categoryName;             
                products = presentsDB.Subcategories.Find(id).Products.ToList();
            }
            products = products.Distinct().ToList();
            products = products.Where(e => e.IsValid).ToList();

            if (sort == 0)
            {
                products = products.OrderBy(x => x.Price).ToList();
            }
            else if (sort == 1)
            {
                products = products.OrderByDescending(x => x.Price).ToList();
            }
            var data = new PageableData<Product>(products, page, itemPerPage);
            return View(data);
        }

        [HttpPost]
        public ActionResult Details(int id, int count)
        {
            var addedProduct = presentsDB.Products.Single(pr => pr.ProductId == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedProduct, count);
            return RedirectToAction("Details");
        }

        public ActionResult Details(int id)
        {
            var product = presentsDB.Products.Find(id);
            DirectoryInfo d = new DirectoryInfo(Server.MapPath(product.ArtUrl));
            List<string> fileList = new List<string>();
            foreach (FileInfo f in d.GetFiles("*.jpg"))
            {
                fileList.Add(Path.Combine(product.ArtUrl, f.Name));
            }
            ViewBag.ListImg = fileList;
            return View(product);
        }

       public ActionResult Menu()
        {
            var categories = presentsDB.Categories.ToList();

            return PartialView(categories);
        }
    }
}
