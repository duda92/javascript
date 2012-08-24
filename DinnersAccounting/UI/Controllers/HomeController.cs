using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult GetMyInfo()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult AdminPart()
        {
            return View();
        }
    }
}
