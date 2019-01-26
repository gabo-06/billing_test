using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class PActivity
    {
        public int Act_code { get; set; }
        public string Act_code_old { get; set; }
        public string Act_description { get; set; }
        public int Aty_code { get; set; }
        public Nullable<decimal> Act_price { get; set; }

        public virtual ActivityType ActivityType { get; set; }
    }
}