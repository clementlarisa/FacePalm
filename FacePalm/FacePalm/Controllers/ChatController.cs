using FacePalm.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class ChatController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chat
        [Authorize(Roles="User,Administrator")]
        public ActionResult Index()
        {
            var currentUser = User.Identity.GetUserId();
            var user = db.Users.Find(currentUser);
            ViewBag.currentUser = currentUser;
            ViewBag.Friends = user.Friends;
            ViewBag.Conversations = user.Conversations;
            var chat = db.Chats.Find(user.Chat.ChatId);
            return View(chat);
        }
    }
}