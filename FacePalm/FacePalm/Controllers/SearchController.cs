using FacePalm.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search
        public ActionResult Search(string searchText)
        {
            var currentUserId = User.Identity.GetUserId();
            ViewBag.CurrentUser = currentUserId;
            ViewBag.SearchText = searchText;
            List<Search> results = new List<Search>();
            IEnumerable<User> users = db.Users.Where(u => string.Concat(u.FirstName," ", u.LastName).ToLower().Contains(searchText.ToLower())).ToList();
            foreach(User user in users)
            {
                Search searchResult = new Search();
                searchResult.user = user;
                results.Add(searchResult);
            }
            return View(results);
        }
    }
}