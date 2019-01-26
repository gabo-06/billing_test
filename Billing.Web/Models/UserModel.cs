using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{ 
    public class UserModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public PUser ObtenerUsuario(String login = "")
        {
            PUser ObjUsuario = new PUser();

            try
            {
                ObjectResult<SP_GET_USER_Result> resultado = db.SP_GET_USER(login);
                // string usuarioDato = "";
                 foreach (var NodoResultado in resultado)
                { 
                    ObjUsuario.Use_code = NodoResultado.Use_code;
                    ObjUsuario.Use_firstName = NodoResultado.Use_firstName;
                    ObjUsuario.Use_lastName = NodoResultado.Use_lastName;
                    ObjUsuario.Use_rolName = NodoResultado.Rol;
                    ObjUsuario.BillManager = NodoResultado.BillManager;

                    // usuarioDato = ObjUsuario.Use_firstName + " " + ObjUsuario.Use_lastName;
                }
                 return ObjUsuario;
             }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public List<SP_LIST_SUPERVISOR_FOR_FIND_MATCHES_Result> ListarSupervisoresParaAutocompletar()
        {
            List<SP_LIST_SUPERVISOR_FOR_FIND_MATCHES_Result> Resultado = new List<SP_LIST_SUPERVISOR_FOR_FIND_MATCHES_Result>();
            ObjectResult<SP_LIST_SUPERVISOR_FOR_FIND_MATCHES_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_SUPERVISOR_FOR_FIND_MATCHES();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_SUPERVISOR_FOR_FIND_MATCHES_Result NodoResultado = new SP_LIST_SUPERVISOR_FOR_FIND_MATCHES_Result();

                    NodoResultado.Codigo = RPAD.Codigo;
                    NodoResultado.NombreCompleto = RPAD.NombreCompleto;


                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public int ObtenerUsuarioPermiso(String login = "")
        {
            PUser ObjUsuario = new PUser();
            //string TipoUsuario = "";
            string sw = "0";
            ObjectResult Resultado = (ObjectResult)null;
            int Respuesta = 0;

            try
            {
                Resultado = db.SP_GET_TYPE_USER(login);//administrador retorna 1, else 0 

                foreach (var NodoObjectResult in Resultado)
                {
                   // if (NodoObjectResult.ToString() != "" && NodoObjectResult.ToString() != null)
                        //sw = "1";

                    sw = NodoObjectResult.ToString();
                }

                if (sw == "0")//digit u otros
                    Respuesta = 0;
                else // administrador
                    Respuesta = 1;

                    return Respuesta;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}