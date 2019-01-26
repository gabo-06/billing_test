using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Billing.Web.Models;
using System.Diagnostics;
using System.Data.Objects;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;

using System.IO;
using System.Web.UI;

using Rotativa.Options;
using Rotativa;


namespace Billing.Web.Controllers
{
    public class ConfigurationController : Controller
    {
        private ConfigurationModel model = new ConfigurationModel();

        public PartialViewResult Index()
        {
            return PartialView();
        }


    }
}
