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
    public class RoleController : Controller
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();
        private RoleModel model = new RoleModel();

        public JsonResult ObtenerRoles()
        {
            List<SP_LIST_ROLES_Result> Roles;

            try
            {
                Roles = model.ObtenerRoles();
                return Json(Roles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        // [HttpGet]
        // public void AsignarPermisoARol(string CodigoRol = (string)null, int CodigoOpcion = 0)
        public void AsignarPermisoARol(string CodigoRol = "", int CodigoOpcion = 0)
        {
            try
            {
                model.AsignarPermisoARol(CodigoRol, CodigoOpcion);                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // [HttpGet]
        [HttpPost]        
        public JsonResult ObtenerPermisosDeRol(string CodigoRol = "E891172F-61B1-42D8-9621-589C6E661A99")
        {
            List<int> ListaCodigosOpciones;

            try
            {
                ListaCodigosOpciones = model.ObtenerPermisosDeRol(CodigoRol);
                return Json(ListaCodigosOpciones, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
        
        // [HttpGet]
        public int prueba()
        {
            Object miObjecto1 = new { codigo = 1, nombre = "gabriel" };
            Object miObjecto2 = new { codigo = 2, nombre = "brenher" };
            Object miObjecto3 = new { codigo = 3, nombre = "ivon" };
            Object miObjecto4 = new { codigo = 4, nombre = "cristian" };
            Object miObjecto5 = new { codigo = 5, nombre = "manuel" };
            Object miObjecto6 = new { codigo = 6, nombre = "jorge" };
            Object miObjecto7 = new { codigo = 6, nombre = "karen" };
            Object miObjecto8 = new { codigo = 6, nombre = "leslie" };

            List<Object> miLista = new List<Object>();
            miLista.Add(miObjecto1);
            miLista.Add(miObjecto2);
            miLista.Add(miObjecto3);
            miLista.Add(miObjecto4);
            miLista.Add(miObjecto5);
            miLista.Add(miObjecto6);
            miLista.Add(miObjecto7);
            miLista.Add(miObjecto8);

            // var miConsulta = from elemento in miLista select elemento.nombre;
            // List<string> nombres = miConsulta.ite
            return 0;

            // string[] miArreglo = {"gabriel", "brenher", "ivon", "cristian",  "manuel", "jorge", "karen", "leslie"};
            // var miConsulta2 = from elemento in miArreglo select elemento.;
            // List<string> ListaCadenas = miConsulta2.ToList();
            
        }


    }
}
