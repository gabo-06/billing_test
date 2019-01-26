﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Billing.Web.Models
{
    using System;
    using System.Collections.Generic;

    public partial class PPatient
    {
        public PPatient()
        {
            this.CaseInformationSheetHeads = new HashSet<CaseInformationSheetHead>();
        }

        public int Pat_code { get; set; }
        public string Pat_code_old { get; set; }
        public string Pat_firstName { get; set; }
        public string Pat_lastName { get; set; }
        public string Pat_address { get; set; }
        public string Pat_phone { get; set; }
        public string Pat_fax { get; set; }
        public string Pat_zipCode { get; set; }
        public string Pat_city { get; set; }
        public string Pat_state { get; set; }
        public string Sta_abbreviation { get; set; }
        public string Pat_zipCodeExt { get; set; }
        public string Pat_phoneExt { get; set; }
        public Nullable<bool> Pat_status { get; set; }
        public byte[] Pat_socialSecurityNumber { get; set; }        
        public string Pat_birthday { get; set; }
        // public String Pat_birthday_cadena { get; set; }
        public string Pat_socialSecurityNumberD { get; set; }
        public string Pat_sex { get; set; }
        public Nullable<int> Sta_code { get; set; }
        public Nullable<int> Cit_code { get; set; }

        // public String FechaSistemaToString
        // {
        //     get
        //     {
        //         if (this.Pat_birthday != null)
        //             return this.Pat_birthday.Value.ToShortDateString();
        //         else
        //             return "";
        // 
        //     }
        // }


        public virtual ICollection<CaseInformationSheetHead> CaseInformationSheetHeads { get; set; }
    }
}
