using System;
using System.Web.Mvc;
using System.Web.Security;
using MagicBoxShop.Models;

namespace MagicBoxShop.Controllers
{
    public class AccountController : Controller
    {
        private void MigrateShoppingCart(string UserName)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(AccountModels.LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    MigrateShoppingCart(model.UserName);
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Имя пользователя или пароль введены неверно.");
                } 
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountModels.RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, "question", "answer", true, null, out createStatus);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    MigrateShoppingCart(model.UserName);
                    Roles.AddUserToRole(model.UserName, "User");
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Home");
                } 
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(AccountModels.ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный текущий пароль или новый пароль введен некорректно.");
                }
            }
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Пользователь с таким логином уже существует. Пожалуйста, введите другой логин.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Пользователь с таким электронным адресом уже существует. Пожалуйста, введите другую почту.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Пароль введен некорректно. Пожалуйста, введите другой пароль.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Электронная почта введена некорректно. Пожалуйста, попробуйте еще раз.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Ответ неверный. Подумайте еще.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Вопрос неправильный. Подумайте еще.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Логин введен некорректно. Пожалуйста, попробуйте еще раз.";

              default:
                    return "Неизвестная ошибка. Пожалуйста, проверьте введенные данные и попробуйте еще раз. Если ошибка повторится - обратитесь к администратору.";
            }
        }
        #endregion
    }
}
