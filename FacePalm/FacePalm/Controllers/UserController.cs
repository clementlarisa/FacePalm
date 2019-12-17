using FacePalm.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private ApplicationDbContext _applicationDBContext = new ApplicationDbContext();
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var users = from user in _applicationDBContext.Users
                        orderby user.FirstName
                        select user;
            ViewBag.Users = users;
            return View();
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Show(string id)
        {
            User user = _applicationDBContext.Users.Find(id);
            var posts = _applicationDBContext.Posts.Where(p => p.UserId == id);
            ViewBag.User = user;
            ViewBag.CurrentUser = User.Identity.GetUserId();
            ViewBag.Posts = posts;
            return View();
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Edit(string id)
        {
            User user = _applicationDBContext.Users.Find(id);
            ViewBag.User = user;
            if (user.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                
                return View(user);
            }
            else
            {
                TempData["message"] = "You do not have the rights to modify!";
                ViewBag.message = TempData["message"].ToString();

                return View("Error");
            }
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        [HttpPut]
        public ActionResult Edit(string id, User requestUser)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    User user = _applicationDBContext.Users.Find(id);
                    ViewBag.User = user;
                    if (user.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(user))
                        {
                            if(!String.IsNullOrEmpty(requestUser.FirstName))
                            {
                                user.FirstName = requestUser.FirstName;
                            }
                            if (!String.IsNullOrEmpty(requestUser.LastName))
                            {
                                user.LastName = requestUser.LastName;
                            }
                            if (!String.IsNullOrEmpty(requestUser.BirthDate.ToString()))
                            {
                                user.BirthDate = requestUser.BirthDate;
                            }
                            if (!String.IsNullOrEmpty(requestUser.Education))
                            {
                                user.Education = requestUser.Education;
                            }
                            if (!String.IsNullOrEmpty(requestUser.Job))
                            {
                                user.Job = requestUser.Job;
                            }
                            user.ProfilePrivacy = requestUser.ProfilePrivacy;
                            user.RelationshipStatus = requestUser.RelationshipStatus;
                            user.Gender = requestUser.Gender;
                            //user.ProfilePicture = requestUser.ProfilePicture;
                            _applicationDBContext.SaveChanges();
                        }
                        return RedirectToAction("Show/"+user.UserId);
                    }
                    else
                    {
                        TempData["message"] = "You do not have the rights to modify!";
                        ViewBag.message = TempData["message"].ToString();
                        
                        return View("Error");
                    }
                }
                else
                {
                    TempData["message"] = "Something went wrong...";
                    ViewBag.message = TempData["message"].ToString();
                    return View("Error");
                }
                
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            User user = _applicationDBContext.Users.Find(id);
            if (user.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                /*
                var albums = _applicationDBContext.Albums.Select(a => a.UserId == user.UserId);
                _applicationDBContext.Albums.Remove(albums);
                Comment comments = _applicationDBContext.Comments.FirstOrDefault(c => c.UserId == user.UserId);
                _applicationDBContext.Comments.Remove(comments);
                _applicationDBContext.Groups.Where(g => g.UsersIds.Remove(user.UserId));
                Conversation
                _applicationDBContext.Conversations.Remove
                */
                
                if (id == null)
                {
                    return View("Error");
                }

                _applicationDBContext.Users.Remove(user);
                _applicationDBContext.SaveChanges();
                TempData["message"] = "The user has been successfully removed!";
                ViewBag.message = TempData["message"].ToString();
                /*
                var _user = await UserManager.FindByIdAsync(id);

                //List Logins associated with user
                var logins = _user.Logins;

                //Gets list of Roles associated with current user
                var rolesForUser = await UserManager.GetRolesAsync(id);

                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await UserManager.RemoveFromRoleAsync(_user.Id, item);
                        }
                    }

                    //Delete User
                    await UserManager.DeleteAsync(user);
                }
                */
                return View("../Home/Index");
            }
            else
            {
                TempData["message"] = "You do not have permission to delete this profile!";
                ViewBag.message = TempData["message"].ToString();
                return View("Error");
            }
        }
           
    }
}
