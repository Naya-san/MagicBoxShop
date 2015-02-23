using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MagicBoxShop.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<PresentsEntities>
    {
        protected override void Seed(PresentsEntities context)
        {
           
           var categoris = new List<Category>()
                                {
                                    new Category
                                        {
                                            Name = "Разное",
                                            Question = "",
                                            Subcategories = new List<Subcategory>()
                                        }
                                };
            categoris.ForEach(c => context.Categories.Add(c));
            var subcategoris = new List<Subcategory>()
                                {
                                    new Subcategory()
                                        {
                                            Name = "Разное",
                                            Question = "",
                                            Products = new List<Product>(),
                                            RootCategory =  categoris.Single(e=> e.Name.Equals("Разное"))
                                        }
                                };
            subcategoris.ForEach(c => context.Subcategories.Add(c));
            base.Seed(context);
            context.SaveChanges();
        }
    }
}