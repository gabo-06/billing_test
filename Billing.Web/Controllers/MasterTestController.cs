using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;



namespace Billing.Web.Controllers
{
    public class MasterTestController : Controller
    {
        //
        // GET: /MasterViewer/

        public ActionResult Index()
        {
            MembershipUser user = Membership.GetUser(true);
            if (user == null)
            {
                return RedirectToAction("login", "Account");
            }
            ViewBag.test = "s";
            return View();

           

        }

    }
}
