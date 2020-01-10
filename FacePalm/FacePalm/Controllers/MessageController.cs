using FacePalm.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Show (int? id)
        {
            if(id == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var message = db.Messages.Find(id);
                ViewBag.Message = message;
                ViewBag.currentUser = User.Identity.GetUserId();
                return View();
            }
            catch(Exception ex)
            {
                return View("../User/Error");
            }
        }

        [HttpPost]
        public ActionResult New(Message message)
        {
            try
            {
                var user = db.Users.Find(message.UserId);
                var conversation = db.Conversations.Find(message.ConverstionId);
                conversation.Messages.Add(message);
                if(conversation.Users.FirstOrDefault(u => u.UserId == message.UserId) == null)
                {
                    conversation.Users.Add(user);
                }
                db.Messages.Add(message);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}