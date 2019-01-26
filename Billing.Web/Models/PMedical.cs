using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Web.Models
{
    public class PMedical
    {
        public PMedical()
        {
            this.CaseInformationSheetBodyMeds = new HashSet<CaseInformationSheetBodyMed>();
            this.Confirmacions = new HashSet<Confirmacion>();
            this.Eventoes = new HashSet<Evento>();
        }
    
        public int Med_code { get; set; }
        public string Med_code_old { get; set; }
        public string Med_firstName { get; set; }
        public string Med_lastName { get; set; }
        public string Med_address { get; set; }
        public string Med_phone { get; set; }
        public string Med_fax { get; set; }
        public string Med_zipCode { get; set; }
        public string Med_city { get; set; }
        public string Med_state { get; set; }
        public string Sta_abbreviation { get; set; }
        public string Med_zipCodeExt { get; set; }
        public string Med_phoneExt { get; set; }
        public Nullable<bool> Med_status { get; set; }
        public int Spe_code { get; set; }
        public string Med_alternatePhone { get; set; }
        public string Med_office { get; set; }
    
        public virtual ICollection<CaseInformationSheetBodyMed> CaseInformationSheetBodyMeds { get; set; }
        public virtual ICollection<Confirmacion> Confirmacions { get; set; }
        public virtual ICollection<Evento> Eventoes { get; set; }
        public virtual Specialty Specialty { get; set; }
    }
}