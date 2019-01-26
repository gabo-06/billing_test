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
    public class BillingModel
    {
        private string CadenaConexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        private OmnimedBDEntities db = new OmnimedBDEntities();


        public List<FacturaParaBilling> BuscarFacturasDeCaso(string Claim = ""
                                                           ,string ApellidoPaciente = ""
                                                           ,int CodigoAseguradora = 0
                                                           ,string EstadoPago = "")
        {
            List<FacturaParaBilling> Lista = new List<FacturaParaBilling>();
            ObjectResult<SP_SEARCH_CASE_BILLING_Result> ResultadoProcedimientoAlmacenadoDuro;
            
            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SEARCH_CASE_BILLING(Claim, ApellidoPaciente, CodigoAseguradora, EstadoPago);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    FacturaParaBilling ElementoLista = new FacturaParaBilling();

                    DateTime FechaFacturacion;
                    DateTime FechaCierre;
                    DateTime FechaAccidente;
                    string MesFechaFacturacion = "";
                    string DiaFechaFacturacion = "";
                    string AñoFechaFacturacion = "";
                    string MesFechaCierre = "";
                    string DiaFechaCierre = "";
                    string AñoFechaCierre = "";
                    string MesFechaAccidente = "";
                    string DiaFechaAccidente = "";
                    string AñoFechaAccidente = "";

                    ElementoLista.CodigoCaso = int.Parse(RPAD.Cis_code.ToString());
                    ElementoLista.Claim = (RPAD.Cis_caseCode == null) ? "" : RPAD.Cis_caseCode.ToString();
                    ElementoLista.Paciente = RPAD.Patient.ToString();
                    ElementoLista.Pat_code = RPAD.Pat_code;

                    ElementoLista.Aseguradora = RPAD.Insurer.ToString();
                    ElementoLista.CodigoFacturaSTR = RPAD.Bih_code_old.ToString();
                    ElementoLista.CodigoFacturaINT = int.Parse(RPAD.Bih_code.ToString());

                    FechaFacturacion = Convert.ToDateTime(RPAD.Bih_billDate);
                    MesFechaFacturacion = FechaFacturacion.Month.ToString().PadLeft(2, '0');
                    DiaFechaFacturacion = FechaFacturacion.Day.ToString().PadLeft(2, '0');
                    AñoFechaFacturacion = FechaFacturacion.Year.ToString().PadLeft(2, '0');
                    ElementoLista.FechaFacturacion = MesFechaFacturacion + '/' + DiaFechaFacturacion + '/' + AñoFechaFacturacion;

                    FechaCierre = Convert.ToDateTime(RPAD.Bih_closingDate);
                    MesFechaCierre = FechaCierre.Month.ToString().PadLeft(2, '0');
                    DiaFechaCierre = FechaCierre.Day.ToString().PadLeft(2, '0');
                    AñoFechaCierre = FechaCierre.Year.ToString().PadLeft(2, '0');
                    ElementoLista.FechaCierre = MesFechaCierre + '/' + DiaFechaCierre + '/' + AñoFechaCierre;

                    FechaAccidente = Convert.ToDateTime(RPAD.AccidentDate);
                    MesFechaAccidente = FechaAccidente.Month.ToString().PadLeft(2, '0');
                    DiaFechaAccidente = FechaAccidente.Day.ToString().PadLeft(2, '0');
                    AñoFechaAccidente = FechaAccidente.Year.ToString().PadLeft(2, '0');
                    ElementoLista.FechaAccidente = MesFechaAccidente + '/' + DiaFechaAccidente + '/' + AñoFechaAccidente;

                    Lista.Add(ElementoLista);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public List<DetalleFactura> CargaDetalleFactura(string NumeroFactura = "")
        {
            ObjectResult<SP_LIST_BILLING_Result> DetalleFactura;
            List<DetalleFactura> Lista;

            DetalleFactura = (ObjectResult<SP_LIST_BILLING_Result>)null;
            Lista = new List<DetalleFactura>();

            try
            {
                DetalleFactura = db.SP_LIST_BILLING(NumeroFactura);

                foreach (var LectorEntrada in DetalleFactura)
                {
                    DetalleFactura ObjEntrada = new DetalleFactura();

                    DateTime FS_DT;         // Fecha de servicio (DateTime).
                    string MesFS_STR = "";  // Mes de la fecha de servicio (string).
                    string DiaFS_STR = "";  // Dia de la fecha de servicio (string).
                    string AñoFS_STR = "";  // Año de la fecha de servicio (string).

                    FS_DT = Convert.ToDateTime(LectorEntrada.Bib_servDate);
                    MesFS_STR = FS_DT.Month.ToString().PadLeft(2, '0');
                    DiaFS_STR = FS_DT.Day.ToString().PadLeft(2, '0');
                    AñoFS_STR = FS_DT.Year.ToString();
                    ObjEntrada.Bib_servDate = MesFS_STR + '/' + DiaFS_STR + '/' + AñoFS_STR;
                    ObjEntrada.Act_code = int.Parse(LectorEntrada.Act_code.ToString());
                    ObjEntrada.Act_description = LectorEntrada.Act_description.ToString();
                    ObjEntrada.Bib_hourMile = decimal.Parse(LectorEntrada.Bib_hourMile.ToString());
                    ObjEntrada.Bib_priceAct = decimal.Parse(LectorEntrada.Bib_priceAct.ToString());
                    ObjEntrada.Bib_amoReim = decimal.Parse(LectorEntrada.Bib_amoReim.ToString());
                    ObjEntrada.Bib_code = int.Parse(LectorEntrada.Bib_code.ToString());
                    ObjEntrada.BalanceDue = decimal.Parse(LectorEntrada.BalanceDue.ToString());

                    Lista.Add(ObjEntrada);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        public List<DetalleFactura> CargaDetalleFactura_Nuevo(string NumeroFactura = "", int paginaDestino = 0)
        {
            DataRowCollection ColeccionFilas;
            List<DetalleFactura> Lista = new List<DetalleFactura>();
            DataTable dt = new DataTable("detalles");

            try
            {
                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_BILLING_2", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Bih_code_old", SqlDbType.Char)).Value = NumeroFactura;
                        Comando.Parameters.Add(new SqlParameter("@paginaDestino", SqlDbType.Int)).Value = paginaDestino;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }                    
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow fila in ColeccionFilas)
                {
                    DetalleFactura ObjEntrada = new DetalleFactura();
                    DateTime FS_DT;         // Fecha de servicio (DateTime).
                    string MesFS_STR = "";  // Mes de la fecha de servicio (string).
                    string DiaFS_STR = "";  // Dia de la fecha de servicio (string).
                    string AñoFS_STR = "";  // Año de la fecha de servicio (string).

                    FS_DT = Convert.ToDateTime(fila.ItemArray[dt.Columns["Bib_servDate"].Ordinal].ToString());
                    MesFS_STR = FS_DT.Month.ToString().PadLeft(2, '0');
                    DiaFS_STR = FS_DT.Day.ToString().PadLeft(2, '0');
                    AñoFS_STR = FS_DT.Year.ToString();
                    
                    ObjEntrada.Bib_servDate = MesFS_STR + '/' + DiaFS_STR + '/' + AñoFS_STR;
                    ObjEntrada.Act_code = int.Parse(fila.ItemArray[dt.Columns["Act_code"].Ordinal].ToString());
                    ObjEntrada.Act_description = fila.ItemArray[dt.Columns["Act_description"].Ordinal].ToString();
                    ObjEntrada.Bib_hourMile = Decimal.Parse(fila.ItemArray[dt.Columns["Bib_hourMile"].Ordinal].ToString());
                    ObjEntrada.Bib_priceAct = Decimal.Parse(fila.ItemArray[dt.Columns["Bib_priceAct"].Ordinal].ToString());
                    ObjEntrada.Bib_amoReim = Decimal.Parse(fila.ItemArray[dt.Columns["Bib_amoReim"].Ordinal].ToString());
                    ObjEntrada.Bib_code = int.Parse(fila.ItemArray[dt.Columns["Bib_code"].Ordinal].ToString());
                    ObjEntrada.BalanceDue = Decimal.Parse(fila.ItemArray[dt.Columns["BalanceDue"].Ordinal].ToString());

                    Lista.Add(ObjEntrada);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       

        public List<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result> ObtenerInformacionPdfBilling_header(int codigoDeCaso = 0, int codigoFactura = 0)
        {
            //ObjectResult<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result> DetalleFactura;
            //List<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result> Lista;

            List<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result> Resultado = new List<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result>();
            ObjectResult<SP_GET_CASE_DATA_FOR_PDF_BILLING_Result> ResultadoProcedimientoAlmacenadoDuro;
            try
            {
            

                ResultadoProcedimientoAlmacenadoDuro = db.SP_GET_CASE_DATA_FOR_PDF_BILLING(codigoDeCaso,codigoFactura);

                foreach (var LectorEntrada in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_GET_CASE_DATA_FOR_PDF_BILLING_Result NodoResultado = new SP_GET_CASE_DATA_FOR_PDF_BILLING_Result();
                    NodoResultado.Bih_returnDate = LectorEntrada.Bih_returnDate;
                    NodoResultado.Bih_startWeekly  = LectorEntrada.Bih_startWeekly;
                    NodoResultado.CaseAccidentDate = LectorEntrada.CaseAccidentDate;
                    NodoResultado.CaseCaseCode  = LectorEntrada.CaseCaseCode;
                    NodoResultado.CaseReferralDate = LectorEntrada.CaseReferralDate;
                    NodoResultado.Ins_carrierCode = LectorEntrada.Ins_carrierCode;
                    NodoResultado.Ins_fax = LectorEntrada.Ins_carrierCode;
                    NodoResultado.Ins_feinCc = LectorEntrada.Ins_feinCc;
                    NodoResultado.Ins_feinSc = LectorEntrada.Ins_feinSc;
                    NodoResultado.Ins_feinSc = LectorEntrada.Ins_feinSc;
                    NodoResultado.Ins_phone = LectorEntrada.Ins_phone;
                    NodoResultado.Ins_scTpaCode = LectorEntrada.Ins_scTpaCode;
                    NodoResultado.Insurer = LectorEntrada.Insurer;
                    NodoResultado.InsurerAddress = LectorEntrada.InsurerAddress;
                    NodoResultado.InsurerCity = LectorEntrada.InsurerCity;
                    NodoResultado.InsurerZipCode  = LectorEntrada.InsurerZipCode;
                    NodoResultado.Par_address = LectorEntrada.Par_address;
                    NodoResultado.Par_city = LectorEntrada.Par_city;
                    NodoResultado.Par_name = LectorEntrada.Par_name;
                    NodoResultado.Par_number = LectorEntrada.Par_number;
                    NodoResultado.Par_phone = LectorEntrada.Par_phone;
                    NodoResultado.Par_state  = LectorEntrada.Par_state;
                    NodoResultado.Par_zipCode = LectorEntrada.Par_zipCode;
                    NodoResultado.Pro_fein = LectorEntrada.Pro_fein;
                    NodoResultado.Pro_number = LectorEntrada.Pro_number;
                    NodoResultado.Pro_status = LectorEntrada.Pro_status;
                    NodoResultado.Pro_status = LectorEntrada.Pro_status;
                    NodoResultado.Pro_fullName = LectorEntrada.Pro_fullName;
                    NodoResultado.Adj_fullName = LectorEntrada.Adj_fullName;
                    

                    Resultado.Add(NodoResultado);

                }
                return Resultado;
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
        }

        // Elimina un detalle de una factura.
        public int EliminaDetalle(int codigoDeDetalle = 0, int codigoDeUsuario = 0, string numeroDeFactura = "")
        {
            // ObjectResult<SP_UPDATE_DATAENTRY_Result> EntradasActualizadas;
            // int EntradasActualizadas;

            // EntradasActualizadas = (ObjectResult<SP_UPDATE_DATAENTRY_Result>)null;
            // EntradasActualizadas = 0;

            try
            {
                db.SP_DELETE_BILLING_BODY(codigoDeDetalle, codigoDeUsuario, numeroDeFactura);

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

        public List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result> Listar_Tabla_Billing_Formulario()
        {
            List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result> Resultado = new List<SP_SEARCH_CASE_BILLING_UNFILTERED_Result>();
            ObjectResult<SP_SEARCH_CASE_BILLING_UNFILTERED_Result> ResultadoProcedimientoAlmacenadoDuro;

           try
           {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SEARCH_CASE_BILLING_UNFILTERED();
                
                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SEARCH_CASE_BILLING_UNFILTERED_Result NodoResultado = new SP_SEARCH_CASE_BILLING_UNFILTERED_Result();
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Cis_caseCode = (RPAD.Cis_caseCode == null) ? "" : RPAD.Cis_caseCode;
                    NodoResultado.Pat_code = RPAD.Pat_code;
                    NodoResultado.Patient = (RPAD.Patient == null) ? "" : RPAD.Patient;
                    NodoResultado.Pat_firstName = (RPAD.Pat_firstName == null) ? "" : RPAD.Pat_firstName;
                    NodoResultado.Pat_lastName = (RPAD.Pat_lastName == null) ? "" : RPAD.Pat_lastName;
                    NodoResultado.Insurer = (RPAD.Insurer == null) ? "" : RPAD.Insurer;
                    NodoResultado.Bih_code = (RPAD.Bih_code == null) ? 0 : RPAD.Bih_code;
                    NodoResultado.Bih_code_old = (RPAD.Bih_code_old == null) ? "" : RPAD.Bih_code_old;
                    NodoResultado.Bih_billDate = RPAD.Bih_billDate;
                    NodoResultado.Bih_closingDate = RPAD.Bih_closingDate;
                    NodoResultado.AccidentDate = RPAD.AccidentDate;
                    NodoResultado.Bih_payStatus = RPAD.Bih_payStatus;

                    Resultado.Add(NodoResultado);
               }
            }
            catch (Exception ex)
            {
                   throw ex;
            }

            return Resultado;
        }

        public int obtenerCantidadDeDetallesDeFactura(string numeroDeFactura = "")
        {
            int cantidadDeDetallesDeFactura = 0;

            try
            {
                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_GET_COUNT_OF_INVOICE_DETAILS", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Bih_code_old", SqlDbType.Char)).Value = numeroDeFactura;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            cantidadDeDetallesDeFactura = int.Parse(Comando.ExecuteScalar().ToString());
                        }
                    }
                }

                return cantidadDeDetallesDeFactura;
            }
            catch (Exception ex)
            {

                throw ex;
            }        
        }

        public DataTable[] obtenerMontosDeFactura(string numeroDeFactura = "")
        {
            DataSet dts = new DataSet();
            DataTable[] arregloTablas;

            try
            {
                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand comando = new SqlCommand("SP_GET_INVOICE_AMOUNTS", Conexion))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.CommandTimeout = 480;
                        comando.Parameters.Add(new SqlParameter("@Bih_code_old", SqlDbType.Char)).Value = numeroDeFactura;                        

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(comando))
                        {
                            Adaptador.Fill(dts);
                        }
                    }
                }

                arregloTablas = new DataTable[] { dts.Tables[0], dts.Tables[1] };

                return arregloTablas;

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        // Actualiza las detalles de una factura.
        // public List<PDetalleFactura_RegistraActualiza> RegistraActualizaDetalleFactura(string DetallesXML)
        public DataTable[] RegistraActualizaDetalleFactura(string DetallesXML)
        {
            DataSet dts = new DataSet();
            DataTable dtt1;
            DataTable dtt2;
            // DataRowCollection Detalles;
            // decimal[] Montos;
            List<PDetalleFactura_RegistraActualiza> Lista;

            Lista = new List<PDetalleFactura_RegistraActualiza>();
            // dt1 = new DataTable("data");
            dtt1 = new DataTable("data");
            dtt2 = new DataTable("data");

            try
            {
                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_SAVE_UPDATE_BILLING", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@InvoiceDetail", SqlDbType.Xml)).Value = DetallesXML;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dts);
                        }
                    }
                }

                // dtt1 = dts.Tables[0];
                // dtt2 = dts.Tables[1];


                // Detalles = dtt2.Rows;

                // foreach (DataRow Detalle in Detalles)
                // {
                //     PDetalleFactura_RegistraActualiza ElementoLista = new PDetalleFactura_RegistraActualiza();                    
                // 
                //     ElementoLista.NumeroCorrelativoDetalle = int.Parse(Detalle.ItemArray.GetValue(0).ToString());
                //     ElementoLista.Bih_code = int.Parse(Detalle.ItemArray.GetValue(1).ToString());
                //     ElementoLista.Bib_code = int.Parse(Detalle.ItemArray.GetValue(2).ToString());
                //     ElementoLista.Bib_servDate = DateTime.Parse(Detalle.ItemArray.GetValue(3).ToString());
                //     ElementoLista.Act_code = int.Parse(Detalle.ItemArray.GetValue(4).ToString());
                //     ElementoLista.Bib_hourMile = decimal.Parse(Detalle.ItemArray.GetValue(5).ToString());
                //     ElementoLista.Bib_priceAct = decimal.Parse(Detalle.ItemArray.GetValue(6).ToString());
                //     ElementoLista.Bib_amoReim = decimal.Parse(Detalle.ItemArray.GetValue(7).ToString());
                // 
                //     Lista.Add(ElementoLista); 
                // }

                // return Json(new response = new { FilaActualizada = CantidadEntradasRegistradas, FilaRegistrada = CantidadEntradasRegistradas } );

                // return dts;

                DataTable[] ArregloTablas = new DataTable[] { dts.Tables[0], dts.Tables[1] };

                return ArregloTablas;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}