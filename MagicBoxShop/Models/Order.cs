using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MagicBoxShop.Models
{
   [Bind(Exclude = "OrderId")]
    public class Order
    {
        [ScaffoldColumn(false)]
        [Display(Name = "Номер заказа")]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Дата оформления заказа")]
        public System.DateTime OrderDate { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "ФИО заказчика")]
        [RegularExpression(@"[A-Za-zА-Яа-я,.\s]+",
            ErrorMessage = "Поле может содержать только буквы и знаки припенания.")]
        [StringLength(160)]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [RegularExpression(@"[A-Za-zА-Яа-я0-9(),.\s]+",
            ErrorMessage = "Поле может содержать только буквы, цифры и знаки препинания.")]
        [StringLength(120)]
        [DisplayName("Адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [StringLength(24)]
        [DisplayName("Номер контактного телефона")]
     //   [RegularExpression(@"d+",
      //      ErrorMessage = "Указаные данные не соответстуют формату телефонного номера")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DisplayName("Электронная почта")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Указаный адрес не является електронной почтой")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Общая сумма")]
        public decimal Total { get; set; }

     //   [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Статус")]
        public string Status { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}