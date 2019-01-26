using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Mapping;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;
using Billing.Utils;
using System.Threading.Tasks;

namespace Billing.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            // return View();
            return Redirect("~/Account/Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
