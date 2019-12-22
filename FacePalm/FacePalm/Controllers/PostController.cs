using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FacePalm.Models;
using Microsoft.AspNet.Identity;

namespace FacePalm.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Post
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

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
            return View(post);
        }

        // GET: Post/New
        [HttpGet]
        public ActionResult New()
        {
            Post post = new Post();

            post.UserId = User.Identity.GetUserId();

            return View(post);
        }

        // POST: Post/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Post post)
        {
            string fileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
            string extension = Path.GetExtension(post.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            post.ImagePath = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            post.ImageFile.SaveAs(fileName);

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
        public ActionResult Edit([Bind(Include = "PostId,UserId,ReadableContent,Date")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int id)
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
    }
}