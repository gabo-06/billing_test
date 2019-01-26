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
    public class CaseModel
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
                    NodoResultado.Patient2 = RPAD.LastNamePatient;
                    NodoResultado.Patient3 = RPAD.FirsNamePatient;
                    NodoResultado.Insurer = RPAD.Insurer;
                     
                    int Año = RPAD.AccidentDate.Year;
                    int Mes = RPAD.AccidentDate.Month;
                    int Dia = RPAD.AccidentDate.Day;
                    int Hora = RPAD.AccidentDate.Hour;
                    int Minuto = RPAD.AccidentDate.Minute;
                    int Segundo = RPAD.AccidentDate.Second;

                    DateTime fecha = new DateTime(Año
                                                , Mes
                                                , Dia
                                                , Hora
                                                , Minuto
                                                , Segundo);
                    NodoResultado.AccidentDate = fecha.ToString("MM/dd/yyyy");
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

        /*
        public int LeerCaso()
        {
            SP_CHECK_AVAILABILITY_CASE_Result Resultado = new SP_CHECK_AVAILABILITY_CASE_Result();
            ObjectResult<SP_CHECK_AVAILABILITY_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;            

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ResultadoProcedimientoAlmacenadoDuro = db.SP_CHECK_AVAILABILITY_CASE(CodigoCasoQueSeraLeido);

                    foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                    {
                        Resultado.Busy = RPAD.Busy;
                        Resultado.User = RPAD.User;
                    }
                    

                    scope.Complete();
                }

                
            }
            catch (TransactionAbortedException ex)
            {
                throw ex;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }
        */

        /* (1) */
        // Este procedimiento verifica la disponibilidad de un caso para poder ser leído, 
        // es decir verifica si el caso está siendo leído por otro usuario.
        // En caso de estar ocupado, devuelve 1 y el nombre del usuario loeguado que tiene cargado el caso y 
        // en caso de estar desocupado devuelve 0 y una cadena vacía.
        public DataTable[] VerificarDisponibilidadCaso(int CodigoCasoSeleccionado = 0, int CodigoUsuarioQueLee = 0)
        // public SP_CHECK_AVAILABILITY_CASE_Result VerificarDisponibilidadCaso(int CodigoCasoSeleccionado = 0, int CodigoUsuarioQueLee = 0)
        {
            SP_CHECK_AVAILABILITY_CASE_Result Resultado = new SP_CHECK_AVAILABILITY_CASE_Result();
            ObjectResult<SP_CHECK_AVAILABILITY_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;
            DataSet dts = new DataSet();

            try
            {
                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_CHECK_AVAILABILITY_CASE", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Cis_code", SqlDbType.Int)).Value = CodigoCasoSeleccionado;
                        Comando.Parameters.Add(new SqlParameter("@Cis_loggedUserCode", SqlDbType.Int)).Value = CodigoUsuarioQueLee;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dts);
                        }
                    }
                }

                DataTable[] ArregloTablas = new DataTable[] { dts.Tables[0], dts.Tables[1] };

                return ArregloTablas;
                
                /*
                ResultadoProcedimientoAlmacenadoDuro = db.SP_CHECK_AVAILABILITY_CASE(CodigoCasoSeleccionado
                                                                                        , CodigoUsuarioQueLee);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    Resultado.Busy = RPAD.Busy;
                    Resultado.User = RPAD.User;
                }

                return Resultado;
                */
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> obtener_data_inicial_case_info()
        {
            List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> Resultado = new List<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result>();
            ObjectResult<SP_LIST_INITIALDATA_FOR_CASE_INFO_Result> ResultadoProcedimientoAlmacenadoDuro;
            
            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_INITIALDATA_FOR_CASE_INFO();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_INITIALDATA_FOR_CASE_INFO_Result NodoResultado = new SP_LIST_INITIALDATA_FOR_CASE_INFO_Result();
                    NodoResultado.Codigo = RPAD.Codigo;
                    NodoResultado.Direccion = RPAD.Direccion;
                    NodoResultado.NombreCompleto = RPAD.NombreCompleto;
                    NodoResultado.tipo = RPAD.tipo;
                    Resultado.Add(NodoResultado);
                }
                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* (2) */
        // Este procedimiento obtiene los datos del caso, en caso esté desocupado
        public Pcase ObtenerDatosCaso(int CodigoCasoALeer = 0) 
        {
            Pcase Resultado = new Pcase();


            ObjectResult<SP_GET_CASE_DATA_Result> ResultadoProcedimientoAlmacenadoDuro;
            DateTime AccidentDate;
            DateTime CaseReferral;
            DateTime Birthday;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_GET_CASE_DATA(CodigoCasoALeer);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    // SP_GET_CASE_DATA_Result NodoResultado = new SP_GET_CASE_DATA_Result();

                    AccidentDate = Convert.ToDateTime(RPAD.CaseAccidentDate);
                    CaseReferral = Convert.ToDateTime(RPAD.CaseReferralDate);
                    Birthday = Convert.ToDateTime(RPAD.Birthday);

                    Resultado.CaseCode = RPAD.CaseCode;
                    Resultado.PatientCode = RPAD.PatientCode;
                    Resultado.AdjusterCode = RPAD.AdjusterCode;
                    Resultado.InsurerCode = RPAD.InsurerCode;
                    Resultado.ProviderCode = RPAD.ProviderCode;
                    Resultado.CasePrice = RPAD.CasePrice;
                    Resultado.CaseAccidentDate = AccidentDate.ToString("MM/dd/yyyy");
                    Resultado.CaseCaseCode = RPAD.CaseCaseCode;
                    Resultado.CaseReferralDate = CaseReferral.ToString("MM/dd/yyyy");
                    Resultado.CaseTransportation = RPAD.CaseTransportation;
                    Resultado.CaseComment = RPAD.CaseComment;
                    Resultado.CaseInjury = RPAD.CaseInjury;
                    Resultado.CaseStatus = RPAD.CaseStatus;                    
                    Resultado.Patient = RPAD.Patient;
                    Resultado.Birthday = Birthday.ToString("MM/dd/yyyy");
                    Resultado.Provider = RPAD.Provider;
                    Resultado.Adjuster = RPAD.Adjuster;
                    Resultado.AdjusterPhone = RPAD.AdjusterPhone;
                    Resultado.AdjusterPhoneExt = RPAD.AdjusterPhoneExt;                    
                    Resultado.Insurer = RPAD.Insurer;
                    Resultado.InsurerAddress = RPAD.InsurerAddress;
                    Resultado.InsurerCity = RPAD.InsurerCity;
                    Resultado.InsurerState = RPAD.InsurerState;
                    Resultado.InsurerZipCode = RPAD.InsurerZipCode;
                    Resultado.InsurerZipCodeExt = RPAD.InsurerZipCodeExt;
                    Resultado.InsurerPhone = RPAD.InsurerPhone;
                    Resultado.InsurerPhoneExt = RPAD.InsurerPhoneExt;
                    Resultado.InsurerFax = RPAD.InsurerFax;                    
                    Resultado.CaseTransportationCompany = RPAD.CaseTransportationCompany;
                    Resultado.CaseTranslation = RPAD.CaseTranslation;
                    Resultado.CaseTranslationCompany = RPAD.CaseTranslationCompany;
                    Resultado.CasePhysicalTherapy = RPAD.CasePhysicalTherapy;
                    Resultado.CasePhysicalTherapyCompany = RPAD.CasePhysicalTherapyCompany;
                    Resultado.CasePermisionContact = RPAD.CasePermisionContact;
                    Resultado.CasePermisionContactCompany = RPAD.CasePermisionContactCompany;
                    Resultado.CaseLongshore = RPAD.CaseLongshore;
                    Resultado.CaseCompanyStatus = RPAD.CaseCompanyStatus;
                    Resultado.CaseOther = RPAD.CaseOther;
                    Resultado.CaseOtherText = RPAD.CaseOtherText;
                    Resultado.CaseContact1 = RPAD.CaseContact1;
                    Resultado.CaseContact2 = RPAD.CaseContact2;
                    Resultado.CaseSupervisorCode = RPAD.CaseSupervisorCode;
                    Resultado.CasePresumption = RPAD.CasePresumption;
                    Resultado.CaseAcuity = RPAD.CaseAcuity;
                    Resultado.CaseSupervisor = RPAD.CaseSupervisor;

                    // Resultado.
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }

            return Resultado; 
        }

        /* (3) */
        // Este procedimiento ocupa el caso leído y libera el caso cargado anteriormente.
        public void OcupaLiberaCasos(int CodigoCasoLeido = 0, int CodigoUsuarioQueLee = 0, int CodigoCasoALiberar = 0)
        {            
            try                                                                   
            {
                db.SP_OCCUPY_LIBERATE_CASES(CodigoCasoLeido
                                           ,CodigoUsuarioQueLee
                                           ,CodigoCasoALiberar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Obtiene el estado de un caso según un paciente.
        public bool obtenerEstadoDeCaso(int codigoDeCaso = 0)
        {
            try
            {
                ObjectResult<bool?> resultado = db.SP_GET_STATUS_OF_CASE(codigoDeCaso);

                bool estadoDeCaso = (bool)resultado.FirstOrDefault();

                // foreach (var item in resultado)
                // {
                //     estadoDeCaso = (bool)item;
                // }

                return estadoDeCaso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
          
        // Este procedimiento se usa cuando hay un caso cargado y se carga un mantenedor u otro proceso.
        public void LiberaCasoDeUsuarioActual(int CodigoUsuarioActual = 0)
        {
            try
            {
                db.SP_LIBERATE_CASE_CURRENT_USER(CodigoUsuarioActual);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // Libera todos los casos
        public void LiberaTodosLosCasos()
        {
            try
            {
                db.RELEASES_ALL_CASES();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SP_FIND_HOMONYM_CASES_Result> BuscarHomonimos(CaseInformationSheetHead Caso)
        {
            List<SP_FIND_HOMONYM_CASES_Result> Resultado = new List<SP_FIND_HOMONYM_CASES_Result>();
            ObjectResult<SP_FIND_HOMONYM_CASES_Result> ResultadoProcedimientoAlmacenadoDuro;
            ObjectParameter ParametroSalida = new ObjectParameter("ParametroSalida", typeof(bool)); // Variable que se crea para recibir el valor del parámetro de salida del procedimiento almacenado. Si encontró coincidencias las devuelve

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_FIND_HOMONYM_CASES(ParametroSalida
                                                                               ,Caso.Cis_code
                                                                               ,Caso.Cis_caseCode);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                { 
                    SP_FIND_HOMONYM_CASES_Result NodoResultado = new SP_FIND_HOMONYM_CASES_Result();

                    NodoResultado.Pat_firstName = RPAD.Pat_firstName;
                    NodoResultado.Pat_lastName = RPAD.Pat_lastName;
                    NodoResultado.Cis_caseCode = RPAD.Cis_caseCode;

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

        public List<SP_SAVE_CASE_Result> Insertar(CaseInformationSheetHead Caso)
        {
            List<SP_SAVE_CASE_Result> Resultado = new List<SP_SAVE_CASE_Result>();  
            ObjectResult<SP_SAVE_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_CASE(Caso.Pat_code
                                                                      ,Caso.Cis_accidentDate
                                                                      ,Caso.Cis_Injury
                                                                      ,Caso.Cis_caseCode
                                                                      ,Caso.Adj_code
                                                                      ,Caso.Ins_code
                                                                      ,Caso.Cis_contact1
                                                                      ,Caso.Cis_contact2
                                                                      ,Caso.Cis_price
                                                                      ,Caso.Cis_referralDate
                                                                      ,Caso.Pro_code
                                                                      ,Caso.Cis_caseSupervisor
                                                                      ,Caso.Cis_Longshore
                                                                      ,Caso.Cis_companyStatus
                                                                      ,Caso.Cis_other
                                                                      ,Caso.Cis_otherText
                                                                      ,Caso.Cis_presumption
                                                                      ,Caso.Cis_acuity
                                                                      ,Caso.Cis_translation
                                                                      ,Caso.Cis_transportation
                                                                      ,Caso.Cis_physicalTherapy
                                                                      ,Caso.Cis_permisionContact
                                                                      ,Caso.Cis_translationCompany
                                                                      ,Caso.Cis_transportationCompany
                                                                      ,Caso.Cis_physicalTherapyCompany
                                                                      ,Caso.Cis_permisionContactCompany
                                                                      , Caso.Use_Code
                                                                      ,Caso.Cis_comment);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_CASE_Result NodoResultado = new SP_SAVE_CASE_Result();

                    NodoResultado.CaseErrorCode = RPAD.CaseErrorCode;
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

        public List<SP_UPDATE_CASE_Result> Actualizar(CaseInformationSheetHead Caso)
        {
            List<SP_UPDATE_CASE_Result> Resultado = new List<SP_UPDATE_CASE_Result>();
            ObjectResult<SP_UPDATE_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;

            Debug.WriteLine(Caso.Cis_referralDate);

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_CASE(Caso.Cis_code
                                                                      , Caso.Pat_code
                                                                      , Caso.Cis_accidentDate
                                                                      , Caso.Cis_Injury
                                                                      , Caso.Cis_caseCode
                                                                      , Caso.Adj_code
                                                                      , Caso.Ins_code
                                                                      , Caso.Cis_contact1
                                                                      , Caso.Cis_contact2
                                                                      , Caso.Cis_price
                                                                      , Caso.Cis_referralDate
                                                                      , Caso.Pro_code
                                                                      , Caso.Cis_caseSupervisor
                                                                      , Caso.Cis_Longshore
                                                                      , Caso.Cis_companyStatus
                                                                      , Caso.Cis_other
                                                                      , Caso.Cis_otherText
                                                                      , Caso.Cis_presumption
                                                                      , Caso.Cis_acuity
                                                                      , Caso.Cis_translation
                                                                      , Caso.Cis_transportation
                                                                      , Caso.Cis_physicalTherapy
                                                                      , Caso.Cis_permisionContact
                                                                      , Caso.Cis_translationCompany
                                                                      , Caso.Cis_transportationCompany
                                                                      , Caso.Cis_physicalTherapyCompany
                                                                      , Caso.Cis_permisionContactCompany
                                                                      , Caso.Cis_comment);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_CASE_Result NodoResultado = new SP_UPDATE_CASE_Result();

                    NodoResultado.CaseErrorCode = RPAD.CaseErrorCode;
                    NodoResultado.ErrorMessage = RPAD.ErrorMessage.ToString();

                    Resultado.Add(NodoResultado);
                }

                return Resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int VerificaPrecio(int CodigoCaso = 0)
        {
            ObjectResult ResultadoProcedimientoAlmacenadoDuro;
            int CantidadCasos = 0;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_CHECK_PRICE(CodigoCaso);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    CantidadCasos = int.Parse(RPAD.ToString());
                }

                return CantidadCasos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   

    }
}