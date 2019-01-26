using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Diagnostics;
using System.Data.Objects; 
using System.Web.Mvc;
using System.Net;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Entity;
using Billing.Web.Models;

namespace Billing.Web.Models
{
    public class DataEntryModel
    {
        private string CadenaConexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        private OmnimedBDEntities db = new OmnimedBDEntities();

        // ****************************************************************************************************************

        // Método que lista los casos para una búsqueda avanzada por pacientes.
        public List<CasoBusquedaAvanzada> ListaCasosParaBusquedaAvanzada()
        {
            List<CasoBusquedaAvanzada> Resultado = new List<CasoBusquedaAvanzada>();
            ObjectResult<SP_LIST_CASE_FOR_ADVANCED_SEARCH_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_CASE_FOR_ADVANCED_SEARCH();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    CasoBusquedaAvanzada NodoResultado = new CasoBusquedaAvanzada();

                    NodoResultado.CaseCode = RPAD.CaseCode;
                    NodoResultado.ClaimNumber = RPAD.ClaimNumber;
                    NodoResultado.PatientCode = RPAD.PatientCode;
                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.Patient2 = RPAD.FirsNamePatient;
                    NodoResultado.Patient3 = RPAD.LastNamePatient;
                    NodoResultado.Insurer = RPAD.Insurer;
                    DateTime fecha = new DateTime(RPAD.AccidentDate.Year
                                                , RPAD.AccidentDate.Month
                                                , RPAD.AccidentDate.Day
                                                , RPAD.AccidentDate.Hour
                                                , RPAD.AccidentDate.Minute
                                                , RPAD.AccidentDate.Second);
                    NodoResultado.AccidentDate = fecha.ToShortDateString();
                    NodoResultado.Status = RPAD.Status;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        // Buscar las entradas correspondientes a un caso.
        /*
        public List<PDataEntry_Lista> ListaEntradasDeCaso(int CodigoCaso = 0)
        {
            ObjectResult<SP_LIST_DATAENTRY_Result> Entradas;
            List<PDataEntry_Lista> Lista;

            Entradas = (ObjectResult<SP_LIST_DATAENTRY_Result>)null;
            Lista = new List<PDataEntry_Lista>();

            try 
            {
                Entradas = db.SP_LIST_DATAENTRY(CodigoCaso);                

                foreach (var LectorEntrada in Entradas)
                {
                    PDataEntry_Lista ObjEntrada = new PDataEntry_Lista();

                    ObjEntrada.Dae_code = int.Parse(LectorEntrada.Dae_code.ToString());
                    ObjEntrada.Dae_code_old = LectorEntrada.Dae_code_old;
                    // ObjEntrada.Dae_date = LectorEntrada.Dae_date;
                    ObjEntrada.Act_code = LectorEntrada.Act_code;
                    ObjEntrada.Act_code_old = LectorEntrada.Act_code_old;
                    ObjEntrada.Dae_hourAct = LectorEntrada.Dae_hourAct;
                    ObjEntrada.Dae_milesAct = LectorEntrada.Dae_milesAct;
                    ObjEntrada.Dae_comment = (LectorEntrada.Dae_comment);
                    ObjEntrada.Acumulado = int.Parse(LectorEntrada.Acumulado.ToString());
                    ObjEntrada.Row = LectorEntrada.Row;
                    ObjEntrada.Dae_facNum = LectorEntrada.Dae_facNum;
                    ObjEntrada.InicialesUsuario = LectorEntrada.InicialesUsuario;

                    Debug.WriteLine(ObjEntrada.Dae_comment);

                    Lista.Add(ObjEntrada);
                }

                return Lista;
            }
            catch (Exception ex) 
            {
                throw ex;
            }   
        }
        */

        // Actualiza las entradas de un caso seleccionado.
        public int[] RegistraActualizaEntradas(string EntradasXML)
        {
            ObjectResult<SP_SAVE_UPDATE_DATAENTRY_Result> ResultadoProcedimientoAlmacenadoDuro;
            int FilaErroneaActualiza;
            int FilaErroneaRegistra;
            //int TotalFilasAfectadas;
            
            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_SAVE_UPDATE_DATAENTRY_Result>)null;
            FilaErroneaActualiza = 0;
            FilaErroneaRegistra = 0;
            //TotalFilasAfectadas = 0;
            
            try
            {
                // return null;
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_UPDATE_DATAENTRY(EntradasXML);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    FilaErroneaActualiza = int.Parse(RPAD.FilaErroneaActualiza.ToString());
                    FilaErroneaRegistra = int.Parse(RPAD.FilaErroneaRegistra.ToString());
                }

                int[] array = new int[] {FilaErroneaActualiza, FilaErroneaRegistra};
                return array;
                // TotalFilasAfectadas = CantidadEntradasActualizadas + CantidadEntradasRegistradas;
                                      
                // return Json(new response = new { FilaActualizada = CantidadEntradasRegistradas, FilaRegistrada = CantidadEntradasRegistradas } );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Elimina una entrada de un caso seleccionado.
        public int EliminaEntrada(int CodigoEntrada, int CodigoUsuario)
        {
            // ObjectResult<SP_UPDATE_DATAENTRY_Result> EntradasActualizadas;
            // int EntradasActualizadas;

            // EntradasActualizadas = (ObjectResult<SP_UPDATE_DATAENTRY_Result>)null;
            // EntradasActualizadas = 0;

            try
            {
                db.SP_DELETE_DATAENTRY(CodigoEntrada, CodigoUsuario);

                // foreach (var Lector in EntradasActualizadas)
                // {
                //     SP_UPDATE_DATAENTRY_Result Objeto = new SP_UPDATE_DATAENTRY_Result();
                // 
                //     Objeto.Dae_code = Lector.Dae_code;
                //     Objeto.Dae_code_old = Lector.Dae_code_old;
                // 
                //     Debug.Flush();
                //     Debug.WriteLine(Objeto.Dae_code + " - " + Objeto.Dae_code_old);
                // }

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Método que lista entradas para convertir en factura
        public List<EntradaConvertidaBilling> ListaEntradasParaConvertirEnFactura(int CodigoCaso = 0
                                                                            , string FechaDeCierre = ""
                                                                            , string FacNum = "-")
        {
            ObjectResult<SP_CONVERT_ENTRIES_IN_INVOICE_Result> ResultadoProcedimientoAlmacenadoDuro;
            List<EntradaConvertidaBilling> Resultado = new List<EntradaConvertidaBilling>();

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_CONVERT_ENTRIES_IN_INVOICE(CodigoCaso, FechaDeCierre, FacNum.ToString());

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    EntradaConvertidaBilling NodoResultado = new EntradaConvertidaBilling();

                    DateTime FE_DT;         // Fecha de entrada (DateTime).
                    string MesFE_STR = "";  // Mes de la fecha de entrada (string).
                    string DiaFE_STR = "";  // Dia de la fecha de entrada (string).
                    string AñoFE_STR = "";  // Año de la fecha de entrada (string).

                    NodoResultado.Dae_code = RPAD.Dae_code;
                    FE_DT = Convert.ToDateTime(RPAD.Dae_date);
                    MesFE_STR = FE_DT.Month.ToString().PadLeft(2, '0');
                    DiaFE_STR = FE_DT.Day.ToString().PadLeft(2, '0');
                    AñoFE_STR = FE_DT.Year.ToString();
                    NodoResultado.Dae_date = MesFE_STR + '/' + DiaFE_STR + '/' + AñoFE_STR;
                    NodoResultado.Act_code = RPAD.Act_code;
                    NodoResultado.Dae_hourAct = Convert.ToDouble(RPAD.Dae_hourAct);
                    NodoResultado.Dae_milesAct = Convert.ToDouble(RPAD.Dae_milesAct);
                    NodoResultado.Dae_comment = RPAD.Dae_comment;
                    NodoResultado.Aty_price = Convert.ToChar(RPAD.Aty_price);

                    Resultado.Add(NodoResultado);
                } 

                return Resultado;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        public List<EntradaVerificadaEnCaso> VerificaEntradasEnCaso(int CodigoCaso = 0)
        {
            ObjectResult<SP_VERIFY_ENTRIES_IN_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;
            List<EntradaVerificadaEnCaso> Resultado = new List<EntradaVerificadaEnCaso>();

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_VERIFY_ENTRIES_IN_CASE(CodigoCaso);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    EntradaVerificadaEnCaso NodoResultado = new EntradaVerificadaEnCaso();

                    DateTime FE_DT;         // Fecha de entrada (DateTime).
                    string MesFE_STR = "";  // Mes de la fecha de entrada (string).
                    string DiaFE_STR = "";  // Dia de la fecha de entrada (string).
                    string AñoFE_STR = "";  // Año de la fecha de entrada (string).

                    NodoResultado.Dae_code = RPAD.Dae_code;
                    FE_DT = Convert.ToDateTime(RPAD.Dae_date);
                    MesFE_STR = FE_DT.Month.ToString().PadLeft(2, '0');
                    DiaFE_STR = FE_DT.Day.ToString().PadLeft(2, '0');
                    AñoFE_STR = FE_DT.Year.ToString();
                    NodoResultado.Dae_date = MesFE_STR + '/' + DiaFE_STR + '/' + AñoFE_STR;
                    NodoResultado.Act_code = RPAD.Act_code;
                    NodoResultado.Dae_hourAct = Convert.ToDouble(RPAD.Dae_hourAct);
                    NodoResultado.Dae_milesAct = Convert.ToDouble(RPAD.Dae_milesAct);
                    NodoResultado.Dae_comment = RPAD.Dae_comment;
                    NodoResultado.Aty_price = Convert.ToChar(RPAD.Aty_price);

                    Resultado.Add(NodoResultado);
                }
                 
                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public List<PDataEntry_Lista> ListaBloqueEntradasDeCaso(int codigoCaso = 0, int paginaDestino = 0)
        {
            // ObjectResult<PruebaCierreMes_Result> ResultadoProcedimientoAlmacenadoDuro;
            DataRowCollection ColeccionFilas;
            List<PDataEntry_Lista> Lista = new List<PDataEntry_Lista>();
            DataTable dt = new DataTable("entradas");

            try
            {
                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_DATAENTRY", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@codigoDeCaso", SqlDbType.Int)).Value = codigoCaso;
                        Comando.Parameters.Add(new SqlParameter("@paginaDestino", SqlDbType.Int)).Value = paginaDestino;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    PDataEntry_Lista NodoLista = new PDataEntry_Lista();

                    NodoLista.Dae_code = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.Dae_code_old = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.Dae_date = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.Act_code = int.Parse(Fila.ItemArray.GetValue(3).ToString());
                    NodoLista.Act_code_old = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.Dae_hourAct = Decimal.Parse(Fila.ItemArray.GetValue(5).ToString());
                    NodoLista.Dae_milesAct = Decimal.Parse(Fila.ItemArray.GetValue(6).ToString());
                    //NodoLista.Dae_comment = WebUtility.HtmlDecode(Fila.ItemArray.GetValue(7).ToString());//para problemas con el & se probo y funciono con esto, al final era vista
                    NodoLista.Dae_comment = Fila.ItemArray.GetValue(7).ToString();
                    NodoLista.Acumulado = Decimal.Parse (Fila.ItemArray.GetValue(8).ToString());
                    NodoLista.Row = int.Parse(Fila.ItemArray.GetValue(9).ToString());
                    NodoLista.Dae_facNum = int.Parse(Fila.ItemArray.GetValue(10).ToString());
                    NodoLista.InicialesUsuario = Fila.ItemArray.GetValue(11).ToString();
                    NodoLista.Eve_codigo = (Fila.ItemArray.GetValue(12) == DBNull.Value) ? 0 : int.Parse(Fila.ItemArray.GetValue(12).ToString());                        

                    Lista.Add(NodoLista);
                }
                
                // ResultadoProcedimientoAlmacenadoDuro = db.PruebaCierreMes(FechaDeCierre, ListaCorrelativosSeleccionados, TipoCierre, CodigoUsuario);

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PDataEntry_Lista leeEntrada(int codigoDeEntrada = 0)
        {
            DataRowCollection coleccionFilas;
            PDataEntry_Lista entrada = new PDataEntry_Lista();
            DataTable dt = new DataTable("entrada");

            try
            {
                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_GET_DATAENTRY", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@codigoDeEntrada", SqlDbType.Int)).Value = codigoDeEntrada;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                coleccionFilas = dt.Rows;

                foreach (DataRow Fila in coleccionFilas)
                {
                    DateTime FE_DT;         // Fecha de entrada (DateTime).
                    string MesFE_STR = "";  // Mes de la fecha de entrada (string).
                    string DiaFE_STR = "";  // Dia de la fecha de entrada (string).
                    string AñoFE_STR = "";  // Año de la fecha de entrada (string).


                    FE_DT = Convert.ToDateTime(Fila.ItemArray.GetValue(dt.Columns["Dae_date"].Ordinal));
                    MesFE_STR = FE_DT.Month.ToString().PadLeft(2, '0');
                    DiaFE_STR = FE_DT.Day.ToString().PadLeft(2, '0');
                    AñoFE_STR = FE_DT.Year.ToString();

                    entrada.Dae_date = MesFE_STR + '/' + DiaFE_STR + '/' + AñoFE_STR;
                    entrada.Act_code = int.Parse(Fila.ItemArray.GetValue(dt.Columns["Act_code"].Ordinal).ToString());
                    entrada.Dae_hourAct = Decimal.Parse(Fila.ItemArray.GetValue(dt.Columns["Dae_hourAct"].Ordinal).ToString());
                    entrada.Dae_milesAct = Decimal.Parse(Fila.ItemArray.GetValue(dt.Columns["Dae_milesAct"].Ordinal).ToString());
                    entrada.Dae_comment = Fila.ItemArray.GetValue(dt.Columns["Dae_comment"].Ordinal).ToString();                    
                }

                return entrada;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

    }
}