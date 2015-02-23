using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;

namespace MagicBoxShop
{
    public static class Helper
    {
        

        public static MvcHtmlString PageLinks(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            StringBuilder builder = new StringBuilder();
     
            //Prev
            var prevBuilder = new TagBuilder("a");
            prevBuilder.AddCssClass("m-btn mini");
            prevBuilder.InnerHtml = "«";
            if (currentPage == 1)
            {
                prevBuilder.MergeAttribute("href", "#");
            }
            else
            {
                prevBuilder.MergeAttribute("href", pageUrl.Invoke(currentPage - 1));      
            }
            builder.AppendLine(prevBuilder.ToString());
            //По порядку
            for (int i = 1; i <= totalPages; i++)
            {
                //Условие что выводим только необходимые номера
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    var subBuilder = new TagBuilder("a");
                    subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                    if (i == currentPage)
                    {
                        subBuilder.MergeAttribute("href", "#");
                        subBuilder.AddCssClass("m-btn mini active");
                    }
                    else
                    {
                        subBuilder.AddCssClass("m-btn mini");
                        subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    }
                    builder.AppendLine(subBuilder.ToString());
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    //Троеточие первое
                    builder.AppendLine("<a href=\"#\" class=\"m-btn mini\">...</a>");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    //Троеточие второе
                    builder.AppendLine("<a href=\"#\" class=\"m-btn mini\">...</a>");
                }
            }
            //Next
            var nextBuilder = new TagBuilder("a");
            nextBuilder.InnerHtml = "»";
            nextBuilder.AddCssClass("m-btn mini");
            if (currentPage == totalPages)
            {
                nextBuilder.MergeAttribute("href", "#");
            }
            else
            {
                nextBuilder.MergeAttribute("href", pageUrl.Invoke(currentPage + 1));
            }
            builder.AppendLine(nextBuilder.ToString());
            return new MvcHtmlString(builder.ToString());
        }
    }
}