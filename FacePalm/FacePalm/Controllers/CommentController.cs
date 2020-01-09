using FacePalm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult New(Comment comment)
        {
            try
            {
                var user = db.Users.Find(comment.UserId);
                comment.User = user;
                user.Comments.Add(comment);
                var postIdInt = Int32.Parse(comment.PostId);
                var post = db.Posts.Find(postIdInt);
                comment.Post = post;
                post.Comments.Add(comment);
                db.Comments.Add(comment);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            if(TempData["message"] != null)
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View(comment);
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comment = db.Comments.Find(id);
                comment.Content = requestComment.Content;
                comment.Date = DateTime.Now;
                db.SaveChanges();
                TempData["message"] = "Comment changed successfully!";
                ViewBag.message = TempData["message"].ToString();
                return RedirectToAction($"../Post/Show/{comment.PostId}");
            }
            catch(Exception ex)
            {
                TempData["message"] = ex.ToString();
                ViewBag.message = TempData["message"].ToString();

                return View($"/Comment/Edit/{requestComment.CommentId}");
            }

        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            var comment = db.Comments.Find(id);
            try
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                TempData["message"] = $"Something bad happened. {ex.ToString()}";
                ViewBag.message = TempData["message"].ToString();

                return Json(new { success = false });
            }
            

        }
    }
}