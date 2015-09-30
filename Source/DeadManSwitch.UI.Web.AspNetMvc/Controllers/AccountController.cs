using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DeadManSwitch.Configuration;
using DeadManSwitch.Service;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using DeadManSwitch.UI.Models;
using DeadManSwitch.UI.Models.Builders;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNetMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
#if DEBUG
        private const int ReauthenticationMinutes = 2;
#else
        private const int ReauthenticationMinutes = 30;
#endif

        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;
        private readonly UserProfileModelBuilder ModelBuilder;

        public AccountController(IAccountService accountService)
        {
            AccountSvc = accountService;

            ModelBuilder = new UserProfileModelBuilder(accountService);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await LoginAndSetAuthCookie(model.UserName, model.Password, model.RememberMe);
                    if (response.IsSuccessful)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        AddErrors(response.LoginFailedUserMessageList);
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return View("Error");
            }
        }

        private async Task<LoginResponse> LoginAndSetAuthCookie(string userName, string password, bool rememberMe = false)
        {
            var response = await AccountSvc.LoginAsync(userName, password);
            if (response.IsSuccessful)
            {
                SetAuthCookie(response.User, rememberMe);
                Reauthenticator.SlideReauthenticatedExpiration(HttpContext, userName, ReauthenticationMinutes);
            }

            return response;
        }

        private void SetAuthCookie(User user, bool rememberMe)
        {
            var properties = new AuthenticationProperties() { IsPersistent = rememberMe };
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
        public async Task<ActionResult> Register()
        {
            if (!await AccountSvc.IsRegistrationOpenAsync()) return View("RegistrationClosed");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!await AccountSvc.IsRegistrationOpenAsync()) return View("RegistrationClosed");

            if (ModelState.IsValid)
            {
                List<string> createUserFailedMsgs = await CreateAccount(model);
                if (!createUserFailedMsgs.Any())
                {
                    await LoginAndSetAuthCookie(model.UserName, model.Password);
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

        private async Task<List<string>> CreateAccount(RegisterViewModel model)
        {
            User user = new User(model.UserName, model.Email, model.FirstName, model.LastName);

            List<string> registrationFailedMsgs = await AccountSvc.RegisterUserAsync(user, model.Password);

            return registrationFailedMsgs;
        }

        //
        // GET: /Account/Manage
        public async Task<ActionResult> Manage()
        {
            var model = await ModelBuilder.BuildUserProfileViewModelAsync(User.Identity.Name);

            return View("ProfileView", model);
        }

        //
        // GET: /Account/EditProfile
        [MustReauthenticate(ReauthenticationMinutes)]
        public async Task<ActionResult> EditProfile()
        {
            var model = await ModelBuilder.BuildUserProfileEditModelAsync(User.Identity.Name);

            return View("ProfileEdit", model);
        }

        //
        // POST: /Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MustReauthenticate(ReauthenticationMinutes)]
        public async Task<ActionResult> EditProfile(UserProfileEditModel model)
        {
            if (ModelState.IsValid)
            {
                var userProfile = model.ToServiceModel();
                await AccountSvc.UpdateProfileAsync(User.Identity.Name, userProfile);

                return RedirectToAction("Manage");
            }

            return RedirectToAction("EditProfile");
        }

        //
        // GET: /Account/EditProfile
        [MustReauthenticate(ReauthenticationMinutes)]
        public async Task<ActionResult> EditPreferences()
        {
            var model = await ModelBuilder.BuildUserPreferenceEditModelAsync(User.Identity.Name);

            return View("PreferenceEdit", model);
        }

        //
        // POST: /Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MustReauthenticate(ReauthenticationMinutes)]
        public async Task<ActionResult> EditPreferences(UserPreferenceEditModel model)
        {
            if (ModelState.IsValid)
            {
                var userProfile = model.ToServiceEntity();
                await AccountSvc.UpdatePreferencesAsync(User.Identity.Name, userProfile);

                return RedirectToAction("Manage");
            }

            return RedirectToAction("EditPreferences");
        }

        //
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordEditModel model)
        {
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            if (ModelState.IsValid)
            {
                bool passwordChanged = await AccountSvc.ChangePasswordAsync(User.Identity.GetUserName(), model.OldPassword, model.NewPassword);
                if (passwordChanged)
                {
                    Reauthenticator.SlideReauthenticatedExpiration(HttpContext, User.Identity.GetUserName(), ReauthenticationMinutes);

                    return await ChangePasswordResult(new ChangePasswordResultModel("Your password has been changed."));
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
        // GET: /Account/ChangePasswordResult
        public async Task<ActionResult> ChangePasswordResult(ChangePasswordResultModel model)
        {
            var userProfileModel = await ModelBuilder.BuildUserProfileViewModelAsync(User.Identity.Name);
            userProfileModel.Message = model.ResultMessage;

            return View("ProfileView", userProfileModel);
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
                //Don't specify a key so message will be displayed in validation summary
                ModelState.AddModelError(string.Empty, error);
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