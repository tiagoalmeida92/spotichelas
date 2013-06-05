using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using UI.Models;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
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
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    ModelState.AddModelError("", "Your email isn't verified.");
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

        //[HttpPost]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Attempt to register the user
        //        MembershipCreateStatus createStatus;
        //        //user create isApproved = false
        //        MembershipUser user = Membership.CreateUser(model.UserName, model.Password, model.Email, null, null,
        //                                                    false, null,
        //                                                    out createStatus);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
        //            Roles.AddUserToRole(model.UserName, "user");
        //            //create profile
        //            var userProfiles = new UserService();
        //            userProfiles.InsertProfile(new Domain.Entities.User
        //                {
        //                    UserId = user.UserName,
        //                    Name = user.UserName
        //                });
        //            string link = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host +
        //                          (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port) +
        //                          "/Account/Verify/?" + user.ProviderUserKey;

        //            //Send email
        //            var email = new MailMessage("noreply.spotichelas@gmail.com", user.Email)
        //                {
        //                    Subject = "Verification Email",
        //                    Body = "Welcome to Spotichelas," + user.UserName +
        //                           "\n Activation link:" + link
        //                };
        //            var smtp = new SmtpClient("smtp.gmail.com")
        //                {
        //                    UseDefaultCredentials = false,
        //                    EnableSsl = true,
        //                    Credentials = new NetworkCredential("noreply.spotichelas@gmail.com", "isel2013")
        //                };
        //            smtp.Send(email);

        //            return RedirectToAction("AccountCreated");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(createStatus));
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        public ActionResult AccountCreated()
        {
            return View("AccountCreated");
        }

        [HttpGet]
        public ActionResult Verify(string id)
        {
            var guid = new Guid(id);
            MembershipUser user = Membership.GetUser(guid);
            if (user != null && !user.IsApproved)
            {
                user.IsApproved = true;
                Membership.UpdateUser(user);
                FormsAuthentication.SetAuthCookie(user.UserName, false);
            }
            return RedirectToAction("Index", "Home");
        }

        ////
        //// GET: /Account/Profile/id or no id
        //[Authorize]
        //public ActionResult Profile(string username  = null)
        //{
        //    MembershipUser user = Membership.GetUser(username ?? User.Identity.Name);
        //    var userService = new UserService();
        //    var userProfile = userService.GetUser(user.UserName);
        //    var viewModel = new UserProfileViewModel
        //        {
        //            LoginName = user.UserName,
        //            Email = user.Email,
        //            Name = userProfile.Name,
        //            PhotoLocation = userProfile.PhotoLocation
        //        };

        //    return View(viewModel);
        //}

        //
        // GET: /Account/Settings

        [Authorize]
        public ActionResult Settings()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangePhoto()
        {
            return View();
        }

        //[HttpPost]
        //[Authorize]
        //public ActionResult ChangePhoto(string newUrl)
        //{
        //    var userService = new UserService();
        //    Domain.Entities.User user = userService.GetUser(User.Identity.Name);
        //    user.PhotoLocation = newUrl;
        //    userService.UpdateUser(user);
        //    return RedirectToAction("Profile");
        //}

        //
        // GET: /Account/Settings

        [Authorize(Roles = "admin")]
        public ActionResult ManageUsers()
        {
            return View(Membership.GetAllUsers().Cast<MembershipUser>());
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddRole(string username, string newRole)
        {
            if (!Roles.IsUserInRole(username, newRole))
                Roles.AddUserToRole(username, newRole);
            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteUser(string username)
        {
            Membership.DeleteUser(username);
            return RedirectToAction("ManageUsers");
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
                    return "UserService name already exists. Please enter a different user name.";

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
    }
}