using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;
using System.Windows;

namespace MagicBoxShop.Controllers
{
  //  [Authorize(Roles = "Admin")]
    public class SubcategoryController : Controller
    {
        private PresentsEntities db = new PresentsEntities();

        public ActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(db.Subcategories.ToList());
        }

        public ActionResult IndexСoncrete(int categoryId)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(db.Subcategories.Where(e=> e.RootCategory.CategoryId == categoryId).ToList());
        }

        public ActionResult Details(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Subcategory subcategory = db.Subcategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Session["categoryListForCreate"] = db.Categories.ToList();
            return View();
        }

        public ActionResult Concrete(int categoryId)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Category cc = db.Categories.Find(categoryId);
            Session["categoryListForCreate"] = new List<Category>() { cc };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subcategory subcategory)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Category cc = db.Categories.Find(Convert.ToInt32(Request["chooseCategory"]));
            subcategory.RootCategory = cc;
            subcategory.Products = new List<Product>();
            if (ModelState.IsValid)
            {
                cc.Subcategories.Add(subcategory);
                db.Subcategories.Add(subcategory);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
           return View();
        }

        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Subcategory subcategory = db.Subcategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            if (subcategory.Name.Equals("Разное"))
            {
                System.Windows.Forms.MessageBox.Show("Эту подкатегорию нельзя редактировать");
                return RedirectToAction("Index");
            }

            Session["categoryListForCreate"] = db.Categories.ToList();
            return View(subcategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Subcategory subcategoryEdited)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Subcategory subcategoryOld = db.Subcategories.Find(id);
            subcategoryEdited.SubcategoryId = id;
            int CategoryId = Convert.ToInt32(Request["InRootCategory"]);
            if(CategoryId != subcategoryOld.RootCategory.CategoryId)
            {
                Category cc = db.Categories.Find(CategoryId);
                subcategoryOld.RootCategory.Subcategories.Remove(subcategoryOld);
                subcategoryEdited.RootCategory = cc;
                cc.Subcategories.Add(subcategoryEdited);
            }else
            {
                subcategoryEdited.RootCategory = subcategoryOld.RootCategory;
            }
            subcategoryEdited.Products= subcategoryOld.Products;
            if(ModelState.IsValid)
            {
                db.Entry(subcategoryOld).CurrentValues.SetValues(subcategoryEdited);

                if (subcategoryOld.RootCategory==null || subcategoryOld.RootCategory.CategoryId != CategoryId)
                {
                    subcategoryOld.RootCategory = subcategoryEdited.RootCategory;
                    db.Subcategories.Remove(subcategoryEdited);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subcategoryEdited);
        }

        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Subcategory subcategory = db.Subcategories.Find(id);
            
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            if (subcategory.Name.Equals("Разное"))
            {
                System.Windows.Forms.MessageBox.Show("Эту подкатегорию нельзя удалять");
                return RedirectToAction("Index");
            }
            return View(subcategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Subcategory subcategory = db.Subcategories.Find(id);

            Subcategory reserv = db.Subcategories.First(e => e.Name == "Разное" && e.RootCategory.CategoryId == subcategory.RootCategory.CategoryId);//  (e => e.Name == "Разное");
            reserv.Products.AddRange(subcategory.Products);

            foreach (var product in subcategory.Products)
            {
                product.Subcategories.Remove(subcategory);
                product.Subcategories.Add(reserv);
            }
            db.Subcategories.Remove(subcategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}