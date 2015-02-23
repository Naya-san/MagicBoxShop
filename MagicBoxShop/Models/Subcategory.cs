using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace MagicBoxShop.Models
{
    [Bind(Exclude = "SubcategoryId")]
    public class Subcategory
    {
        [ScaffoldColumn(false)]
        public int SubcategoryId { get; set; }

        [DisplayName("Название подкатегории")]
        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"[A-Za-zА-Яа-я0-9,:._()\s\""]+",
       ErrorMessage = "Название может содержать только буквы, цифры, нижнее подчеркивание и знаки препинания.")]
        public string Name { get; set; }

        [DisplayName("Характеризирующий вопрос")]
        [RegularExpression(@"[A-Za-zА-Яа-я0-9,:.\?_()\s\""]+",
       ErrorMessage = "Вопрос может содержать только буквы, цифры, нижнее подчеркивание и знаки препинания.")]
        public string Question { get; set; }

        [DisplayName("Родительская категория")]
        public virtual Category RootCategory { get; set; }

        public virtual List<Product> Products { get; set; } 
    }
}