using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexus.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        // GET: Groups
        public ActionResult Index()
        {
            return View();
        }
    }
}