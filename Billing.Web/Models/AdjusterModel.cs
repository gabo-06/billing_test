using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class AdjusterModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();


        public List<Adjuster> Listar()
        {
            List<Adjuster> Lista = new List<Adjuster>();

            var Ajustadores = db.SP_LIST_ADJUSTER();

            foreach (var LectorAjustador in Ajustadores)
            {
                Adjuster ObjAjustador = new Adjuster();                
                ObjAjustador.Adj_code = LectorAjustador.Adj_code ;
                ObjAjustador.Adj_firstName = (LectorAjustador.Adj_firstName == null) ? "" : LectorAjustador.Adj_firstName ; 
                ObjAjustador.Adj_lastName = (LectorAjustador.Adj_lastName == null) ? "" : LectorAjustador.Adj_lastName; 
                ObjAjustador.Adj_phone = (LectorAjustador.Adj_phone == null) ? "" : LectorAjustador.Adj_phone;
                ObjAjustador.Adj_phoneExt = (LectorAjustador.Adj_phoneExt == null) ? "" : LectorAjustador.Adj_phoneExt; 

                Lista.Add(ObjAjustador);
            }

            return Lista;
        }

        public List<Adjuster> ListarParaTrazabilidad()
        {
            List<Adjuster> Lista = new List<Adjuster>();

            var Ajustadores = db.SP_LIST_ADJUSTER_FOR_TRACEABILITY();

            foreach (var LectorAjustador in Ajustadores)
            {
                Adjuster ObjAjustador = new Adjuster();
                ObjAjustador.Adj_code = LectorAjustador.Adj_code;
                ObjAjustador.Adj_firstName = (LectorAjustador.Adj_firstName == null) ? "" : LectorAjustador.Adj_firstName;
                ObjAjustador.Adj_lastName = (LectorAjustador.Adj_lastName == null) ? "" : LectorAjustador.Adj_lastName;
                ObjAjustador.Adj_phone = (LectorAjustador.Adj_phone == null) ? "" : LectorAjustador.Adj_phone;
                ObjAjustador.Adj_phoneExt = (LectorAjustador.Adj_phoneExt == null) ? "" : LectorAjustador.Adj_phoneExt;

                Lista.Add(ObjAjustador);
            }

            return Lista;
        }

        public List<SP_LIST_ADJUSTER_FOR_FIND_MATCHES_Result> ListarParaAutocompletar()
        {
            List<SP_LIST_ADJUSTER_FOR_FIND_MATCHES_Result> Resultado = new List<SP_LIST_ADJUSTER_FOR_FIND_MATCHES_Result>();
            ObjectResult<SP_LIST_ADJUSTER_FOR_FIND_MATCHES_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_ADJUSTER_FOR_FIND_MATCHES();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_ADJUSTER_FOR_FIND_MATCHES_Result NodoResultado = new SP_LIST_ADJUSTER_FOR_FIND_MATCHES_Result();

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

        public List<SP_SAVE_ADJUSTER_Result> Insertar(Adjuster Ajustador)
        {
            List<SP_SAVE_ADJUSTER_Result> Resultado = new List<SP_SAVE_ADJUSTER_Result>();
            ObjectResult<SP_SAVE_ADJUSTER_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_ADJUSTER(  Ajustador.Adj_firstName
                                                                           , Ajustador.Adj_lastName
                                                                           , Ajustador.Adj_phone
                                                                           , Ajustador.Adj_phoneExt
                                                                           , Ajustador.Adj_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_ADJUSTER_Result NodoResultado = new SP_SAVE_ADJUSTER_Result();

                    NodoResultado.AdjusterErrorCode = RPAD.AdjusterErrorCode;
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

        public Adjuster Buscar(int Codigo)
        {
            Adjuster ObjAjustador = new Adjuster();

            try
            {
                var Resultado = db.SP_FIND_ADJUSTER(Codigo);

                foreach (var NodoResultado in Resultado)
                {
                    ObjAjustador.Adj_code = NodoResultado.Adj_code;
                    ObjAjustador.Adj_firstName = NodoResultado.Adj_firstName;
                    ObjAjustador.Adj_lastName = NodoResultado.Adj_lastName;
                    ObjAjustador.Adj_phone = NodoResultado.Adj_phone;
                    ObjAjustador.Adj_phoneExt = NodoResultado.Adj_phoneExt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjAjustador;
        }

        public List<SP_UPDATE_ADJUSTER_Result> Actualizar(Adjuster Ajustador)
        {
            List<SP_UPDATE_ADJUSTER_Result> Resultado = new List<SP_UPDATE_ADJUSTER_Result>();
            ObjectResult<SP_UPDATE_ADJUSTER_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_ADJUSTER( Ajustador.Adj_code
                                                                             ,Ajustador.Adj_firstName
                                                                             ,Ajustador.Adj_lastName
                                                                             ,Ajustador.Adj_phone
                                                                             ,Ajustador.Adj_phoneExt
                                                                             ,Ajustador.Adj_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_ADJUSTER_Result NodoResultado = new SP_UPDATE_ADJUSTER_Result();

                    NodoResultado.AdjusterErrorCode = RPAD.AdjusterErrorCode;
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

        public List<SP_DELETE_ADJUSTER_Result> Eliminar(int Codigo = 0)
        {
            List<SP_DELETE_ADJUSTER_Result> Resultado = new List<SP_DELETE_ADJUSTER_Result>();
            ObjectResult<SP_DELETE_ADJUSTER_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_DELETE_ADJUSTER(Codigo);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_DELETE_ADJUSTER_Result NodoResultado = new SP_DELETE_ADJUSTER_Result();

                    NodoResultado.AdjusterErrorCode = RPAD.AdjusterErrorCode;
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

        public List<SP_FIND_HOMONYM_ADJUSTERS_Result> BuscarHomonimos(Adjuster Ajustador)
        {
            List<SP_FIND_HOMONYM_ADJUSTERS_Result> Resultado = new List<SP_FIND_HOMONYM_ADJUSTERS_Result>();
            ObjectResult<SP_FIND_HOMONYM_ADJUSTERS_Result> ResultadoProcedimientoAlmacenadoDuro;
            ObjectParameter ParametroSalida = new ObjectParameter("ParametroSalida", typeof(bool)); // Variable que se crea para recibir el valor del parámetro de salida del procedimiento almacenado. Si encontró coincidencias las devuelve

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_FIND_HOMONYM_ADJUSTERS(ParametroSalida
                                                                                   , Ajustador.Adj_code
                                                                                   , Ajustador.Adj_firstName
                                                                                   , Ajustador.Adj_lastName);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_FIND_HOMONYM_ADJUSTERS_Result NodoResultado = new SP_FIND_HOMONYM_ADJUSTERS_Result();

                    NodoResultado.Adj_code = RPAD.Adj_code;
                    NodoResultado.Adj_firstName = RPAD.Adj_firstName;
                    NodoResultado.Adj_lastName = RPAD.Adj_lastName;
                    NodoResultado.Adj_phone = RPAD.Adj_phone;
                    NodoResultado.Adj_phoneExt = RPAD.Adj_phoneExt;
                    NodoResultado.Adj_status = RPAD.Adj_status;

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