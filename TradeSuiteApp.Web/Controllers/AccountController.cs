using BusinessLayer;
using BusinessObject;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;


namespace TradeSuiteApp.Web.Controllers
{

    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IUserContract userBL;
        private IGeneralContract generalBL;
        private ILocationContract locationBL;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            userBL = new UserBL();
            generalBL = new GeneralBL();
            locationBL = new LocationBL();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Session["EmployeeName"] != null && User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<string> LoginAPI(string UserName, string Password)
        {

            var user = await UserManager.FindByNameAsync(UserName);

            var result = await SignInManager.PasswordSignInAsync(UserName, Password, false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    try
                    {
                        SetSessionDetails(user.Id, user.Email);
                        return GeneralBO.LocationID.ToString();
                        //return "1";
                    }
                    catch (Exception e)
                    {
                        return "0";
                    }
                default:
                    return "0";
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
             ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user =  UserManager.FindByName(model.UserName);

            //UnComment after email enabled for Registration
            //if (user != null)
            //{
            //    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            //    {
            //        ModelState.AddModelError("", "Email not yet confirmed.");
            //        return View(model);
            //    }
            //}

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    try
                    {
                        SetSessionDetails(user.Id, user.Email);
                        return RedirectToLocal(returnUrl);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Something went wrong, Please try again later");
                        return View(model);
                    }

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> AjaxLogin(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", Message = errors.FirstOrDefault().ErrorMessage }, JsonRequestBehavior.AllowGet);
            }

            var user = await UserManager.FindByNameAsync(model.UserName);

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    try
                    {
                        SetSessionDetails(user.Id, user.Email);
                        return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        return Json(new { Status = "failure", Message = "Something went wrong" }, JsonRequestBehavior.AllowGet);
                    }
                default:
                    return Json(new { Status = "failure", Message = "Incorrect login" }, JsonRequestBehavior.AllowGet);
            }

        }

        private void SetSessionDetails(int UserID, string Email)
        {
            UserBO userBO = userBL.GetLoginDetails(UserID, Email);
            FinYearBO finYearBO = generalBL.GetCurrentFinYear(userBO.ApplicationID);

            Session["UserID"] = userBO.UserID;
            Session["Email"] = userBO.Email;
            Session["UserName"] = userBO.UserName;
            Session["EmployeeName"] = userBO.EmployeeName;
            Session["CurrentLocationID"] = userBO.CurrentLocationID;
            Session["Designation"] = userBO.Designation;
            Session["DepartmentID"] = userBO.DepartmentID;
            Session["CompanyName"] = userBO.CompanyName;
            Session["AddressLine1"] = userBO.AddressLine1;
            Session["AddressLine2"] = userBO.AddressLine2;
            Session["AddressLine3"] = userBO.AddressLine3;
            Session["AddressLine4"] = userBO.AddressLine4;
            Session["AddressLine5"] = userBO.AddressLine5;
            Session["StateID"] = userBO.StateID;
            Session["CompanyName"] = userBO.CompanyName;
            Session["FinYear"] = finYearBO.Year;
            Session["FinYearTitle"] = finYearBO.Title;
            Session["ApplicationID"] = userBO.ApplicationID;
            
            Session["CustomerID"] = userBO.CustomerID;
            Session["SupplierID"] = userBO.SupplierID;
            Session["FinStartDate"] = General.FormatDate(finYearBO.StartDate);
            Session["Logo"] = userBO.Logo;
            Session["GSTNo"] = userBO.LocationGSTNo;
            Session["MobileNo"] = userBO.MobileNoInAddress;
            Session["CINNo"] = userBO.LocationCINNo;
            Session["PIN"] = userBO.PIN;
            Session["LandLine1"] = userBO.LandLine1;
            Session["ReportLogoPath"] = userBO.ReportLogoPath;
            Session["ReportfooterPath"] = userBO.ReportfooterPath ;
        }

        public JsonResult UpdateSessionLog()
        {
            try
            {
                int SessionID = Convert.ToInt16(Session["SessionID"] ?? 0);
                Session["SessionID"] = userBL.UpdateSessionLog(Convert.ToInt16(User.Identity.GetUserId()), SessionID);
                return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                //await _userManager.AddToRoleAsync(user.Id, "User");
                if (result.Succeeded)
                {
                    var userBL = new UserBL();
                    bool isAdded = userBL.AddUserLocation(user.Id.ToString(), model.LocationIDs);
                    ////Redirect to Login page.. No need to sign in that user

                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(Convert.ToInt32(userId), code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session["UserID"] = null;
            Session["Email"] = null;
            Session["UserName"] = null;
            Session["EmployeeName"] = null;
            Session["CurrentLocationID"] = null;
            Session["Designation"] = null;
            Session["DepartmentID"] = null;
            Session["CompanyName"] = null;
            Session["AddressLine1"] = null;
            Session["AddressLine2"] = null;
            Session["AddressLine3"] = null;
            Session["StateID"] = null;
            Session["CompanyName"] = null;
            Session["FinYear"] = null;
            Session["FinYearTitle"] = null;
            Session["ApplicationID"] = null;
            Session["FinStartDate"] = null;
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
            if (HttpContext.Request.Cookies[".AspNet.ApplicationCookie"] != null)
            {
                var c = new HttpCookie(".AspNet.ApplicationCookie");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            if (HttpContext.Request.Cookies["__RequestVerificationToken"] != null)
            {
                var c = new HttpCookie("__RequestVerificationToken");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            Session.Abandon();

            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        public JsonResult GetUserLocations()
        {
            try
            {
                List<LocationBO> Locations = locationBL.GetLocationListByUser(Convert.ToInt16(Session["UserID"]));

                return Json(
                    new { Status = "success", Locations = Locations, CurrentLocationID = Convert.ToInt16(Session["CurrentLocationID"]) },
                    JsonRequestBehavior.AllowGet
                    );
            }
            catch (Exception e)
            {

                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult ChangeUserLocation(int LocationID)
        {
            try
            {
                locationBL.SetCurrentLocation(Convert.ToInt16(Session["UserID"]), LocationID);
                SetSessionDetails(Convert.ToInt16(Session["UserID"]), Session["Email"].ToString());
                return Json(
                    new { Status = "success" },
                    JsonRequestBehavior.AllowGet
                    );
            }
            catch (Exception e)
            {

                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);
            }

        }

        [AllowAnonymous]
        public PartialViewResult MainMenu()
        {
            List<MenuItemModel> MenuItems = generalBL.GetMenuItems().Select(
                a => new MenuItemModel()
                {
                    ID = a.ID,
                    Name = a.Name,
                    ActionID = a.ActionID,
                    ParentID = a.ParentID,
                    SortOrder = a.SortOrder,
                    IconClass = a.IconClass,
                    BaseParentID = a.BaseParentID,
                    Area = a.Area,
                    Controller = a.Controller,
                    Action = a.Action,
                    URL = a.URL,
                    ReportURL = a.ReportURL
                }).ToList();
            return PartialView(MenuItems);
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }

        public void LogPerformance(string Area, string ControllerName, string ActionName, string ID, int Duration)
        {
            generalBL.LogPerformance(Area, ControllerName, ActionName, ID, Duration);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
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
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}