using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MagicBoxShop.ViewModels
{
    public class ShoppingCartChangeViewModel
    {
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int ChangeId { get; set; }
    }
}