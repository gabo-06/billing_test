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
    public class SpecialityController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        public PatientModel model = new PatientModel();

        //
        // GET: /Patient/

        public PartialViewResult Index()
        {
            //var Pacientes = model.ListaPacientes();            
            return PartialView();            
        }

        //
        // GET: /Patient/Details/5

        public ActionResult Details(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        //
        // GET: /Patient/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Patient/Create

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

        //
        // GET: /Patient/Edit/5
        
        [HttpPost]
        [ActionName("ObtenerDatosParaEditar")]
        public JsonResult Edit(int CodigoPaciente = 0)
        {
            PPatient ObjPaciente = null;

            try
            {
                ObjPaciente = model.Buscar(CodigoPaciente);
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

        //
        // POST: /Patient/Edit/5

        [HttpPost]
        [ActionName("ActualizarDatos")]
        public JsonResult Edit(Patient Paciente)
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

        //
        // GET: /Patient/Delete/5
        [HttpPost]
        public JsonResult Delete(int CodigoPaciente = 0)
        {
            List<SP_DELETE_PATIENT_Result> Resultado;

            try
            {
                Resultado = model.Eliminar(CodigoPaciente);
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

        //
        // POST: /Patient/Delete/5
        /*
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}