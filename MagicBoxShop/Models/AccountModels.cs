using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MagicBoxShop.Models
{
    public class AccountModels
    {
        public class ChangePasswordModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Текущий пароль")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Пароль {0} должен содержать минимум {2} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Новый пароль")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите новый пароль")]
            [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
        }

        public class LogOnModel
        {
            [Required]
            [Display(Name = "Логин")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня")]
            public bool RememberMe { get; set; }
        }

        public class RegisterModel
        {
            [Required]
            [RegularExpression(@"[A-Za-zА-Яа-я0-9._]+",
            ErrorMessage = "Логин может содержать только буквы, цифры, нижнее подчеркивание и точку.")]
            [Display(Name = "Логин")]
            public string UserName { get; set; }

            [Required]
            [RegularExpression(@"[A-Za-zА-Яа-я.\s]+",
           ErrorMessage = "Имя может содержать только буквы и точку.")]
            [Display(Name = "Полное имя")]
            public string FullName { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Электронная почта")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Пароль {0} должен содержать минимум {2} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [RegularExpression(@"[A-Za-zА-Яа-я0-9._]+",
           ErrorMessage = "Пароль может содержать только буквы, цифры, нижнее подчеркивание и точку.")]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [RegularExpression(@"[A-Za-zА-Яа-я0-9._]+",
           ErrorMessage = "Пароль может содержать только буквы, цифры, нижнее подчеркивание и точку.")]
            [Display(Name = "Подтвердите пароль")]
            [System.Web.Mvc.Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }

        }
    }
}