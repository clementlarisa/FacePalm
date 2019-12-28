using FacePalm.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class AlbumController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Album
        [Authorize(Roles = "User, Editor,Administrator")]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var userId = User.Identity.GetUserId();

            var usersAlbums = from al in db.Albums
                             where al.UserId == userId
                             select al;
            ViewBag.Albums = usersAlbums;

            return View();
        }

        public ActionResult Show(int id)
        {
            Album album = db.Albums.Find(id);
            var posts = album.Post;
            ViewBag.Posts = posts;
            ViewBag.afisareButoane = false;
            if (User.Identity.GetUserId() == album.UserId || User.IsInRole("Administrator"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Administrator");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            ViewBag.numeCreator = db.Users.Find(album.UserId).FirstName;
            return View(album);
        }

        public ActionResult New()
        {
            Album al = new Album();
            al.UserId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        public ActionResult New(Album al)
        {
            try
            {
                al.UserId = User.Identity.GetUserId();

                if (ModelState.IsValid)
                {
                    db.Albums.Add(al);
                    db.SaveChanges();
                    TempData["message"] = "Album successfully created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(al);
                }
            }
            catch (Exception e)
            {
                return View(al);
            }
        }

        public ActionResult Edit(int id)
        {
            Album al = db.Albums.Find(id);
            return View(al);
        }

        [HttpPut]
        public ActionResult Edit(int id, Album requestAlbum)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Album al = db.Albums.Find(id);
                    if (TryUpdateModel(al))
                    {
                        al.AlbumTitle = requestAlbum.AlbumTitle;
                        TempData["message"] = "Album title has been changed!";
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestAlbum);
                }
            }
            catch (Exception e)
            {
                return View(requestAlbum);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Album al = db.Albums.Find(id);
            db.Albums.Remove(al);
            TempData["message"] = "Album was successfully deleted!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}