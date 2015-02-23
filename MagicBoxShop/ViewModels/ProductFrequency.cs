using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MagicBoxShop.Models;

namespace MagicBoxShop.ViewModels
{
    public class ProductFrequency
    {
        public Product productCore { get; set; }
        public int Frequency { get; set; }

        public ProductFrequency(Product pr, int f)
        {
            productCore = pr;
            Frequency = f;
        }
    }
}