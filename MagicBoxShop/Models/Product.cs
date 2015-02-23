using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MagicBoxShop.Models
{
     [Bind(Exclude = "ProductId")]
    public class Product
    {
        [ScaffoldColumn(false)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [StringLength(160)]
        [Display(Name = "Наименование товара")]
        [RegularExpression(@"[A-Za-zА-Яа-я0-9,:._()\s\""]+",
        ErrorMessage = "Название может содержать только буквы, цифры, нижнее подчеркивание и знаки препинания.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Цена")]
        [RangeAttribute(0.0, 900000.0,
        ErrorMessage = "Значение выходит за возможные пределы.")]
         public decimal Price { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Range(0, Int32.MaxValue,
        ErrorMessage = "Количество не может быть отрицательным")]
        [Display(Name = "Количество")]
         public int Quantity { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [StringLength(700)]
        [Display(Name = "Описание")]
        [RegularExpression(@"[A-Za-zА-Яа-я0-9,:._()\s]+",
        ErrorMessage = "Текст описания может содержать только буквы, цифры, нижнее подчеркивание и знаки препинания.")]
        public string Description { get; set; }

        [DisplayName("Продано одиниц товара")]
        [Range(0, Int32.MaxValue,
        ErrorMessage = "Значение выходит за возможные пределы.")]
        public int Popularity { get; set; }

        [DisplayName("Впервые появился в продаже ")]
        public System.DateTime DateCreated { get; set; }

        [DisplayName("Изображение")]
        [StringLength(1024)]
        public string ArtUrl { get; set; }

        [DisplayName("Действительный")]
        public bool IsValid { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
        public virtual List<Subcategory> Subcategories { get; set; }
    }
}
