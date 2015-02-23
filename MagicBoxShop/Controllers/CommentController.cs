using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MagicBoxShop.Models;

namespace MagicBoxShop.Controllers
{
    public class CommentController : Controller
    {

        PresentsEntities presentsDB = new PresentsEntities();

     //  [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            var comm = presentsDB.Comments.Where(e => e.isCheked == false).ToList();
            return View(comm);
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Comment comment = presentsDB.Comments.Find(id);
            return View(comment);
        }

        [ChildActionOnly]
        public ActionResult Browse(int id)
        {
            var comments = presentsDB.Comments.Where(e => e.Product.ProductId == id && e.isCheked).ToList();
            return View(comments);
        }
        
        [ChildActionOnly]
        public ActionResult Create(int id)
        {
            Session["productIdForCommment"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            comment.DateCreated = DateTime.Now;
            if(comment.Name == null && User.Identity.IsAuthenticated)
            {
                comment.Name = User.Identity.Name;
            }
            if (@Roles.IsUserInRole("Admin"))
            {
                comment.isCheked = true;
            }
            comment.Product = presentsDB.Products.Find(Session["productIdForCommment"]);
            if (ModelState.IsValid)
            {
                presentsDB.Comments.Add(comment);
                presentsDB.SaveChanges();
                if (User.Identity.IsAuthenticated && @Roles.IsUserInRole("Admin"))
                {
                    return RedirectToAction("Index", "Comment");
                }
                System.Windows.Forms.MessageBox.Show(comment.Name+", спасибо за Ваше мнение. Оно будет опубликовано после модерации.", "Благодарим");
                return RedirectToAction("Details", "Store", new {id = Session["productIdForCommment"]});
            }

            return View(comment);
        }

      //  [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Comment comment = presentsDB.Comments.Find(id);
            return View(comment);
        }

       // [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(int id, Comment pr)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }    
            Comment p = presentsDB.Comments.Find(id);
            p.isCheked = true;
            presentsDB.SaveChanges();
            return RedirectToAction("Index");
        }

       // [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Comment com = presentsDB.Comments.Find(id);
            return View(com);
        }

       // [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Comment com = presentsDB.Comments.Find(id);
            presentsDB.Comments.Remove(com);
            presentsDB.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}
