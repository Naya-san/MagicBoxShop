using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MagicBoxShop.Models
{
    [Bind(Exclude = "CommentId")]
    public class Comment
    {
        [ScaffoldColumn(false)]
        public int CommentId { get; set; }

        [DisplayName("Прокомментированый подарок")]
        virtual public Product Product { get; set; }

        [Required(ErrorMessage = "Введите текст комментария")]
        [RegularExpression(@"[A-Za-zА-Яа-я0-9,:._()\s-!\?]+", ErrorMessage = "Текст коментария может содержать только буквы, цифры, нижнее подчеркивание и знаки припинания.")]
        [StringLength(400)]
        [Display(Name = "Текст комментария")]
        public string Text { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(40)]
        [Display(Name = "Автор")]
        public string Name { get; set; }

        [DisplayName("Дата")]
        public System.DateTime DateCreated { get; set; }

        public bool isCheked { get; set; }
    }
}