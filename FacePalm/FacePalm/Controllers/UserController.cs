using FacePalm.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private ApplicationDbContext _applicationDBContext = new ApplicationDbContext();
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var users = from user in _applicationDBContext.Users
                        orderby user.FirstName
                        select user;
            ViewBag.Users = users;
            return View();
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Show(string id)
        {
            User user = _applicationDBContext.Users.Find(id);
            var posts = _applicationDBContext.Posts.Where(p => p.UserId == id);
            var albums = _applicationDBContext.Albums.Where(a => a.UserId == id);
            var currentUserId = User.Identity.GetUserId();
            ViewBag.User = user;
            ViewBag.CurrentUser = currentUserId;
            ViewBag.Posts = posts.OrderByDescending(x => x.Date);
            ViewBag.Albums = albums;
            var friendshipStatus = _applicationDBContext.Friendship.Any(f => (f.FirstUserId == currentUserId && f.SecondUserId == id) || (f.FirstUserId == id && f.SecondUserId == currentUserId));
            if (friendshipStatus)
            {
                ViewBag.IsFriend = true;
            }
            else
            {
                ViewBag.IsFriend = false;
            }
            var anyFriendRequest = _applicationDBContext.FriendRequests.Any(r => r.FromUserId == id && r.ToUserId == currentUserId);
            if (anyFriendRequest)
            {
                var friendReqFromUserToMe = _applicationDBContext.FriendRequests.Where(r => r.FromUserId == id && r.ToUserId == currentUserId);
                if (friendReqFromUserToMe != null)
                {
                    ViewBag.FriendRequestStatus = true;
                }
            }
            else
            {
                ViewBag.FriendRequestStatus = false;
            }

            return View();
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Edit(string id)
        {
            User user = _applicationDBContext.Users.Find(id);
            ViewBag.User = user;
            if (user.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {

                return View(user);
            }
            else
            {
                TempData["message"] = "You do not have the rights to modify!";
                ViewBag.message = TempData["message"].ToString();

                return View("Error");
            }
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        [HttpPut]
        public ActionResult Edit(string id, User requestUser)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    User user = _applicationDBContext.Users.Find(id);
                    ViewBag.User = user;
                    if (user.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                    {
                        
                        if (TryUpdateModel(user))
                        {
                            if(requestUser.ImageFile!=null)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(requestUser.ImageFile.FileName);
                                string extension = Path.GetExtension(requestUser.ImageFile.FileName);
                                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                                user.ImagePath = "~/Images/" + fileName;
                                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                                user.ImageFile.SaveAs(fileName);
                            }
                            if (!String.IsNullOrEmpty(requestUser.FirstName))
                            {
                                user.FirstName = requestUser.FirstName;
                            }
                            if (!String.IsNullOrEmpty(requestUser.LastName))
                            {
                                user.LastName = requestUser.LastName;
                            }
                            if (!String.IsNullOrEmpty(requestUser.BirthDate.ToString()))
                            {
                                user.BirthDate = requestUser.BirthDate;
                            }
                            if (!String.IsNullOrEmpty(requestUser.Education))
                            {
                                user.Education = requestUser.Education;
                            }
                            if (!String.IsNullOrEmpty(requestUser.Job))
                            {
                                user.Job = requestUser.Job;
                            }
                            user.ProfilePrivacy = requestUser.ProfilePrivacy;
                            user.RelationshipStatus = requestUser.RelationshipStatus;
                            user.Gender = requestUser.Gender;
                            //user.ProfilePicture = requestUser.ProfilePicture;
                            _applicationDBContext.SaveChanges();
                        }
                        return RedirectToAction("Show/" + user.UserId);
                    }
                    else
                    {
                        TempData["message"] = "You do not have the rights to modify!";
                        ViewBag.message = TempData["message"].ToString();

                        return View("Error");
                    }
                }
                else
                {
                    TempData["message"] = "Something went wrong...";
                    ViewBag.message = TempData["message"].ToString();
                    return View("Error");
                }

            }
            catch (Exception ex)
            {
                TempData["message"] = "Something went wrong..." + ex.ToString();
                ViewBag.message = TempData["message"].ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            User user = _applicationDBContext.Users.Find(id);
            if (user.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                /*
                var albums = _applicationDBContext.Albums.Select(a => a.UserId == user.UserId);
                _applicationDBContext.Albums.Remove(albums);
                Comment comments = _applicationDBContext.Comments.FirstOrDefault(c => c.UserId == user.UserId);
                _applicationDBContext.Comments.Remove(comments);
                _applicationDBContext.Groups.Where(g => g.UsersIds.Remove(user.UserId));
                Conversation
                _applicationDBContext.Conversations.Remove
                */

                if (id == null)
                {
                    return View("Error");
                }

                _applicationDBContext.Users.Remove(user);
                _applicationDBContext.SaveChanges();
                TempData["message"] = "The user has been successfully removed!";
                ViewBag.message = TempData["message"].ToString();
                /*
                var _user = await UserManager.FindByIdAsync(id);

                //List Logins associated with user
                var logins = _user.Logins;

                //Gets list of Roles associated with current user
                var rolesForUser = await UserManager.GetRolesAsync(id);

                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await UserManager.RemoveFromRoleAsync(_user.Id, item);
                        }
                    }

                    //Delete User
                    await UserManager.DeleteAsync(user);
                }
                */
                return View("../Home/Index");
            }
            else
            {
                TempData["message"] = "You do not have permission to delete this profile!";
                ViewBag.message = TempData["message"].ToString();
                return View("Error");
            }
        }

        public ActionResult Friends(string id)
        {
            try
            {
                var user = _applicationDBContext.Users.Find(id);
                ViewBag.Friends = user.Friends;
                return View();
            }
            catch (Exception ex)
            {
                TempData["message"] = "Something went wrong..." + ex.ToString();
                ViewBag.message = TempData["message"].ToString();
                return View("Error");
            }
        }




        [HttpPost]
        public ActionResult AddFriend(FriendRequest friendRequestData)
        {
            if (friendRequestData != null)
            {
                _applicationDBContext.FriendRequests.Add(friendRequestData);
                _applicationDBContext.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public ActionResult GetFriendRequests()
        {
            try
            {

                var currentUserId = User.Identity.GetUserId();
                ViewBag.CurrentUser = currentUserId;
                var anyfriendRequest = _applicationDBContext.FriendRequests.Any(f => f.ToUserId == currentUserId);
                if (anyfriendRequest)
                {
                    var fr = _applicationDBContext.FriendRequests.Where(f => f.ToUserId == currentUserId);
                    var friendsToBe = _applicationDBContext.Users.Where(u => fr.Select(f => f.FromUserId).Contains(u.UserId));
                    ViewBag.FriendRequests = friendsToBe;
                    return View("FriendRequests");
                }
                else
                {
                    ViewBag.FriendRequests = null;
                    return View("FriendRequests");
                }

            }
            catch (Exception ex)
            {
                TempData["message"] = "Something went wrong..." + ex.ToString();
                ViewBag.message = TempData["message"].ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult AcceptFriendRequest(FriendRequest friendRequestData)
        {
            try
            {
                if (friendRequestData != null)
                {
                    var userToAdd = _applicationDBContext.Users.Find(friendRequestData.FromUserId);
                    var currentUser = _applicationDBContext.Users.Find(friendRequestData.ToUserId);
                    Friendship friendship = new Friendship();
                    friendship.FirstUserId = friendRequestData.FromUserId;
                    friendship.SecondUserId = friendRequestData.ToUserId;
                    _applicationDBContext.Friendship.Add(friendship);
                    userToAdd.Friends.Add(currentUser);
                    currentUser.Friends.Add(userToAdd);
                    //userToAdd.Friendships.Add(friendship);
                    //currentUser.Friendships.Add(friendship);
                    _applicationDBContext.Entry(friendRequestData).State = EntityState.Deleted;
                    //_applicationDBContext.FriendRequests.Remove(friendRequestData);
                    _applicationDBContext.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }

        }

        [HttpPost]
        public ActionResult DeleteFriend(string friendId)
        {
            try
            {
                var user = _applicationDBContext.Users.Find(friendId);
                var currentUserId = User.Identity.GetUserId();
                var currentUser = _applicationDBContext.Users.Find(currentUserId);
                currentUser.Friends.Remove(user);
                user.Friends.Remove(currentUser);
                var friendship = _applicationDBContext.Friendship.Find(friendId, currentUserId);
                if (friendship == null)
                {
                    friendship = _applicationDBContext.Friendship.Find(currentUserId, friendId);
                }
                _applicationDBContext.Entry(friendship).State = EntityState.Deleted;
                _applicationDBContext.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

    }
}
