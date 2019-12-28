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
    }
}