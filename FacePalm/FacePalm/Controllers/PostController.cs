using FacePalm.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Post
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ViewBag.currentUserId = User.Identity.GetUserId();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View(db.Posts.ToList().OrderByDescending(x => x.Date));
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        // GET: Post/Show/5
        public ActionResult Show(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            ViewBag.numeCreator = db.Users.Find(post.UserId).FirstName;
            return View(post);
        }

        // GET: Post/New
        [HttpGet]
        public ActionResult New()
        {
            Post post = new Post();
            post.Albums = GetAllAlbums();

            post.UserId = User.Identity.GetUserId();

            return View(post);
        }

        // POST: Post/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Post post)
        {
            if (post.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                string extension = Path.GetExtension(post.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                post.ImagePath = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                post.ImageFile.SaveAs(fileName);
            }
            post.Albums = GetAllAlbums();
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("../User/Show/" + post.UserId);
            }

            return View(post);
        }

        // GET: Post/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,UserId,ImageDescription,Date")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        [HttpDelete]
        [Authorize(Roles = "Editor,Administrator")]
        // GET: Post/Delete/5
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();
            TempData["message"] = "Post has been successfully deleted!";
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllAlbums()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            var userId = User.Identity.GetUserId();
            // Extragem toate albumele create de utilizator
            var albums = from al in db.Albums
                         where al.UserId == userId
                         select al;

            // iteram prin albume
            foreach (var album in albums)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = album.AlbumId.ToString(),
                    Text = album.AlbumTitle.ToString()
                });
            }

            // returnam lista de albume
            return selectList;
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
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
    }
}