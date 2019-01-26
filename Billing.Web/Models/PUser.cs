using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class PUser
    {
        public int Use_code { get; set; }
        public string Use_firstName { get; set; }
        public string Use_lastName { get; set; }               
        public string Use_rolName { get; set; }
        public int BillManager { get; set; }
    }
} 