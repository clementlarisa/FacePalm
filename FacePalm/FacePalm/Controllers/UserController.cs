using FacePalm.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewBag.User = user;
            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
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
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, User requestUser)
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
                            user.FirstName = requestUser.FirstName;
                            user.LastName = requestUser.LastName;
                            user.Email = requestUser.Email;
                            user.BirthDate = requestUser.BirthDate;
                            //user.ProfilePicture = requestUser.ProfilePicture;
                            _applicationDBContext.SaveChanges();
                        }
                        return RedirectToAction("Show");
                    }
                    else
                    {
                        TempData["message"] = "You do not have the rights to modify!";
                        return RedirectToAction("Show");
                    }
                }
                else
                {
                    return View(requestUser);
                }
                
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            User user = _applicationDBContext.Users.Find(id);
            if (user.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                _applicationDBContext.Users.Remove(user);
                _applicationDBContext.SaveChanges();
                TempData["message"] = "The user has been removed!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "You do not have the rights to modify!";
                return RedirectToAction("Index");
            }
        }
    }
}
