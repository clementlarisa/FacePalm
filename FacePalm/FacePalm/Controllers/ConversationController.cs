using FacePalm.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class ConversationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var conversation = db.Conversations.Find(id);
                //var chat = db.Chats.Find(conversation.ChatId);
                //chat.ActiveConversation = conversation;
                ViewBag.Conversation = conversation;
                ViewBag.currentUser = User.Identity.GetUserId();
                return View();
            }
            catch (Exception ex)
            {
                return View("../User/Error");
            }
        }


        public ActionResult Test()
        {

            ApplicationDbContext applicationDbContext = new ApplicationDbContext();

            var user = applicationDbContext.Users.Find(User.Identity.GetUserId());
            var chat = applicationDbContext.Chats.Find(user.Chat.ChatId);
            var conv = user.Conversations;
            //var conv = applicationDbContext.Conversations.Find(5);
            //var chats = conv.Chats;

            var toReturn = Json(new { count = conv.Count });
            return toReturn;

        }


        [HttpPost]
        public ActionResult CreateConversation(List<string> members, string name)
        {
           
            try
            {
                ApplicationDbContext applicationDbContext = new ApplicationDbContext();
                Conversation conversation = new Conversation();
                conversation.Name = name;
                foreach (string id in members)
                {
                    var user = applicationDbContext.Users.Find(id);
                    conversation.Users.Add(user);
                    conversation.Chats.Add(user.Chat);
                    user.Chat.Conversations.Add(conversation);


                    //ChatConversations chatConversations = new ChatConversations();
                    //chatConversations.ChatId = user.Chat.ChatId;
                    //chatConversations.ConversationId = conversation.ConversationId;
                    //applicationDbContext.ChatConversations.Add(chatConversations);
                }


                applicationDbContext.Conversations.Add(conversation);
                applicationDbContext.SaveChanges();



                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult GetConversation(int id)
        {

            try
            {
                ApplicationDbContext applicationDbContext = new ApplicationDbContext();

                var conv1 = applicationDbContext.Conversations.Find(id);

                string toReturn = "[";

                foreach(var msg in conv1.Messages)
                {
                    toReturn += "{\"text\" : \"" + msg.Content + "\", \"autor\": \"" + msg.User.FirstName + "\", \"uid\" :\"" + msg.User.UserId + "\", \"date\": \"" + msg.Date + "\"},"; 
                }

                toReturn = toReturn.Remove(toReturn.Length - 1);
                toReturn += "]";
                return Json(new { success = true, conv = toReturn });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult AddMessage(string text, int conversation)
        {

            try
            {
                ApplicationDbContext applicationDbContext = new ApplicationDbContext();

                var conv = applicationDbContext.Conversations.Find(conversation);
                var user = applicationDbContext.Users.Find(User.Identity.GetUserId());

                Message msg = new Message();
                msg.Content = text;
                msg.ConverstionId = conversation;
                msg.Conversation = conv;
                msg.User = user;
                msg.UserId = user.UserId;
                msg.Date = DateTime.Now;

                conv.Messages.Add(msg);
                user.Messages.Add(msg);

                applicationDbContext.Messages.Add(msg);
                applicationDbContext.SaveChanges();

                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
    }
}