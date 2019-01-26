using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class RoleModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<SP_LIST_ROLES_Result> ObtenerRoles()
        {
            List<SP_LIST_ROLES_Result> Lista = new List<SP_LIST_ROLES_Result>();
            ObjectResult<SP_LIST_ROLES_Result> Roles;

            try
            { 
                Roles = db.SP_LIST_ROLES();

                foreach (var NodoResultado in Roles)
                {
                    SP_LIST_ROLES_Result Rol = new SP_LIST_ROLES_Result();

                    Rol.RoleId = NodoResultado.RoleId;
                    Rol.RoleName = NodoResultado.RoleName;

                    Lista.Add(Rol);
                }

                return Lista;            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AsignarPermisoARol(string CodigoRol, int CodigoOpcion )
        {
            try
            {
                db.SP_MODIFY_ROLE_PERMISSION(CodigoRol, CodigoOpcion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> ObtenerPermisosDeRol(string CodigoRol)       
        {
            // Donde se obtiene el resultado del procedimiento almacenado.
            var ResultadoProcedimientoAlmacenado = (ObjectResult<int?>)null;
            
            // Donde se obtiene la lista de códigos de opciones (tabla "Option").
            List<int> ListaCodigosOpciones; 

            // Inicialización de variables.
            ListaCodigosOpciones = (List<int>)null;
        
            try
            {
                ResultadoProcedimientoAlmacenado = db.SP_GET_PERMITS_BY_ROLE(CodigoRol);
                ListaCodigosOpciones = ResultadoProcedimientoAlmacenado.Where(x => x.HasValue).Select(x => x.Value).ToList();

                return ListaCodigosOpciones;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
