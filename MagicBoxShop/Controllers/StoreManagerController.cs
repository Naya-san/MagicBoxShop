using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicBoxShop.Models;
using System.Data;
using System.Data.Entity;
using System.IO;

namespace MagicBoxShop.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class StoreManagerController : Controller
    {
        private const String ImagePath = "/Content/Images";
        private const String DefaultImagePath = "/Content/Images/Default";
        private PresentsEntities presentsDB = new PresentsEntities();

        public ActionResult Index(int id = -1, bool isCategory = false, int page = 1, int itemPerPage = 10, int sort = 0 )
        {
            ViewBag.sort = sort;
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }          
            var products = new List<Product>();
            var notOrderList = presentsDB.Products.Where(e => e.IsValid == true).ToList();
            switch (sort)
            {
                case 0: products = notOrderList.OrderBy(e => e.Name).ToList();
                    break;
                case 1: products = notOrderList.OrderByDescending(e => e.Name).ToList();
                    break;
                case 2: products = notOrderList.OrderBy(e => e.Price).ToList();
                    break;
                case 3: products = notOrderList.OrderByDescending(e => e.Price).ToList();
                    break;
                case 4: products = notOrderList.OrderBy(e => e.Quantity).ToList();
                    break;
                case 5: products = notOrderList.OrderByDescending(e => e.Quantity).ToList();
                    break;
                case 6: products = notOrderList.OrderBy(e => e.Popularity).ToList();
                    break;
                case 7: products = notOrderList.OrderByDescending(e => e.Popularity).ToList();
                    break;
                default: products = notOrderList;
                    break;
            }
            var data = new PageableData<Product>(products.ToList(), page, itemPerPage);
            return View(data);
        }

        public ActionResult IndexConcrete(int id, bool isCategory, int page = 1, int itemPerPage = 10, int sort = 0)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            ViewBag.IsCategory = isCategory;
            ViewBag.categoryId = id;
            ViewBag.sort = sort;
            List<Product> notOrderList = new List<Product>();
            if (isCategory)
            {
                var categoryName = presentsDB.Categories.FirstOrDefault(e => e.CategoryId == id).Name;
                ViewBag.categoryName = categoryName;
                Category category = presentsDB.Categories.Find(id);
                foreach (var sub in category.Subcategories)
                {
                    notOrderList.AddRange(sub.Products);
                }
            }
            else
            {
                var categoryName = presentsDB.Subcategories.FirstOrDefault(e => e.SubcategoryId == id).Name;
                ViewBag.categoryName = categoryName;
                notOrderList = presentsDB.Subcategories.Find(id).Products.ToList();
            }
            notOrderList = notOrderList.Distinct().ToList();
            notOrderList = notOrderList.Where(e => e.IsValid).ToList();
            switch (sort)
            {
                case 0: notOrderList = notOrderList.OrderBy(e => e.Name).ToList();
                    break;
                case 1: notOrderList = notOrderList.OrderByDescending(e => e.Name).ToList();
                    break;
                case 2: notOrderList = notOrderList.OrderBy(e => e.Price).ToList();
                    break;
                case 3: notOrderList = notOrderList.OrderByDescending(e => e.Price).ToList();
                    break;
                case 4: notOrderList = notOrderList.OrderBy(e => e.Quantity).ToList();
                    break;
                case 5: notOrderList = notOrderList.OrderByDescending(e => e.Quantity).ToList();
                    break;
                case 6: notOrderList = notOrderList.OrderBy(e => e.Popularity).ToList();
                    break;
                case 7: notOrderList = notOrderList.OrderByDescending(e => e.Popularity).ToList();
                    break;
                default: notOrderList = notOrderList;
                    break;
            }
            var data = new PageableData<Product>(notOrderList, page, itemPerPage);
            return View(data);
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
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult Create()
        {
            Session["categoryList"] = presentsDB.Categories.ToList();
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            return View();
        }

        private void getLocation(Product pr)
        {
            if(pr.Subcategories == null)
            {
                pr.Subcategories = new List<Subcategory>();
            }
            else
            {
                List<Subcategory> sub = pr.Subcategories;
                foreach (var subcategory in sub)
                {
                    subcategory.Products.Remove(pr);
                }
                pr.Subcategories.Clear();
            }
    

            List<string> tags = (List<string>)Session["ListOfTags"];
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    if (Request[tag] == "on")
                    {
                        int id = Convert.ToInt32(tag.Substring(2));
                        Subcategory sc = presentsDB.Subcategories.Find(id);
                        pr.Subcategories.Add(sc);
                        sc.Products.Add(pr);
                    }
                }
            }
            if(pr.Subcategories.Count == 0)
            {
                Subcategory subcategory = presentsDB.Subcategories.First(e => e.Name.Equals("Разное") && e.RootCategory.Name.Equals("Разное"));
                pr.Subcategories.Add(subcategory);
                subcategory.Products.Add(pr);
            }
        }


        private bool loadAndSave(List<HttpPostedFileBase> uploadFile, string uniqueFolder)
        {
            bool flag = true;
            foreach (HttpPostedFileBase file in uploadFile)
            {
                if (file != null && file.ContentLength > 0)
                {
                    TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                    String uniqueName = Path.Combine(uniqueFolder, span.TotalMilliseconds.ToString() + ".jpg");
                    file.SaveAs(uniqueName);
                    flag = false;
                }
            }
            return flag;
        }

        private string plaseArts (List<HttpPostedFileBase> uploadFile)
        {
            string filePath = "";
            bool flag = true;
            if (uploadFile != null && uploadFile.Count > 0)
            {
                TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                String uniqueFolder = Path.Combine(Server.MapPath(ImagePath), span.TotalSeconds.ToString());
                filePath = Path.Combine(ImagePath, span.TotalSeconds.ToString());
                DirectoryInfo d = new DirectoryInfo(uniqueFolder);
                d.Create();
                flag = loadAndSave(uploadFile, uniqueFolder);
            }

            if (flag)
            {
                filePath = DefaultImagePath;
            }
            filePath = filePath.Replace('\\', '/');
            return filePath;
        }

        [HttpPost]
        public ActionResult Create(Product pr, List<HttpPostedFileBase> uploadFile )
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            pr.ArtUrl = plaseArts(uploadFile);

            pr.DateCreated = DateTime.Now;
            pr.Popularity = 0;
            pr.IsValid = true;

            getLocation(pr);
            

            if (ModelState.IsValid)
            {
                presentsDB.Products.Add(pr);
                presentsDB.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pr);
        }

        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Product pr = presentsDB.Products.Find(id);
            Session["categoryList"] = presentsDB.Categories.ToList();
            return View(pr);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product productNew, List<HttpPostedFileBase> uploadFile)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            var productOld = presentsDB.Products.First(e => e.ProductId==id);
            //productNew.ProductId = id;
            getLocation(productOld);
            if (uploadFile != null && uploadFile.Count > 0)
            {
                if (productOld.ArtUrl.Equals(DefaultImagePath))
                {
                    productNew.ArtUrl = plaseArts(uploadFile);
                }
                else
                {
                    loadAndSave(uploadFile, Server.MapPath(productOld.ArtUrl));
                    productNew.ArtUrl = productOld.ArtUrl;
                }
            }
            else
            {
                productNew.ArtUrl = productOld.ArtUrl;
            }
            //productNew.IsValid = productOld.IsValid;
            //productNew.DateCreated = productOld.DateCreated;
            //productNew.OrderDetails = productOld.OrderDetails;

            if(ModelState.IsValid)
            {
               // var entry = presentsDB.Entry(productOld);
                productOld.Name = productNew.Name;
                //productOld.ArtUrl = productNew.ArtUrl;
                productOld.Description = productNew.Description;
                productOld.Price = productNew.Price;
                productOld.Quantity = productNew.Quantity;
            //    productOld.Subcategories = productNew.Subcategories;
                
              //  presentsDB.Entry(productOld).CurrentValues.SetValues(productNew);
                //presentsDB.Products.Remove(productNew);
                presentsDB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productNew);
        }


        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Product pr = presentsDB.Products.Find(id);
            return View(pr);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            Product product = presentsDB.Products.Find(id);
            foreach (var subcategory in product.Subcategories)
            {
                subcategory.Products.Remove(product);
            }
            product.Subcategories.Clear();
            product.IsValid = false;
            presentsDB.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            presentsDB.Dispose();
            base.Dispose(disposing);
        }

    }
}
