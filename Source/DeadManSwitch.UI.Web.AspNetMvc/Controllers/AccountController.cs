using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DeadManSwitch.Service;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using DeadManSwitch.UI.Web.AspNetMvc.Models;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNetMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;

        public AccountController(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = LoginAndSetAuthCookie(model.UserName, model.Password);
                if (user != null)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private User LoginAndSetAuthCookie(string userName, string password)
        {
            var user = AccountSvc.Login(userName, password);
            if (user != null)
            {
                SetAuthCookie(user);
            }

            return user;
        }

        private void SetAuthCookie(User user)
        {
            var properties = new AuthenticationProperties() {IsPersistent = true};
            var id = BuildClaimsIdentity(user);

            Request.GetOwinContext().Authentication.SignIn(properties, id);
        }

        private ClaimsIdentity BuildClaimsIdentity(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            return new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!AccountSvc.IsRegistrationOpen()) return View("RegistrationClosed");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!AccountSvc.IsRegistrationOpen()) return View("RegistrationClosed");

            if (ModelState.IsValid)
            {
                List<string> createUserFailedMsgs = CreateAccount(model);
                if (!createUserFailedMsgs.Any())
                {
                    LoginAndSetAuthCookie(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(createUserFailedMsgs);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private List<string> CreateAccount(RegisterViewModel model)
        {
            User user = new User(model.UserName, model.Email, model.FirstName, model.LastName);

            IEnumerable<string> registrationFailedMsgs = this.AccountSvc.RegisterUser(user, model.Password);

            return new List<string>(registrationFailedMsgs);
        }

        //
        // GET: /Account/Manage
        public ActionResult ChangePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ManageUserViewModel model)
        {
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            if (ModelState.IsValid)
            {
                bool passwordChanged = AccountSvc.ChangePassword(User.Identity.GetUserName(), model.OldPassword, model.NewPassword);
                if (passwordChanged)
                {
                    return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    AddErrors(new string[] { "Your password could not be changed."});
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IEnumerable<string> msgList)
        {
            foreach (var error in msgList)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}