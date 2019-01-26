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
    public class PaymentModel
    {
        private string CadenaConexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        private OmnimedBDEntities db = new OmnimedBDEntities();

        // ****************************************************************************************************************

        public List<SP_SEARCH_BILLING_FOR_PAYMENT_Result> ListarFacturas(int CodigoAseguradora = 0)
        {
            ObjectResult<SP_SEARCH_BILLING_FOR_PAYMENT_Result> ResultadoProcedimientoAlmacenadoDuro;
            List<SP_SEARCH_BILLING_FOR_PAYMENT_Result> Resultado = new List<SP_SEARCH_BILLING_FOR_PAYMENT_Result>();

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SEARCH_BILLING_FOR_PAYMENT(CodigoAseguradora);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SEARCH_BILLING_FOR_PAYMENT_Result NodoResultado = new SP_SEARCH_BILLING_FOR_PAYMENT_Result();

                    NodoResultado.Bih_code = int.Parse(RPAD.Bih_code.ToString());
                    NodoResultado.Bih_code_old = (RPAD.Bih_code_old == null || RPAD.Bih_code_old.ToString().Trim() == "") ? "" : RPAD.Bih_code_old.ToString().Trim();
                    NodoResultado.Cis_code = int.Parse(RPAD.Cis_code.ToString());
                    NodoResultado.Cis_caseCode = (RPAD.Cis_caseCode == null || RPAD.Cis_caseCode.ToString().Trim() == "") ? "" : RPAD.Cis_caseCode.ToString().Trim();                                        
                    NodoResultado.Patient = (RPAD.Patient == null || RPAD.Patient.ToString().Trim() == "") ? "" : RPAD.Patient.ToString().Trim();
                    NodoResultado.Pat_socialSecurityNumberD = (RPAD.Pat_socialSecurityNumberD == null || RPAD.Pat_socialSecurityNumberD.ToString().Trim() == "") ? "" : RPAD.Pat_socialSecurityNumberD.ToString().Trim();
                    NodoResultado.Insurer = (RPAD.Insurer == null || RPAD.Insurer.ToString().Trim() == "") ? "" : RPAD.Insurer.ToString().Trim();
                    NodoResultado.Total = (RPAD.Total == null || RPAD.Total.ToString().Trim() == "") ? 0 : decimal.Parse(RPAD.Total.ToString());
                    NodoResultado.Balance = (RPAD.Balance == null || RPAD.Balance.ToString().Trim() == "") ? 0 : decimal.Parse(RPAD.Balance.ToString().Trim());
                    NodoResultado.AccidentDate = (RPAD.AccidentDate == null || RPAD.AccidentDate.ToString().Trim() == "") ? "" : RPAD.AccidentDate.ToString().Trim();
                    NodoResultado.ClosingDate = (RPAD.ClosingDate == null || RPAD.ClosingDate.ToString().Trim() == "") ? "" : RPAD.ClosingDate.ToString().Trim();
                    NodoResultado.PaidLevel = RPAD.PaidLevel.ToString().Trim();
                    
                    Resultado.Add(NodoResultado);
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_LIST_PAYMENTS_BY_FILTERS_Result> ListarPagos(int CodigoAseguradora = 0
                                                                   ,int CodigoPaciente = 0
                                                                   ,string NumeroCheque = ""
                                                                   ,string Claim = ""
                                                                   ,string NumeroFactura = "")
        {
            ObjectResult<SP_LIST_PAYMENTS_BY_FILTERS_Result> ResultadoProcedimientoAlmacenadoDuro;
            List<SP_LIST_PAYMENTS_BY_FILTERS_Result> Resultado = new List<SP_LIST_PAYMENTS_BY_FILTERS_Result>();

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_PAYMENTS_BY_FILTERS(CodigoAseguradora
                                                                                     , CodigoPaciente
                                                                                     , NumeroCheque
                                                                                     , Claim
                                                                                     , NumeroFactura);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_PAYMENTS_BY_FILTERS_Result NodoResultado = new SP_LIST_PAYMENTS_BY_FILTERS_Result();

                    NodoResultado.PayCode = int.Parse(RPAD.PayCode.ToString());
                    NodoResultado.CheckNumber = (RPAD.CheckNumber == null || RPAD.CheckNumber.ToString().Trim() == "") ? "" : RPAD.CheckNumber.ToString().Trim();
                    NodoResultado.PayType = (RPAD.PayType == null || RPAD.PayType.ToString().Trim() == "") ? "" : RPAD.PayType.ToString().Trim();
                    NodoResultado.PayAmount = (RPAD.PayAmount == null || RPAD.PayAmount.ToString().Trim() == "") ? 0 : decimal.Parse(RPAD.PayAmount.ToString());
                    NodoResultado.PayDate = (RPAD.PayDate == null || RPAD.PayDate.ToString().Trim() == "") ? "" : RPAD.PayDate.ToString().Trim();

                    Resultado.Add(NodoResultado);
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_LIST_DETAILT_PAYMENT_Result> ListarDetallePago(int CodigoPago = 0)
        {
            ObjectResult<SP_LIST_DETAILT_PAYMENT_Result> ResultadoProcedimientoAlmacenadoDuro;
            List<SP_LIST_DETAILT_PAYMENT_Result> Resultado = new List<SP_LIST_DETAILT_PAYMENT_Result>();

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_DETAILT_PAYMENT(CodigoPago);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_DETAILT_PAYMENT_Result NodoResultado = new SP_LIST_DETAILT_PAYMENT_Result();

                    NodoResultado.Bih_code = int.Parse(RPAD.Bih_code.ToString());
                    NodoResultado.Bih_code_old = (RPAD.Bih_code_old == null || RPAD.Bih_code_old.ToString().Trim() == "") ? "" : RPAD.Bih_code_old.ToString().Trim();
                    NodoResultado.Cis_code = int.Parse(RPAD.Cis_code.ToString());
                    NodoResultado.Cis_caseCode = (RPAD.Cis_caseCode == null || RPAD.Cis_caseCode.ToString().Trim() == "") ? "" : RPAD.Cis_caseCode.ToString().Trim();
                    NodoResultado.Insurer = (RPAD.Insurer == null || RPAD.Insurer.ToString().Trim() == "") ? "" : RPAD.Insurer.ToString().Trim();
                    NodoResultado.Patient = (RPAD.Patient == null || RPAD.Patient.ToString().Trim() == "") ? "" : RPAD.Patient.ToString().Trim();
                    NodoResultado.AccidentDate = (RPAD.AccidentDate == null || RPAD.AccidentDate.ToString().Trim() == "") ? "" : RPAD.AccidentDate.ToString().Trim();
                    NodoResultado.ClosingDate = (RPAD.ClosingDate == null || RPAD.ClosingDate.ToString().Trim() == "") ? "" : RPAD.ClosingDate.ToString().Trim();
                    NodoResultado.Total = (RPAD.Total == null || RPAD.Total.ToString().Trim() == "") ? 0 : decimal.Parse(RPAD.Total.ToString());
                    NodoResultado.Pab_amount = (RPAD.Pab_amount == null || RPAD.Pab_amount.ToString().Trim() == "") ? 0 : decimal.Parse(RPAD.Pab_amount.ToString());

                    Resultado.Add(NodoResultado);
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result> ListarPagosParaEliminar(int CodigoAseguradora = 0, string NumeroCheque = "")
        {
            ObjectResult<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result> ResultadoProcedimientoAlmacenado;
            List<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result> ListaPagos;

            ResultadoProcedimientoAlmacenado = (ObjectResult<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result>)null;
            ListaPagos = (List<SP_LIST_PAYMENTS_TO_BE_REMOVED_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenado = db.SP_LIST_PAYMENTS_TO_BE_REMOVED(CodigoAseguradora, NumeroCheque);
                ListaPagos = ResultadoProcedimientoAlmacenado.ToList();

                return ListaPagos;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int ObtenerCantidadFacturasPorPagar(int CodigoAseguradora = 0)
        {
            int? ResultadoProcedimientoAlmacenado;
            int CantidadFacturasPorPagar;

            ResultadoProcedimientoAlmacenado = (int?)null;
            CantidadFacturasPorPagar = 0;

            try
            {
                ResultadoProcedimientoAlmacenado = db.SP_GET_COUNT_BILLING_FOR_PAYMENT(CodigoAseguradora).FirstOrDefault();
                CantidadFacturasPorPagar = int.Parse(ResultadoProcedimientoAlmacenado.ToString());

                return CantidadFacturasPorPagar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        public List<SP_SAVE_PAY_Result> LlevaXML_A_BD(decimal Monto = 0
                               , string FechaPago = ""
                               , int FormaPago = 0
                               , string NumeroTransaccion = ""
                               , int CodigoUsuario = 0
                               , string FacturasPorPagarXML = "")
            */
        public DataTable LlevaXML_A_BD(char TipoPago = '\0'
                                     , decimal Monto = 0
                                     , string FechaPago = ""
                                     , int FormaPago = 0
                                     , string NumeroTransaccion = ""
                                     , int CodigoUsuario = 0
                                     , string FacturasPorPagarXML = "")
        {
            // ObjectResult<SP_SAVE_PAY_Result> ResultadoProcedimientoAlmacenadoDuro;
            // List<SP_SAVE_PAY_Result> Lista;
            // 
            // ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_SAVE_PAY_Result>)null;
            // Lista = (List<SP_SAVE_PAY_Result>)null;
            // 
            // Debug.WriteLine(FacturasPorPagarXML);

            try
            {
                DataTable dt = new DataTable("data");
                
                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                { 
                    Conexion.Open();
                    using (SqlCommand Comando = new SqlCommand("SP_SAVE_PAY", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@TipoPago", SqlDbType.Char)).Value = TipoPago;
                        Comando.Parameters.Add(new SqlParameter("@Monto", SqlDbType.Decimal)).Value = Monto;
                        Comando.Parameters.Add(new SqlParameter("@FechaPago", SqlDbType.Char, 10)).Value = FechaPago;
                        Comando.Parameters.Add(new SqlParameter("@FormaPago", SqlDbType.Int)).Value = FormaPago;
                        Comando.Parameters.Add(new SqlParameter("@NumeroTransaccion", SqlDbType.VarChar, 30)).Value = NumeroTransaccion;
                        Comando.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
                        Comando.Parameters.Add(new SqlParameter("@FacturasPorPagarXML", SqlDbType.Text)).Value = FacturasPorPagarXML;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }   
                    }
                }

                return dt;

                /*
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_PAY(Monto
                                                                     ,FechaPago
                                                                     ,FormaPago
                                                                     ,NumeroTransaccion
                                                                     ,CodigoUsuario
                                                                     ,FacturasPorPagarXML);
                Lista = ResultadoProcedimientoAlmacenadoDuro.ToList(); 

                return Lista;
                 */
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public List<SP_LIST_PAY_TYPE_Result> ListarTiposDePago()
        {
            ObjectResult<SP_LIST_PAY_TYPE_Result> ResultadoProcedimientoAlmacenadoDuro;
            List<SP_LIST_PAY_TYPE_Result> ListaTipospago;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_LIST_PAY_TYPE_Result>)null;
            ListaTipospago = (List<SP_LIST_PAY_TYPE_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_PAY_TYPE();
                ListaTipospago = ResultadoProcedimientoAlmacenadoDuro.ToList();

                return ListaTipospago;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal ObtenerDeudaDePago(int CodigoAseguradora = 0)
        {
            decimal? ResultadoProcedimientoAlmacenado;
            decimal DeudaTotal;

            ResultadoProcedimientoAlmacenado = (decimal?)null;
            DeudaTotal = 0;

            try
            {
                ResultadoProcedimientoAlmacenado = db.SP_BILLING_DEBIT(CodigoAseguradora).FirstOrDefault();
                DeudaTotal = decimal.Parse(ResultadoProcedimientoAlmacenado.ToString());

                return DeudaTotal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<SP_SEARCH_BILLING_FOR_PAYMENT_Result> BuscarPorRangoDeFecha(Int32 code = 0,  String fechaIn = "", String fechaFin = "")
        {
            List<SP_SEARCH_BILLING_FOR_PAYMENT_Result> Resultado = new List<SP_SEARCH_BILLING_FOR_PAYMENT_Result>();
            ObjectResult<SP_SEARCH_BILLING_FOR_PAYMENT_BY_DATE_RANGE_Result> ResultadoProcedimientoAlmacenadoDuro;

            //use_code = 98;
            ResultadoProcedimientoAlmacenadoDuro = db.SP_SEARCH_BILLING_FOR_PAYMENT_BY_DATE_RANGE(code, fechaIn, fechaFin);
            try
            {
                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SEARCH_BILLING_FOR_PAYMENT_Result NodoResultado = new SP_SEARCH_BILLING_FOR_PAYMENT_Result();

                    NodoResultado.Bih_code = int.Parse(RPAD.Bih_code.ToString());
                    NodoResultado.Bih_code_old = (RPAD.Bih_code_old == null || RPAD.Bih_code_old.ToString().Trim() == "") ? "" : RPAD.Bih_code_old.ToString().Trim();
                    NodoResultado.Cis_code = int.Parse(RPAD.Cis_code.ToString());
                    NodoResultado.Cis_caseCode = (RPAD.Cis_caseCode == null || RPAD.Cis_caseCode.ToString().Trim() == "") ? "" : RPAD.Cis_caseCode.ToString().Trim();
                    NodoResultado.Insurer = (RPAD.Insurer == null || RPAD.Insurer.ToString().Trim() == "") ? "" : RPAD.Insurer.ToString().Trim();
                    NodoResultado.Patient = (RPAD.Patient == null || RPAD.Patient.ToString().Trim() == "") ? "" : RPAD.Patient.ToString().Trim();
                    NodoResultado.Total = (RPAD.Total == null || RPAD.Total.ToString().Trim() == "") ? 0 : decimal.Parse(RPAD.Total.ToString());
                    NodoResultado.Balance = (RPAD.Balance == null || RPAD.Balance.ToString().Trim() == "") ? 0 : decimal.Parse(RPAD.Balance.ToString().Trim());
                    NodoResultado.AccidentDate = (RPAD.AccidentDate == null || RPAD.AccidentDate.ToString().Trim() == "") ? "" : RPAD.AccidentDate.ToString().Trim();
                    NodoResultado.ClosingDate = (RPAD.ClosingDate == null || RPAD.ClosingDate.ToString().Trim() == "") ? "" : RPAD.ClosingDate.ToString().Trim();
                    NodoResultado.PaidLevel = RPAD.PaidLevel.ToString().Trim();

                    Resultado.Add(NodoResultado);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;

        }



        // Este método devuelve un pago, es decir un conjunto de facturas que conforman un pago.
        public List<REMOVE_PAYMENT_Result> ListaPago(int CodigoPago = 0)
        {
            ObjectResult<REMOVE_PAYMENT_Result> ResultadoProcedimientoAlmacenado;
            List<REMOVE_PAYMENT_Result> Pago; // Un pago va contener una lista de facturas pagadas.

            ResultadoProcedimientoAlmacenado = (ObjectResult<REMOVE_PAYMENT_Result>)null;
            Pago = (List<REMOVE_PAYMENT_Result>)null; // Lo que retorna es un conjunto de facturas pagadas

            try
            {
                ResultadoProcedimientoAlmacenado = db.REMOVE_PAYMENT(CodigoPago);
                Pago = ResultadoProcedimientoAlmacenado.ToList();

                return Pago;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Este método recibe las facturas cuyos pagos se van a revertir en formato XML (nodos: Codigo de factura, monto).
        public void RevierteMontosEnFactura(string FacturasParaRevertirXML = "")
        {
            try
            {
                db.DELETE_UPDATE_BILLING_HEAD(FacturasParaRevertirXML);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Elimina el pago de la tabla "PayHead" y "PayBody".
        public void EliminaPago(int CodigoPago = 0)
        {
            try
            {
                db.CLEAR_ELIMINATION_PAY_BODY_HEAD(CodigoPago);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        public String buscaCoincidenciaTipoPago(string tipo_pago = "")  
        {
            try
            {
                
                string Resultado="";
                Resultado = db.SP_BUSCA_TIPO_PAGO(tipo_pago).FirstOrDefault();

                return Resultado;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public int registraTipoPago(string tipo_pago = "")
        {
            try
            {
                int Resultado = 0;
                //Resultado = db.SP_SAVE_TYPE_PAY(tipo_pago);
                // Resultado = db.SP_SAVE_TYPE_PAY(tipo_pago);

                return Resultado;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // Elimina el pago de la tabla "PayHead" y "PayBody".

        public Int32 EliminarTipoDePago(int CodigoTipoPago = 0)
        {
            try
            {

                Int32 Resultado =0;
                // Resultado = db.SP_DELETE_TYPE_PAY(CodigoTipoPago); // .FirstOrDefault();

                return Resultado;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}