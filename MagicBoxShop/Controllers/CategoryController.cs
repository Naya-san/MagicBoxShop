using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;

namespace MagicBoxShop.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private PresentsEntities db = new PresentsEntities();

        public ActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(db.Categories.ToList());
        }

        public ActionResult Details(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Subcategory sub = new Subcategory();
            sub.Name = "Разное";
            sub.Products = new List<Product>();
            sub.Question = "";
            sub.RootCategory = category;
            if (ModelState.IsValid)
            {
                
                db.Categories.Add(category);
                db.Subcategories.Add(sub);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            if (category.Name.Equals("Разное"))
            {
                System.Windows.Forms.MessageBox.Show("Эту категорию нельзя редактировать");
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Category categoryOld = db.Categories.Find(id);
            category.CategoryId = id;
            if (ModelState.IsValid)
            {
                db.Entry(categoryOld).State = EntityState.Modified;
                db.Entry(categoryOld).CurrentValues.SetValues(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            if (category.Name.Equals("Разное"))
            {
                System.Windows.Forms.MessageBox.Show("Эту категорию нельзя удалять");
                return RedirectToAction("Index");
            }
            return View(category);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Category category = db.Categories.Find(id);
            Category reserv = db.Categories.First(e => e.Name == "Разное");
            reserv.Subcategories.AddRange(category.Subcategories);
            foreach (var sub in category.Subcategories)
            {
                sub.RootCategory = reserv;
                if(sub.Name.Equals("Разное"))
                {
                    sub.Name = category.Name;
                }
            }
            db.Categories.Remove(category);

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