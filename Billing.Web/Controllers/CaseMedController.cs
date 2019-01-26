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

namespace Billing.Web.Controllers 
{
    public class CaseMedController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public CaseMedModel model = new CaseMedModel();
        
        // ********************************************************************************
        
        public JsonResult ObtenerDoctoresCaso(int CodigoCaso = 0)
        { 
            List<SP_LIST_CASE_MEDICAL_Result> DoctoresCaso;

            try
            {
                DoctoresCaso = model.ObtenerDoctoresCaso(CodigoCaso);
                return Json(DoctoresCaso, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult AsignarDoctorACaso(int CodigoCaso = 0, int CodigoDoctor = 0, string ProcedenciaDoctor = "", int CodigoUsuario = 0)
        {
            SP_ASSIGN_MEDICAL_TO_CASE_Result Resultado;

            try
            {
                Resultado = model.AsignarDoctorACaso(CodigoCaso
                                                    , CodigoDoctor
                                                    , ProcedenciaDoctor
                                                    , CodigoUsuario);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult EliminarDoctorDeCaso(int CodigoCaso = 0, int CodigoDoctor = 0)
        {
            SP_DELETE_MEDICAL_FROM_CASE_Result Resultado;

            try
            {
                Resultado = model.EliminarDoctorDeCaso(CodigoCaso
                                                    , CodigoDoctor);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
