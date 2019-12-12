using FacePalm.Models;
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
        private UserDBContext _userDB = new UserDBContext();

        public ActionResult Index()
        {
            var users = from user in _userDB.Users
                        orderby user.FirstName
                        select user;
            ViewBag.Users = users;
            return View();
        }

        public ActionResult Show(int id)
        {
            User user = _userDB.Users.Find(id);
            ViewBag.User = user;
            return View();
        }

        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(User user)
        {
            try
            {
                _userDB.Users.Add(user);
                _userDB.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            User user = _userDB.Users.Find(id);
            ViewBag.User = user;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, User requestUser)
        {
            try
            {
                User user = _userDB.Users.Find(id);
                if (TryUpdateModel(user))
                {
                    user.FirstName = requestUser.FirstName;
                    user.LastName = requestUser.LastName;
                    user.Email = requestUser.Email;
                    user.BirthDate = requestUser.BirthDate;
                    user.ProfileImage = requestUser.ProfileImage;
                    _userDB.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            User user = _userDB.Users.Find(id);
            _userDB.Users.Remove(user);
            _userDB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
