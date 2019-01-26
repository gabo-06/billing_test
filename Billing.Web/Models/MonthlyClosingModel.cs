using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web; 
using System.Diagnostics;
using System.Data.Objects;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Billing.Web.Models
{
    public class MonthlyClosingModel
    {
        private string CadenaConexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        private OmnimedBDEntities db = new OmnimedBDEntities();

        // ****************************************************************************************************************

        // Método que lista los casos que NO TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora.
        public List<CasoCierreMes> ListaCasosNoCarlisle()
        {
            List<CasoCierreMes> Resultado = new List<CasoCierreMes>();
            ObjectResult<SP_LIST_GENERATED_INVOICE_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_GENERATED_INVOICE();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    CasoCierreMes NodoResultado = new CasoCierreMes();

                    string MesFechaAccidenteStr = "";
                    string DiaFechaAccidenteStr = "";
                    string AñoFechaAccidenteStr = "";
                    string FechaAccidenteStr = "";
                    string MesFechaEntradaStr = ""; 
                    string DiaFechaEntradaStr = "";
                    string AñoFechaEntradaStr = ""; 
                    string FechaEntradaStr = "";

                    MesFechaAccidenteStr = RPAD.Cis_accidentDate.Month.ToString().PadLeft(2, '0');
                    DiaFechaAccidenteStr = RPAD.Cis_accidentDate.Day.ToString().PadLeft(2, '0');
                    AñoFechaAccidenteStr = RPAD.Cis_accidentDate.Year.ToString();
                    FechaAccidenteStr = MesFechaAccidenteStr + '/' + DiaFechaAccidenteStr + '/' + AñoFechaAccidenteStr;

                    NodoResultado.Correlativo = int.Parse(RPAD.Correlativo.ToString());
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Cis_code_old = RPAD.Cis_code_old;
                    NodoResultado.Cis_caseCode = RPAD.Cis_caseCode;
                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.Pat_socialSecurityNumberD = RPAD.Pat_socialSecurityNumberD;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.Cis_accidentDate = FechaAccidenteStr;
                    NodoResultado.Hours = RPAD.Hours.ToString();
                    // ************************** Fecha de Entrada **************************
                    if (RPAD.Date.ToString() == "-")
                        FechaEntradaStr = RPAD.Date;
                    else
                    {
                        MesFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Month.ToString().PadLeft(2, '0');
                        DiaFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Day.ToString().PadLeft(2, '0');
                        AñoFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Year.ToString();
                        FechaEntradaStr = MesFechaEntradaStr + '/' + DiaFechaEntradaStr + '/' + AñoFechaEntradaStr;
                    }
                    NodoResultado.Date = FechaEntradaStr;
                    // **********************************************************************
                    NodoResultado.Dae_facNum = RPAD.Dae_facNum.ToString();
                    NodoResultado.Cis_price = RPAD.Cis_price.ToString();

                    Resultado.Add(NodoResultado);
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Método que lista los casos que TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora (Tipo de cierre: Por Hora).
        public List<CasoCierreMes> ListaCasosCarlislePorHora()
        {
            List<CasoCierreMes> Resultado = new List<CasoCierreMes>();
            ObjectResult<SP_LIST_GENERATED_INVOICE_HOUR_DATE_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_GENERATED_INVOICE_HOUR_DATE(2);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    CasoCierreMes NodoResultado = new CasoCierreMes();
                    
                    string MesFechaAccidenteStr = "";
                    string DiaFechaAccidenteStr = "";
                    string AñoFechaAccidenteStr = "";
                    string FechaAccidenteStr = "";
                    string MesFechaEntradaStr = ""; 
                    string DiaFechaEntradaStr = "";
                    string AñoFechaEntradaStr = ""; 
                    string FechaEntradaStr = "";

                    MesFechaAccidenteStr = RPAD.Cis_accidentDate.Month.ToString().PadLeft(2, '0');
                    DiaFechaAccidenteStr = RPAD.Cis_accidentDate.Day.ToString().PadLeft(2, '0');
                    AñoFechaAccidenteStr = RPAD.Cis_accidentDate.Year.ToString();
                    FechaAccidenteStr = MesFechaAccidenteStr + '/' + DiaFechaAccidenteStr + '/' + AñoFechaAccidenteStr;

                    NodoResultado.Correlativo = int.Parse(RPAD.Correlativo.ToString());
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Cis_code_old = RPAD.Cis_code_old;
                    NodoResultado.Cis_caseCode = RPAD.Cis_caseCode;
                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.Pat_socialSecurityNumberD = RPAD.Pat_socialSecurityNumberD;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.Cis_accidentDate = FechaAccidenteStr;
                    NodoResultado.Hours = RPAD.Hours.ToString();
                    // ************************** Fecha de Entrada **************************
                    if (RPAD.Date.ToString() == "-")
                        FechaEntradaStr = RPAD.Date.ToString();
                    else
                    {
                        MesFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Month.ToString().PadLeft(2, '0');
                        DiaFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Day.ToString().PadLeft(2, '0');
                        AñoFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Year.ToString();
                        FechaEntradaStr = MesFechaEntradaStr + '/' + DiaFechaEntradaStr + '/' + AñoFechaEntradaStr;
                    }
                    NodoResultado.Date = FechaEntradaStr;
                    // **********************************************************************
                    NodoResultado.Dae_facNum = (Convert.ToInt64(RPAD.Dae_facNum) == 0) ? "-" : Convert.ToInt64(RPAD.Dae_facNum).ToString();
                    NodoResultado.Cis_price = RPAD.Cis_price.ToString();

                    Resultado.Add(NodoResultado);
                }

                return Resultado;                
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        // Método que lista los casos que TIENEN a "Insurer - Amtrust C/O Carlisle & Assoc" como aseguradora (Tipo de cierre: Por Fecha).
        public List<CasoCierreMes> ListaCasosCarlislePorFecha()
        {
            List<CasoCierreMes> Resultado = new List<CasoCierreMes>();
            ObjectResult<SP_LIST_GENERATED_INVOICE_HOUR_DATE_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_GENERATED_INVOICE_HOUR_DATE(3);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    CasoCierreMes NodoResultado = new CasoCierreMes();

                    string MesFechaAccidenteStr = "";
                    string DiaFechaAccidenteStr = "";
                    string AñoFechaAccidenteStr = "";
                    string FechaAccidenteStr = "";
                    string MesFechaEntradaStr = ""; 
                    string DiaFechaEntradaStr = "";
                    string AñoFechaEntradaStr = ""; 
                    string FechaEntradaStr = "";

                    MesFechaAccidenteStr = RPAD.Cis_accidentDate.Month.ToString().PadLeft(2, '0');
                    DiaFechaAccidenteStr = RPAD.Cis_accidentDate.Day.ToString().PadLeft(2, '0');
                    AñoFechaAccidenteStr = RPAD.Cis_accidentDate.Year.ToString();
                    FechaAccidenteStr = MesFechaAccidenteStr + '/' + DiaFechaAccidenteStr + '/' + AñoFechaAccidenteStr;

                    NodoResultado.Correlativo = int.Parse(RPAD.Correlativo.ToString());
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Cis_code_old = RPAD.Cis_code_old;
                    NodoResultado.Cis_caseCode = RPAD.Cis_caseCode;
                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.Pat_socialSecurityNumberD = RPAD.Pat_socialSecurityNumberD;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.Cis_accidentDate = FechaAccidenteStr;
                    NodoResultado.Hours = RPAD.Hours.ToString();
                    // ************************** Fecha de Entrada **************************
                    if (RPAD.Date.ToString() == "-")
                        FechaEntradaStr = RPAD.Date.ToString();
                    else
                    {
                        MesFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Month.ToString().PadLeft(2, '0');
                        DiaFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Day.ToString().PadLeft(2, '0');
                        AñoFechaEntradaStr = Convert.ToDateTime(RPAD.Date).Year.ToString();
                        FechaEntradaStr = MesFechaEntradaStr + '/' + DiaFechaEntradaStr + '/' + AñoFechaEntradaStr;
                    }
                    NodoResultado.Date = FechaEntradaStr;
                    // **********************************************************************
                    NodoResultado.Dae_facNum = (Convert.ToInt64(RPAD.Dae_facNum) == 0) ? "-" : Convert.ToInt64(RPAD.Dae_facNum).ToString();
                    NodoResultado.Cis_price = RPAD.Cis_price.ToString();

                    Resultado.Add(NodoResultado);                    
                }

                return Resultado;   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public string ObtenerUltimaFechaCierre(int CodigoCaso = 0)
        {
            ObjectResult ResultadoProcedimientoAlmacenadoDuro;
            DateTime FechaUltimoCierre = DateTime.Now;
            string FechaUltimoCierreSTR = "";

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_GET_LAST_CLOSING_DATE(CodigoCaso);                

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    DateTime FechaUltimoCierreDT = Convert.ToDateTime(RPAD);

                    string MesUFC = FechaUltimoCierreDT.Month.ToString();  // Mes de la fecha de entrada (string).
                    string DiaUFC = FechaUltimoCierreDT.Day.ToString();  // Dia de la fecha de entrada (string).
                    string AñoUFC = FechaUltimoCierreDT.Year.ToString();  // Año de la fecha de entrada (string).

                    FechaUltimoCierreSTR = AñoUFC + '-' + MesUFC.PadLeft(2, '0') + '-' + DiaUFC.PadLeft(2, '0');
                }

                return FechaUltimoCierreSTR;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int RegistraCabeceraFactura(int CodigoCaso = 0
                                                 , int CodigoParametro = 0
                                                 , int CodigoUsuario = 0
                                                 , string FechaDeCierre = ""
                                                 , string NumFac = "")
        {
            ObjectResult ResultadoProcedimientoAlmacenadoDuro;
            int CodigoFacturaRegistrada = 0;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_BILLING_HEAD(CodigoCaso
                                                                            , CodigoParametro
                                                                            , CodigoUsuario
                                                                            , FechaDeCierre
                                                                            , NumFac);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    CodigoFacturaRegistrada = int.Parse(RPAD.ToString());

                    Debug.WriteLine(CodigoFacturaRegistrada);
                }

                return CodigoFacturaRegistrada;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistraDetalleCuerpoFactura(int CodigoCabeceraFactura = 0
                                        ,string FechaServidor = ""
                                        ,string CodigoMMC = "" // "MMC"
                                        ,int codigoActividad = 0
                                        ,decimal Unidad = 0
                                        ,decimal Precio = 0
                                        ,int CodigoCaso = 0
                                        ,char TipoActividad = '\0'
                                        ,decimal AmoReim = 0
                                        ,int CodigoUsuario = 0
                                        ,int CodigoEntrada = 0)
        {
            ObjectResult ResultadoProcedimientoAlmacenadoDuro;
            int CodigoDetalleFacturaRegistrado = 0;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_BILLING_BODY(CodigoCabeceraFactura
                                                                                , FechaServidor
                                                                                , CodigoMMC
                                                                                , codigoActividad
                                                                                , Unidad
                                                                                , Precio
                                                                                , CodigoCaso
                                                                                , TipoActividad.ToString()
                                                                                , AmoReim
                                                                                , CodigoUsuario
                                                                                , CodigoEntrada);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    CodigoDetalleFacturaRegistrado = int.Parse(RPAD.ToString());

                    Debug.WriteLine(CodigoDetalleFacturaRegistrado);
                }

                return CodigoDetalleFacturaRegistrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public float[] TotalFactura(int CodigoFacturaGenerada) 
        {
            ObjectResult<SP_GET_FULL_AMOUNT_BILLING_Result> ResultadoProcedimientoAlmacenadoDuro;
            float Total = 0;
            float Amount = 0;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_GET_FULL_AMOUNT_BILLING(CodigoFacturaGenerada);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    Total = float.Parse(RPAD.Total.ToString());
                    Amount = float.Parse(RPAD.Amount.ToString());                    
                }

                float[] array = new float[] { Total, Amount };
             
                return array;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void ActualizaTotalFactura(int CodigoFacturaGenerada = 0, decimal Total = 0)
        {
            try
            {
                db.SP_UPDATE_TOTAL_BILLING_HEAD(CodigoFacturaGenerada, Total);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public void GeneraFactura(int CodigoCaso = 0, string FechaDeCierre = "", string NumFac = "")
        {
            try
            {
                db.SP_GENERATE_INVOICE(CodigoCaso, FechaDeCierre, NumFac);
            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }

        public List<FacturaParaRevertir> ListarBillingPorFechaDeCierre(string FechaDeCierre = "")
        {
            ObjectResult<SP_LIST_BILLING_BY_CLOSING_DATE_Result> ResultadoProcedimientoAlmacenadoDuro;
            List<FacturaParaRevertir> Resultado = new List<FacturaParaRevertir>();

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_BILLING_BY_CLOSING_DATE(FechaDeCierre);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    FacturaParaRevertir NodoResultado = new FacturaParaRevertir();

                    NodoResultado.CodigoFactura = RPAD.Bih_code;
                    NodoResultado.CodigoCaso = RPAD.Cis_code;
                    NodoResultado.Claim = RPAD.Cis_caseCode;
                    NodoResultado.Paciente = RPAD.Patient;
                    NodoResultado.NumeroSeguroSocial = RPAD.Pat_socialSecurityNumberD;
                    NodoResultado.Aseguradora = RPAD.Insurer;

                    DateTime FA_DT;         // Fecha de accidente (DateTime).
                    string MesFA_STR = "";  // Mes de la fecha de accidente (string).
                    string DiaFA_STR = "";  // Dia de la fecha de accidente (string).
                    string AñoFA_STR = "";  // Año de la fecha de accidente (string).

                    FA_DT = Convert.ToDateTime(RPAD.AccidentalDate);
                    MesFA_STR = FA_DT.Month.ToString().PadLeft(2, '0');
                    DiaFA_STR = FA_DT.Day.ToString().PadLeft(2, '0');
                    AñoFA_STR = FA_DT.Year.ToString();
                    NodoResultado.FechaAccidente = MesFA_STR + '/' + DiaFA_STR + '/' + AñoFA_STR;

                    NodoResultado.TotalFactura = Convert.ToDouble(RPAD.Bih_billTotal);
                    NodoResultado.PagoFactura = Convert.ToDouble(RPAD.Bih_billPay);

                    Resultado.Add(NodoResultado);
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }                
        }

        public List<UltimaFechaCierreReversion> ObtenerUltimasFechaCierreReversion()
        {
            ObjectResult<SP_LIST_LAST_CLOSING_DATE_Result> ResultadoProcedimientoAlmacenadoDuro;
            List<UltimaFechaCierreReversion> Resultado = new List<UltimaFechaCierreReversion>();

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_LAST_CLOSING_DATE();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    UltimaFechaCierreReversion NodoResultado = new UltimaFechaCierreReversion();

                    DateTime FC_DT;         // Fecha de cierre (DateTime).
                    string MesFC_STR = "";  // Mes de la fecha de cierre (string).
                    string DiaFC_STR = "";  // Dia de la fecha de cierre (string).
                    string AñoFC_STR = "";  // Año de la fecha de cierre (string).

                    FC_DT = Convert.ToDateTime(RPAD.FechaCierre);
                    MesFC_STR = FC_DT.Month.ToString().PadLeft(2, '0');
                    DiaFC_STR = FC_DT.Day.ToString().PadLeft(2, '0');
                    AñoFC_STR = FC_DT.Year.ToString();
                    NodoResultado.FechaCierre = MesFC_STR + '/' + DiaFC_STR + '/' + AñoFC_STR;
                    // --------------------------------------------------------------------------
                    // DateTime FRC_DT;         // Fecha de registro del cierre (DateTime).
                    // string MesFRC_STR = "";  // Mes de la fecha de registro del cierre (string).
                    // string DiaFRC_STR = "";  // Dia de la fecha de registro del cierre (string).
                    // string AñoFRC_STR = "";  // Año de la fecha de registro del cierre (string).

                    // FRC_DT = Convert.ToDateTime(RPAD.FechaRegistroCierre);
                    // MesFRC_STR = FRC_DT.Month.ToString().PadLeft(2, '0');
                    // DiaFRC_STR = FRC_DT.Day.ToString().PadLeft(2, '0');
                    // AñoFRC_STR = FRC_DT.Year.ToString();
                    // NodoResultado.FechaRegistroCierre = MesFRC_STR + '/' + DiaFRC_STR + '/' + AñoFRC_STR;

                    NodoResultado.CodigoUsuario = int.Parse(RPAD.CodigoUsuario.ToString());
                    NodoResultado.Usuario = RPAD.Usuario;

                    Resultado.Add(NodoResultado);
                }

                return Resultado; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RevertirFactura(int CodigoFactura = 0, int CodigoUsuario = 0)
        {
            try
            { 
                db.SP_UNDO_GENERATE_INVOICE(CodigoFactura, CodigoUsuario);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<DetalleResultadoCierreMes> PruebaCierreMes(string FechaDeCierre = ""
                                   , string ListaCorrelativosSeleccionados = ""
                                   , int TipoCierre = 0
                                   , int CodigoUsuario = 0)
        {
            // ObjectResult<PruebaCierreMes_Result> ResultadoProcedimientoAlmacenadoDuro;
            DataRowCollection ColeccionFilas;
            List<DetalleResultadoCierreMes> Lista = new List<DetalleResultadoCierreMes>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("PruebaCierreMes", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@FechaCierre", SqlDbType.Char, 10)).Value = FechaDeCierre;
                        Comando.Parameters.Add(new SqlParameter("@ListaCorrelativos", SqlDbType.NVarChar, 8000)).Value = ListaCorrelativosSeleccionados;
                        Comando.Parameters.Add(new SqlParameter("@TipoCierre", SqlDbType.Int)).Value = TipoCierre;
                        Comando.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
                    
                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }
                 
                ColeccionFilas = dt.Rows;
                 
                foreach (DataRow Fila in ColeccionFilas)
                {
                    DetalleResultadoCierreMes NodoLista = new DetalleResultadoCierreMes();
                
                    NodoLista.Correlativo                = Fila.ItemArray.GetValue(0).ToString();
                    NodoLista.CodigoCaso                 = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.PrecioCaso                 = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.Paciente                   = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.FacNum                     = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.CantidadTotalEntradasCaso  = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.CantidadEntradasCasoCerrar = Fila.ItemArray.GetValue(6).ToString();
                    NodoLista.CantidadEntradasRestantes  = Fila.ItemArray.GetValue(7).ToString();
                    NodoLista.Comentario                 = Fila.ItemArray.GetValue(8).ToString();
                    NodoLista.CodigoFactura              = Fila.ItemArray.GetValue(9).ToString();

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



    }
}