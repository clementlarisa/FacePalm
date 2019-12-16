using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace FacePalm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
    }
}