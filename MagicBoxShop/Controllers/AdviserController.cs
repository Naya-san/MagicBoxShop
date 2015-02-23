using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;
using MagicBoxShop.ViewModels;

namespace MagicBoxShop.Controllers
{
    public class AdviserController : Controller
    {

        private PresentsEntities db = new PresentsEntities();

        public ActionResult Index()
        {
            List<Category> categories = db.Categories.Where(e => e.Question.Length > 0).ToList();
            if(categories.Count == 0)
            {
                Session["preFinalResult"] = false;
                return RedirectToAction("SecondStep", "Adviser");
            }
            Session["preFinalResult"] = true;
            return View(categories);
        }


        public ActionResult SecondStep()
        {
            List<Subcategory> subcategories = new List<Subcategory>();
            if ((bool) Session["preFinalResult"])
            {
                foreach (var category in db.Categories.Where(e => e.Question.Length > 0).ToList())
                {
                    string ansver = Request[category.CategoryId.ToString()];
                    if (ansver != null && ansver.Equals("yes"))
                    {
                        subcategories.AddRange(category.Subcategories.Where(e => e.Question.Length > 0));
                    }
                }
            } else
            {
                subcategories = db.Subcategories.Where(e => e.Question.Length > 0).ToList();
            }
            
            if (subcategories.Count == 0)
            {
                return RedirectToAction("Browse", "Store", new { id = 1, isCategory = true });
            }

            return View(subcategories);
        }

        [HttpPost]
        public ActionResult Catalog()
        {
            Dictionary<Product, int> frequencies = new Dictionary<Product, int>();
            foreach (var subcategory in db.Subcategories.Where(e => e.Question.Length > 0).ToList())
            {
                string ansver = Request[subcategory.SubcategoryId.ToString()];
                if (ansver != null && ansver.Equals("yes"))
                {
                    foreach (var product in subcategory.Products)
                    {
                        if(frequencies.ContainsKey(product))
                        {
                            frequencies[product] += 1;
                        }
                        else
                        {
                            frequencies.Add(product, 1);
                        }
                    }
                }
            }
          
            if (frequencies.Count == 0)
            {
                return RedirectToAction("Browse", "Store", new {id = 1, isCategory = true});
            }
            IOrderedEnumerable<KeyValuePair<Product, int>> orderByDescending = frequencies.OrderByDescending(i => i.Value);
            List<ProductFrequency> helpList = new List<ProductFrequency>();
            foreach (var keyValuePair in orderByDescending)
            {
                helpList.Add(new ProductFrequency(keyValuePair.Key, keyValuePair.Value));
            }
            Session["finalResult"] = helpList;
            ViewBag.sortType = 2;
            var data = new PageableData<ProductFrequency>(helpList, 1, 10);
            return View(data);
        }

        public ActionResult Catalog(int page = 1, int itemPerPage = 10, int sort = 0)
        {
            List<ProductFrequency> listPr = (List<ProductFrequency>) Session["finalResult"];
            ViewBag.sortType = sort;
            List<ProductFrequency> helpList = new List<ProductFrequency>();
            switch (sort)
            {
                case 0: helpList = listPr.OrderBy(x => x.productCore.Price).ToList();
                    break;
                case 1: helpList = listPr.OrderByDescending(x => x.productCore.Price).ToList();
                    break;
                case 2: helpList = listPr.OrderByDescending(x => x.Frequency).ToList();
                    break;
                default:
                    helpList = listPr;
                    break;
            }
            Session["finalResult"] = helpList;
            var data = new PageableData<ProductFrequency>(helpList, page, itemPerPage);
            return View(data);
        }

    }
}
