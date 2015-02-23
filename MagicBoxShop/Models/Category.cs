using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MagicBoxShop.Models
{
    [Bind(Exclude = "СategoryId")]
    public class Category
    {
        [ScaffoldColumn(false)]
        public int CategoryId { get; set; }

        [DisplayName("Название категории")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"[A-Za-zА-Яа-я0-9,:._()\s\""]+",
       ErrorMessage = "Название может содержать только буквы, цифры, нижнее подчеркивание и знаки препинания.")]
        public string Name { get; set; }

        [DisplayName("Характеризирующий вопрос")]
        [RegularExpression(@"[A-Za-zА-Яа-я0-9,:.\?_()\s\""]+",
       ErrorMessage = "Вопрос может содержать только буквы, цифры, нижнее подчеркивание и знаки препинания.")]
        public string Question { get; set; }

        public virtual List<Subcategory> Subcategories { get; set; } 
    }
}