using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class ProviderModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        // ****************************************************************************************************************

        public List<Provider> Listar()
        {
            List<Provider> Lista = new List<Provider>();

            var Proveedores = db.SP_LIST_PROVIDER();

            foreach (var LectorProveedor in Proveedores)
            {
                Provider ObjProveedor = new Provider();

                
                ObjProveedor.Pro_code = LectorProveedor.Pro_code;
                ObjProveedor.Pro_firstName = (LectorProveedor.Pro_firstName == null) ? "" : LectorProveedor.Pro_firstName.ToUpper();
                ObjProveedor.Pro_lastName = (LectorProveedor.Pro_lastName == null) ? "" : LectorProveedor.Pro_lastName.ToUpper();
                ObjProveedor.Pro_number = (LectorProveedor.Pro_number == null) ? "" : LectorProveedor.Pro_number.ToUpper();

                Lista.Add(ObjProveedor);
            }

            return Lista;
        }


        public List<Provider> ListarParaTrazabilidad()
        {
            List<Provider> Lista = new List<Provider>();

            var Proveedores = db.SP_LIST_PROVIDER_FOR_TRACEABILITY();

            foreach (var LectorProveedor in Proveedores)
            {
                Provider ObjProveedor = new Provider();


                ObjProveedor.Pro_code = LectorProveedor.Pro_code;
                ObjProveedor.Pro_firstName = (LectorProveedor.Pro_firstName == null) ? "" : LectorProveedor.Pro_firstName.ToUpper();
                ObjProveedor.Pro_lastName = (LectorProveedor.Pro_lastName == null) ? "" : LectorProveedor.Pro_lastName.ToUpper();
                ObjProveedor.Pro_number = (LectorProveedor.Pro_number == null) ? "" : LectorProveedor.Pro_number.ToUpper();

                Lista.Add(ObjProveedor);
            }

            return Lista;
        }

        public List<SP_LIST_PROVIDER_FOR_FIND_MATCHES_Result> ListarParaAutocompletar()
        {
            List<SP_LIST_PROVIDER_FOR_FIND_MATCHES_Result> Resultado = new List<SP_LIST_PROVIDER_FOR_FIND_MATCHES_Result>();
            ObjectResult<SP_LIST_PROVIDER_FOR_FIND_MATCHES_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_PROVIDER_FOR_FIND_MATCHES();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_PROVIDER_FOR_FIND_MATCHES_Result NodoResultado = new SP_LIST_PROVIDER_FOR_FIND_MATCHES_Result();

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



        public List<SP_SAVE_PROVIDER_Result> Insertar(Provider Proveedor)
        {
            List<SP_SAVE_PROVIDER_Result> Resultado = new List<SP_SAVE_PROVIDER_Result>();
            ObjectResult<SP_SAVE_PROVIDER_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_PROVIDER( Proveedor.Pro_firstName
                                                                           ,Proveedor.Pro_lastName
                                                                           , Proveedor.Pro_number
                                                                           ,Proveedor.Pro_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_PROVIDER_Result NodoResultado = new SP_SAVE_PROVIDER_Result();

                    NodoResultado.ProviderErrorCode = RPAD.ProviderErrorCode;
                    NodoResultado.ErrorMessage = RPAD.ErrorMessage.ToString();

                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public Provider Buscar(int Codigo)
        {
            Provider ObjProveedor = new Provider();

            try
            {
                var Resultado = db.SP_FIND_PROVIDER(Codigo);

                foreach (var NodoResultado in Resultado)
                {
                    ObjProveedor.Pro_code = NodoResultado.Pro_code;
                    ObjProveedor.Pro_firstName = NodoResultado.Pro_firstName;
                    ObjProveedor.Pro_lastName = NodoResultado.Pro_lastName;
                    ObjProveedor.Pro_number = NodoResultado.Pro_number;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjProveedor;
        }

        public List<SP_UPDATE_PROVIDER_Result> Actualizar(Provider Proveedor)
        {
            List<SP_UPDATE_PROVIDER_Result> Resultado = new List<SP_UPDATE_PROVIDER_Result>();
            ObjectResult<SP_UPDATE_PROVIDER_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_PROVIDER(Proveedor.Pro_code
                                                                            ,Proveedor.Pro_firstName
                                                                            ,Proveedor.Pro_lastName
                                                                            ,Proveedor.Pro_number
                                                                            ,Proveedor.Pro_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_PROVIDER_Result NodoResultado = new SP_UPDATE_PROVIDER_Result();

                    NodoResultado.ProviderErrorCode = RPAD.ProviderErrorCode;
                    NodoResultado.ErrorMessage = RPAD.ErrorMessage.ToString();

                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public List<SP_DELETE_PROVIDER_Result> Eliminar(int Codigo = 0)
        {
            List<SP_DELETE_PROVIDER_Result> Resultado = new List<SP_DELETE_PROVIDER_Result>();
            ObjectResult<SP_DELETE_PROVIDER_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_DELETE_PROVIDER(Codigo);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_DELETE_PROVIDER_Result NodoResultado = new SP_DELETE_PROVIDER_Result();

                    NodoResultado.ProviderErrorCode = RPAD.ProviderErrorCode;
                    NodoResultado.ErrorMessage = RPAD.ErrorMessage.ToString();

                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public List<SP_FIND_HOMONYM_PROVIDERS_Result> BuscarHomonimos(Provider Proveedor)
        {
            List<SP_FIND_HOMONYM_PROVIDERS_Result> Resultado = new List<SP_FIND_HOMONYM_PROVIDERS_Result>();
            ObjectResult<SP_FIND_HOMONYM_PROVIDERS_Result> ResultadoProcedimientoAlmacenadoDuro;
            ObjectParameter ParametroSalida = new ObjectParameter("ParametroSalida", typeof(bool)); // Variable que se crea para recibir el valor del parámetro de salida del procedimiento almacenado. Si encontró coincidencias las devuelve

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_FIND_HOMONYM_PROVIDERS(ParametroSalida
                                                                                   , Proveedor.Pro_code
                                                                                   , Proveedor.Pro_firstName
                                                                                   , Proveedor.Pro_lastName);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_FIND_HOMONYM_PROVIDERS_Result NodoResultado = new SP_FIND_HOMONYM_PROVIDERS_Result();

                    NodoResultado.Pro_code = RPAD.Pro_code;
                    NodoResultado.Pro_firstName = RPAD.Pro_firstName;
                    NodoResultado.Pro_lastName = RPAD.Pro_lastName;
                    NodoResultado.Pro_number = RPAD.Pro_number;
                    NodoResultado.Pro_status = RPAD.Pro_status;

                    Resultado.Add(RPAD);
                }

                if ((bool)ParametroSalida.Value)
                    return Resultado;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}