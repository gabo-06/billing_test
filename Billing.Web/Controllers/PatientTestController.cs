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
    public class PatientTestController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public PatientModel model = new PatientModel();

        public PartialViewResult Index()
        {
            try
            {
                var Pacientes = model.Listar();
            return PartialView(Pacientes);            
        }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListaPacientesParaAutocompletar()
        {
            List<SP_LIST_PATIENT_FOR_FIND_MATCHES_Result> Pacientes;

            try
            {
                Pacientes = model.ListarParaAutocompletar();
                return Json(Pacientes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(Patient Paciente)
        {
            List<SP_SAVE_PATIENT_Result> Resultado;

            try
            {
                Resultado = model.Insertar(Paciente);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // return Redirect("~/Patient/index");
        }
        
        [HttpPost]
        public JsonResult Buscar(int Codigo = 0)
        {
            PPatient ObjPaciente = null;

            try
            {
                ObjPaciente = model.Buscar(Codigo);
                return Json(ObjPaciente, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            // Patient patient = db.Patients.Find(id);
            // if (patient == null)
            // {
            //     return HttpNotFound();
            // }
            // return View(patient);
        }

        [HttpPost]
        public JsonResult Actualizar(Patient Paciente)
        {
            List<SP_UPDATE_PATIENT_Result> Resultado;

            try
            {
                Resultado = model.Actualizar(Paciente);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // if (ModelState.IsValid)
            // {
            //     db.Entry(patient).State = EntityState.Modified;
            //     db.SaveChanges();
            //     return RedirectToAction("Index");
            // }
            // return View(patient);
        }

        [HttpPost]
        public JsonResult Delete(int Codigo = 0)
        {
            List<SP_DELETE_PATIENT_Result> Resultado;

            try
            {
                Resultado = model.Eliminar(Codigo);
                return Json(Resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Patient patient = db.Patients.Find(id);
            // if (patient == null)
            // {
            //     return HttpNotFound();
            // }
            // return View(patient);
        }

        //[HttpPost]
        //public JsonResult BuscarHomonimos(Patient Paciente)
        //{
        //    int CantidadHomonimos = 0;

        //    try
        //    {
        //        CantidadHomonimos = model.BuscarHomonimos(Paciente);
        //        return Json(CantidadHomonimos, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}