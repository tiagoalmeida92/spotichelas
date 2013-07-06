using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dto;
using Services;
using UI.Models;
using UI.Utils;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;


        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
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
                    var user = Membership.GetUser(model.UserName);
                    if (user != null && !user.IsApproved)
                        ModelState.AddModelError("", "Your email confirmation is pending, please check your inbox.");
                    else
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                var user = Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, false, null,
                                                 out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(model.UserName, "user");
                    _userService.CreateProfile(model.UserName);
                    string link = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
                                  (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port) +
                                  Url.Action("Verify") + "/" + user.ProviderUserKey;
                    EmailUtil.Send(user.Email, user.UserName, link);
                    return RedirectToAction("AccountCreated");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AccountCreated()
        {
            return View("AccountCreated");
        }

        [HttpGet]
        public ActionResult Verify(string id)
        {
            Guid guid;
            if (Guid.TryParse(id, out guid))
            {
                MembershipUser user = Membership.GetUser(guid);
                if (!user.IsApproved)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("LogOn");
                }
            }
            FormsAuthentication.SignOut();
            return View("Error");
        }

        [Authorize]
        public ActionResult Profile(String id)
        {
            var user = Membership.GetUser(id ?? User.Identity.Name);
            if (user == null)
            {
                return new HttpNotFoundResult("Non existant user.");
            }
            var profile = _userService.GetById(user.UserName);
            profile.Email = user.Email;
            if (profile.PhotoLocation == null)
            {
                profile.PhotoLocation = Url.Content("~/Resources/Photos/user-icon1.jpg");
            }
            else
            {
                profile.PhotoLocation = Url.Content(Path.Combine("~/Resources/Photos", profile.PhotoLocation));
            }
            return View(profile);
        }

        //
        // GET: /Account/Settings

        [Authorize]
        public ActionResult Settings()
        {
            var profile = _userService.GetById(User.Identity.Name);
            profile.Email = Membership.GetUser().Email;
            if (profile.PhotoLocation == null)
            {
                profile.PhotoLocation = Url.Content("~/Resources/Photos/user-icon1.jpg");
            }
            else
            {
                profile.PhotoLocation = Url.Content(Path.Combine("~/Resources/Photos", profile.PhotoLocation));
            }
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Settings(UserProfileDto profile, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                var randomFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(photo.FileName));
                profile.PhotoLocation = randomFileName;
                var path = Path.Combine(Server.MapPath("~/Resources/Photos"), randomFileName);
                photo.SaveAs(path);
            }
            _userService.Update(profile);
            return RedirectToAction("Profile");
        }


        [Authorize(Roles = "admin")]
        public ActionResult AdminArea()
        {
            return View(Membership.GetAllUsers().Cast<MembershipUser>());
        }


        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
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
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion

        [HttpPost]
        public ActionResult RemoveBySelf()
        {
            var user = User.Identity.Name;
            _userService.Delete(user);
            Membership.DeleteUser(user);
            return RedirectToAction("Index","Home");
        }

        //AJAX
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Remove(string id)
        {
            _userService.Delete(id);
            Membership.DeleteUser(id);
            return new EmptyResult();
        }

        //AJAX
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditRole(string id, string role)
        {
            if(!Roles.IsUserInRole(id, role))
                 Roles.AddUserToRole(id, role);
            return new EmptyResult();
        }
    }
}