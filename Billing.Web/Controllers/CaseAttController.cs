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
    public class CaseAttController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public CaseAttModel model = new CaseAttModel();

        // ********************************************************************************

        public JsonResult ObtenerAbogadosCaso(int CodigoCaso = 0)
        {
            List<SP_LIST_CASE_ATTORNEY_Result> AbogadosCaso;

            try
            {
                AbogadosCaso = model.ObtenerAbogadosCaso(CodigoCaso);
                return Json(AbogadosCaso, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult AsignarAbogadoACaso(int CodigoCaso = 0, int CodigoAbogado = 0)
        {
            SP_ASSIGN_ATTORNEY_TO_CASE_Result Resultado;

            try
            {
                Resultado = model.AsignarAbogadoACaso(CodigoCaso, CodigoAbogado);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult EliminarAbogadoDeCaso(int CodigoCaso = 0, int CodigoAbogado = 0)
        {
            SP_DELETE_ATTORNEY_FROM_CASE_Result Resultado;

            try
            {
                Resultado = model.EliminarAbogadoDeCaso(CodigoCaso, CodigoAbogado);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
