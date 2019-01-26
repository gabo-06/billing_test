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
    public class DataAuditController : Controller
    {
        private DataAudit model = new DataAudit();        

        static List<PPatient> PacientesGlobalEstatica = new List<PPatient>();
        static List<PPatient> PacientesTemporal = new List<PPatient>();

        public PartialViewResult Index()
        {
            return PartialView();
        }

        public PartialViewResult listaPaciente()
        {
            return PartialView("listaPaciente");
        }

        public PartialViewResult trazabilidadPaciente(int codigoPaciente = 0)
        {
            try
            {
                var trazabilidadPaciente = model.trazabilidadPaciente(codigoPaciente);
                return PartialView("trazabilidadPaciente", trazabilidadPaciente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult trazabilidadMedico(int codigoMedico = 0)
        {
            try
            {
                var trazabilidadMedico = model.trazabilidadMedico(codigoMedico);
                return PartialView("trazabilidadMedico", trazabilidadMedico);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult trazabilidadAseguradora(int codigoAseguradora = 0)
        {
            try
            {
                var trazabilidadAseguradora = model.trazabilidadAseguradora(codigoAseguradora);
                return PartialView("trazabilidadAseguradora", trazabilidadAseguradora);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult trazabilidadAbogado(int codigoAbogado = 0)
        {
            try
            {
                var trazabilidadAbogado = model.trazabilidadAbogado(codigoAbogado);
                return PartialView("trazabilidadAbogado", trazabilidadAbogado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult trazabilidadAjustador(int codigoAjustador= 0)
        {
            try
            {
                var trazabilidadAjustador = model.trazabilidadAjustador(codigoAjustador);
                return PartialView("trazabilidadAjustador", trazabilidadAjustador);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult trazabilidadProveedor(int codigoProveedor = 0)
        {
            try
            {
                var trazabilidadProveedor = model.trazabilidadProveedor(codigoProveedor);
                return PartialView("trazabilidadProveedor", trazabilidadProveedor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult trazabilidadActividad(int codigoActividad = 0)
        {
            try
            {
                var trazabilidadActividad = model.trazabilidadActividad(codigoActividad);
                return PartialView("trazabilidadActividad", trazabilidadActividad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult trazabilidadEspecialidad(int codigoEspecialidad= 0)
        {
            try
            {
                var trazabilidadEspecialidad = model.trazabilidadEspecialidad(codigoEspecialidad);
                return PartialView("trazabilidadEspecialidad", trazabilidadEspecialidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public PartialViewResult listaDoctor()
        {
            return PartialView("listaDoctor");
        }

        public PartialViewResult listaInsurer()
        {
            return PartialView("listaInsurer");
        }

        public PartialViewResult listaAttorney()
        {
            return PartialView("listaAttorney");
        }

        public PartialViewResult listaAdjusters()
        {
            return PartialView("listaAdjusters");
        }

        public PartialViewResult listaProviders()
        {
            return PartialView("listaProviders");
        }

        public PartialViewResult listaActivities()
        {
            return PartialView("listaActivities");
        }

        public PartialViewResult listaSpeciality()
        {
            return PartialView("listaSpeciality");
        }

        public PartialViewResult listaCase()
        {
            return PartialView("listaCase");
        }

        public PartialViewResult listaDataEntry()
        {
            return PartialView("listaData");
        }

        public PartialViewResult listaMonthly()
        {
            return PartialView("listaMonthly");
        }

        public PartialViewResult listaBilling()
        {
            return PartialView("listaBilling");
        }
    }
}
