using FacePalm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class FriendsController : Controller
    {
        // GET: Friendship
        //private ApplicationDbContext _applicationDBContext = new ApplicationDbContext();
        //[Authorize(Roles = "User,Editor,Administrator")]
        //public ActionResult Index(string userId)
        //{
        //    var friendsids = from friendships in _applicationDBContext.Friendships
        //                       where friendships.FirstUserId == userId
        //                        select friendships.FirstUserId;
        //    var friends2ids = from friendships in _applicationDBContext.Friendships
        //                   where friendships.SecondUserId == userId
        //                   select friendships.FirstUserId;
        //    friends2ids.Select(f => friendsids.Append(f));
        //    List<User> friends = new List<User>();
        //    foreach(var id in friendsids)
        //    {
        //        var friend = _applicationDBContext.Users.FirstOrDefault(a => a.UserId == id);
        //        friends.Add(friend);
        //    }
        //    ViewBag.Friends = friends;
        //    return View("User/Friends");
        //}
    }
}