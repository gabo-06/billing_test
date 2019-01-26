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
    public class ReportModel
    {
        private string CadenaConexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> BuscarEntradasPorPaciente(string ApelliidoPaciente = "")
        {
            List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> Resultado = new List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>();
            ObjectResult<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_SEARCH_DATAENTRY_PATIENT(ApelliidoPaciente);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result NodoResultado = new SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result();

                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Pat_firstName = RPAD.Pat_firstName;
                    NodoResultado.Pat_lastName = RPAD.Pat_lastName;
                    NodoResultado.Pat_socialSecurityNumber = RPAD.Pat_socialSecurityNumber;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

        public List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> ReporteVistaEntradasPorPaciente(int CodigoCaso = 0,Int32 Bih_code = 0)
        {
            List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> Resultado = new List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result>();
            ObjectResult<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_VIEW_DATAENTRY_PATIENT(CodigoCaso, Bih_code );

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_VIEW_DATAENTRY_PATIENT_Result NodoResultado = new SP_REPORT_VIEW_DATAENTRY_PATIENT_Result();

                    NodoResultado.Dae_code = RPAD.Dae_code;
                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.Pat_socialSecurityNumber = RPAD.Pat_socialSecurityNumber;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.Dae_closedDate = RPAD.Dae_closedDate;
                    NodoResultado.Dae_date = RPAD.Dae_date;
                    NodoResultado.Dae_comment = RPAD.Dae_comment;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }


/////////////////////////////////////////////OPEN CASES BY REFERRAL DATE/////////////////////////////////////////////////////////////

        public List<SP_OPEN_CASES_BY_REFERRAL_DATE_Result> ReporteOpenCasesByReferralDate()
        {
            List<SP_OPEN_CASES_BY_REFERRAL_DATE_Result> Resultado = new List<SP_OPEN_CASES_BY_REFERRAL_DATE_Result>();
            ObjectResult<SP_OPEN_CASES_BY_REFERRAL_DATE_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_OPEN_CASES_BY_REFERRAL_DATE_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_OPEN_CASES_BY_REFERRAL_DATE();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_OPEN_CASES_BY_REFERRAL_DATE_Result NodoResultado = new SP_OPEN_CASES_BY_REFERRAL_DATE_Result();

                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Cis_referralDate = RPAD.Cis_referralDate;
                    NodoResultado.patient = RPAD.patient;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.adjuster = RPAD.adjuster;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.acuity = RPAD.acuity;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }


        /////////////////////////////////////////////REPORTE ADJUSTER FORMULARIO/////////////////////////////////////////////////////////////


        public List<SP_FIND_ADJ_REPORTS_Result> ListaFormularioAdjuster(String name = "")
        {
            List<SP_FIND_ADJ_REPORTS_Result> Resultado = new List<SP_FIND_ADJ_REPORTS_Result>();
            ObjectResult<SP_FIND_ADJ_REPORTS_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_FIND_ADJ_REPORTS_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_FIND_ADJ_REPORTS(name);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_FIND_ADJ_REPORTS_Result NodoResultado = new SP_FIND_ADJ_REPORTS_Result();

                    NodoResultado.Adj_code = RPAD.Adj_code;
                    NodoResultado.Adjuster = RPAD.Adj_firstName + " " + RPAD.Adj_lastName;   //AKI
                    NodoResultado.Adj_firstName = RPAD.Adj_firstName;   //AKI
                    NodoResultado.Adj_lastName = RPAD.Adj_lastName;
                    NodoResultado.Adj_phone = (RPAD.Adj_phone == null) ? "" : RPAD.Adj_phone.ToUpper();
                    NodoResultado.Adj_phoneExt = (RPAD.Adj_phoneExt == null) ? "" : RPAD.Adj_phoneExt.ToUpper();
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }

       
        /////////////////////////////////////////////REPORTE ADJUSTER/////////////////////////////////////////////////////////////


        public List<SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result> ReporteAdjuster(Int32 codigoAdjuster = 0)
        {
            List<SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result> Resultado = new List<SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result>();
            ObjectResult<SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORTS_VIEW_CASES_FOR_ADJUSTER(codigoAdjuster);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result NodoResultado = new SP_REPORTS_VIEW_CASES_FOR_ADJUSTER_Result();

                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.AccidentDate = RPAD.AccidentDate;
                    NodoResultado.Pat_socialSecurityNumberD = (RPAD.Pat_socialSecurityNumberD == null) ? "" : RPAD.Pat_socialSecurityNumberD.ToUpper();
                    NodoResultado.Cis_caseCode = RPAD.Cis_caseCode;
                    NodoResultado.Cis_status = RPAD.Cis_status;                    
                    NodoResultado.Ins_name = (RPAD.Ins_name == null) ? "" : RPAD.Ins_name.ToUpper();
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Resultado;
        }
///////////////////////////////////////////////PHYSICAL THERAPY REPORTS///////////////////////////////////////////////////////////

        public List<SP_PHYSICAL_THERAPY_REPORTS_Result> ReportePhysicalTherapy()
 {
     List<SP_PHYSICAL_THERAPY_REPORTS_Result> Resultado = new List<SP_PHYSICAL_THERAPY_REPORTS_Result>();
     ObjectResult<SP_PHYSICAL_THERAPY_REPORTS_Result> ResultadoProcedimientoAlmacenadoDuro;

     ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_PHYSICAL_THERAPY_REPORTS_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_PHYSICAL_THERAPY_REPORTS();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_PHYSICAL_THERAPY_REPORTS_Result NodoResultado = new SP_PHYSICAL_THERAPY_REPORTS_Result();

                    NodoResultado.Cis_referralDate = RPAD.Cis_referralDate;
                    NodoResultado.patient = RPAD.patient;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.adjuster = RPAD.adjuster;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.Cis_physicalTherapyCompany = RPAD.Cis_physicalTherapyCompany;                    

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
 }



/////////////////////////////////////////////// TRANSLATION REPORT///////////////////////////////////////////////////////////

        public List<SP_REPORT_TRANSLATION_Result> ReporteTranslation()
        {
            List<SP_REPORT_TRANSLATION_Result> Resultado = new List<SP_REPORT_TRANSLATION_Result>();
            ObjectResult<SP_REPORT_TRANSLATION_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_TRANSLATION_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_TRANSLATION();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_TRANSLATION_Result NodoResultado = new SP_REPORT_TRANSLATION_Result();

                    NodoResultado.Cis_referralDate= RPAD.Cis_referralDate;
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.Cis_translationCompany = RPAD.Cis_translationCompany ;
                    NodoResultado.adjuster = RPAD.adjuster;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.patient = RPAD.patient;
                    NodoResultado.Ins_name = RPAD.Ins_name ;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }

        
/////////////////////////////////////////////// TRANPORTATION REPORT///////////////////////////////////////////////////////////

        public List<SP_REPORT_TRANSPORTATION_Result> ReporteTransportation()
        {
            List<SP_REPORT_TRANSPORTATION_Result> Resultado = new List<SP_REPORT_TRANSPORTATION_Result>();
            ObjectResult<SP_REPORT_TRANSPORTATION_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_TRANSPORTATION_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_TRANSPORTATION();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_TRANSPORTATION_Result NodoResultado = new SP_REPORT_TRANSPORTATION_Result();

                    NodoResultado.Cis_referralDate = RPAD.Cis_referralDate;
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.patient = RPAD.patient;
                    NodoResultado.Ins_name = RPAD.Ins_name ;
                    NodoResultado.adjuster = RPAD.adjuster;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.Cis_transportationCompany = RPAD.Cis_transportationCompany;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }

        /////////////////////////////////////////////// OPEN_CASE_BY_SUPERVISOR REPORT///////////////////////////////////////////////////////////

        public List<SP_REPORT_OPEN_CASE_BY_SUPERVISOR_Result> ReporteOpenCaseBySupervisor(Int32 codigoSupervisor =0)
        {
            List<SP_REPORT_OPEN_CASE_BY_SUPERVISOR_Result> Resultado = new List<SP_REPORT_OPEN_CASE_BY_SUPERVISOR_Result>();
            ObjectResult<SP_REPORT_OPEN_CASE_BY_SUPERVISOR_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_OPEN_CASE_BY_SUPERVISOR_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_OPEN_CASE_BY_SUPERVISOR(codigoSupervisor);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_OPEN_CASE_BY_SUPERVISOR_Result NodoResultado = new SP_REPORT_OPEN_CASE_BY_SUPERVISOR_Result();

                    NodoResultado.Cis_referralDate = RPAD.Cis_referralDate;
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.patient = RPAD.patient;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.adjuster = RPAD.adjuster;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }

        /////////////////////////////////////////////// REPORT LIST OF PRESUMPTIOM CASE///////////////////////////////////////////////////////////

        public List<SP_REPORT_LIST_OF_PRESUMPTIOM_CASE_Result> ReporteListOfPresumtiomCase()
        {
            List<SP_REPORT_LIST_OF_PRESUMPTIOM_CASE_Result> Resultado = new List<SP_REPORT_LIST_OF_PRESUMPTIOM_CASE_Result>();
            ObjectResult<SP_REPORT_LIST_OF_PRESUMPTIOM_CASE_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_LIST_OF_PRESUMPTIOM_CASE_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_LIST_OF_PRESUMPTIOM_CASE();

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_LIST_OF_PRESUMPTIOM_CASE_Result NodoResultado = new SP_REPORT_LIST_OF_PRESUMPTIOM_CASE_Result();

                    NodoResultado.Cis_referralDate = RPAD.Cis_referralDate;
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.patient = RPAD.patient;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.adjuster = RPAD.adjuster;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.acuity = RPAD.acuity;

                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }

        /////////////////////////////////////////////// REPORT OPEN CASES By INSURER///////////////////////////////////////////////////////////
        public List<SP_REPORT_OPEN_CASES_BY_INSURER_Result> ReporteOpenCaseByInsurer(Int32 codigoInsurer = 0)
         {

             List<SP_REPORT_OPEN_CASES_BY_INSURER_Result> Resultado = new List<SP_REPORT_OPEN_CASES_BY_INSURER_Result>();
             ObjectResult<SP_REPORT_OPEN_CASES_BY_INSURER_Result> ResultadoProcedimientoAlmacenadoDuro;

             ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_OPEN_CASES_BY_INSURER_Result>)null;

             try
             {
                 ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_OPEN_CASES_BY_INSURER(codigoInsurer);

                 foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                 {
                     SP_REPORT_OPEN_CASES_BY_INSURER_Result NodoResultado = new SP_REPORT_OPEN_CASES_BY_INSURER_Result();


                     NodoResultado.Cis_code = RPAD.Cis_code;
                     NodoResultado.Patient = RPAD.Patient;
                     NodoResultado.Ins_name = RPAD.Ins_name;
                     NodoResultado.Adjuster = RPAD.Adjuster;
                     NodoResultado.ReferalDate  = RPAD.ReferalDate;
                     NodoResultado.AccidentDate = RPAD.AccidentDate;
                     

                     Resultado.Add(NodoResultado);
                 }

             }
             catch (Exception ex)
             {
                 throw ex;
             }
             return Resultado;
         }
        /////////////////////////////////////////////// REPORT REFERRAL BY SUPERVISOR///////////////////////////////////////////////////////////
        public List<PA_NEW_REFERRAL_BYSUPERVISOR_Result> ReporteReferralBySupervisor(Int32 codigoSupervisor = 0,Int32 dias =0 )
        {

            List<PA_NEW_REFERRAL_BYSUPERVISOR_Result> Resultado = new List<PA_NEW_REFERRAL_BYSUPERVISOR_Result>();
            ObjectResult<PA_NEW_REFERRAL_BYSUPERVISOR_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<PA_NEW_REFERRAL_BYSUPERVISOR_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.PA_NEW_REFERRAL_BYSUPERVISOR(codigoSupervisor,dias);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    PA_NEW_REFERRAL_BYSUPERVISOR_Result NodoResultado = new PA_NEW_REFERRAL_BYSUPERVISOR_Result();


                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Cis_referralDate = RPAD.Cis_referralDate;
                    NodoResultado.patient = RPAD.patient;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.adjuster = RPAD.adjuster;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.Cis_Injury = RPAD.Cis_Injury;
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }

        /////////////////////////////////////////////// REPORT DATA ENTRY PER USER ///////////////////////////////////////////////////////////
        public List<DATA_ENTRY_PER_USER_Result> ReporteDataEntryPerUser(DateTime fechaInicial, DateTime fechaFinal,Int32 user=0,Int32 filtro=0 )
        {

            List<DATA_ENTRY_PER_USER_Result> Resultado = new List<DATA_ENTRY_PER_USER_Result>();
            ObjectResult<DATA_ENTRY_PER_USER_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<DATA_ENTRY_PER_USER_Result>)null;

            try
            {
               // Int32 user = 32;
                ResultadoProcedimientoAlmacenadoDuro = db.DATA_ENTRY_PER_USER(fechaInicial, fechaFinal,user,filtro);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    DATA_ENTRY_PER_USER_Result NodoResultado = new DATA_ENTRY_PER_USER_Result();

                    NodoResultado.Dae_code = RPAD.Dae_code;
                    NodoResultado.Dae_date = RPAD.Dae_date;
                    NodoResultado.Act_description = RPAD.Act_description;
                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.Total = RPAD.Total;
                    NodoResultado.UserN = RPAD.UserN;
                    NodoResultado.Dae_hourAct = RPAD.Dae_hourAct;
                    NodoResultado.Comentary = RPAD.Comentary;
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }



        public List<SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result> ReporteDataEntryPerUserCount(String fechaInicial, String fechaFinal)
        {

            List<SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result> Resultado = new List<SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result>();
            ObjectResult<SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result>)null;
            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_LIST_COUNT_DATA_ENTRY_PER_USER( fechaInicial, fechaFinal);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result NodoResultado = new SP_LIST_COUNT_DATA_ENTRY_PER_USER_Result();

                    NodoResultado.Use_code = RPAD.Use_code;
                    NodoResultado.Total = RPAD.Total;  
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }

        /////////////////////////////////////////////// REPORT CLOSING 1///////////////////////////////////////////////////////////
        public List<SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result> ReporteClosing1(String fechaInicial)
        {

            List<SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result> Resultado = new List<SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result>();
            ObjectResult<SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result>)null;

            try
            {
                
                ResultadoProcedimientoAlmacenadoDuro = db.SP_CASE_INFORMATION_FOR_CLOSING_DATE(fechaInicial);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result NodoResultado = new SP_CASE_INFORMATION_FOR_CLOSING_DATE_Result();

                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.ADJUSTER = RPAD.ADJUSTER;
                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.accident_date = RPAD.accident_date;
                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Cis_status = RPAD.Cis_status;
                    
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }


        /////////////////////////////////////////////// REPORT CLOSING 2///////////////////////////////////////////////////////////
        public List<SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result> ReporteClosing2(String fechaInicial, String fechaFinal)
        {

            List<SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result> Resultado = new List<SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result>();
            ObjectResult<SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result>)null;

            try
            {
                //String estado = "";

                ResultadoProcedimientoAlmacenadoDuro = db.SP_CASE_INFORMATION_FOR_CLOSING_DATE2(fechaInicial, fechaFinal);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result NodoResultado = new SP_CASE_INFORMATION_FOR_CLOSING_DATE2_Result();

                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.ADJUSTER = RPAD.ADJUSTER;
                    NodoResultado.Patient = RPAD.Patient;
                    NodoResultado.accident_date = RPAD.accident_date;
                    NodoResultado.Dae_closedDate = RPAD.Dae_closedDate;

                    NodoResultado.Cis_code = RPAD.Cis_code;
                    //if (RPAD.Cis_status == true)
                    //    estado = "Open";
                    //else
                    //    estado = "Close";
                    NodoResultado.Cis_status = RPAD.Cis_status;
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }



        /////////////////////////////////////////////// REPORT DATA ENTRY SEARCH///////////////////////////////////////////////////////////
        public List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> Reporte_Data_Entry_Search(String paciente ="")
        {

            List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> Resultado = new List<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>();
            ObjectResult<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result>)null;
            try
            {
                //String estado = "";

                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_SEARCH_DATAENTRY_PATIENT(paciente);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result NodoResultado = new SP_REPORT_SEARCH_DATAENTRY_PATIENT_Result();

                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.Pat_firstName = RPAD.Pat_firstName;
                    NodoResultado.Pat_lastName = RPAD.Pat_lastName;
                    NodoResultado.Pat_socialSecurityNumber = RPAD.Pat_socialSecurityNumber;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.closingDate = RPAD.closingDate;
                    // NodoResultado.Status = RPAD.Status;
                        
                    Resultado.Add(NodoResultado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }


        /////////////////////////////////////////////// REPORT DATA ENTRY SEARCH///////////////////////////////////////////////////////////
        public List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> ReporteDataEntry(Int32 codigoCaso = 0, Int32 Bih_code = 0)
        {

            List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> Resultado = new List<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result>();
            ObjectResult<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_REPORT_VIEW_DATAENTRY_PATIENT_Result>)null;

            try
            {
                //String estado = "";

                ResultadoProcedimientoAlmacenadoDuro = db.SP_REPORT_VIEW_DATAENTRY_PATIENT(codigoCaso, Bih_code);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_REPORT_VIEW_DATAENTRY_PATIENT_Result NodoResultado = new SP_REPORT_VIEW_DATAENTRY_PATIENT_Result();
                    NodoResultado.Dae_date = RPAD.Dae_date;
                    NodoResultado.Dae_comment= RPAD.Dae_comment;
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////CASE BY INSURER//////////////////////////////////////////////////
        public List<SP_CASE_BY_INSURER_Result> ReporteCaseByInsurer(Int32 codigoInsurer = 0)
        {

            List<SP_CASE_BY_INSURER_Result> Resultado = new List<SP_CASE_BY_INSURER_Result>();
            ObjectResult<SP_CASE_BY_INSURER_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<SP_CASE_BY_INSURER_Result>)null;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_CASE_BY_INSURER(codigoInsurer);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_CASE_BY_INSURER_Result NodoResultado = new SP_CASE_BY_INSURER_Result();


                    NodoResultado.Cis_code = RPAD.Cis_code;
                    NodoResultado.patient = RPAD.patient;
                    NodoResultado.Ins_name = RPAD.Ins_name;
                    NodoResultado.Adjuster = RPAD.Adjuster;
                    NodoResultado.Cis_accidentDate = RPAD.Cis_accidentDate;
                    NodoResultado.Cis_referralDate = RPAD.Cis_referralDate;
                    NodoResultado.Cis_closedDate = RPAD.Cis_closedDate;
                    NodoResultado.Cis_caseCode = RPAD.Cis_caseCode;
                    NodoResultado.Supervisor = RPAD.Supervisor;
                    NodoResultado.Provider = RPAD.Provider;
                    Resultado.Add(NodoResultado);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultado;
        }


        /////////////////////////////////////////////// REPORT MONTHLY BILLING BY INSURER ///////////////////////////////////////////////////////////
        public List<PA_MONTHLY_BILLING_BY_INSURER_Result> ReporteMonthlyBillingByInsurer(Int32 codigoInsurer, String fechaInicial, String fechaFinal)
        {

            List<PA_MONTHLY_BILLING_BY_INSURER_Result> Resultado = new List<PA_MONTHLY_BILLING_BY_INSURER_Result>();
            ObjectResult<PA_MONTHLY_BILLING_BY_INSURER_Result> ResultadoProcedimientoAlmacenadoDuro;

            ResultadoProcedimientoAlmacenadoDuro = (ObjectResult<PA_MONTHLY_BILLING_BY_INSURER_Result>)null;

            var sw = 0;
            try
            {
            
                ResultadoProcedimientoAlmacenadoDuro = db.PA_MONTHLY_BILLING_BY_INSURER(codigoInsurer,fechaInicial, fechaFinal);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    PA_MONTHLY_BILLING_BY_INSURER_Result NodoResultado = new PA_MONTHLY_BILLING_BY_INSURER_Result();

                    NodoResultado.Paciente = RPAD.Paciente;
                    NodoResultado.Cis_caseCode = RPAD.Cis_caseCode;
                    NodoResultado.cierre = RPAD.cierre;
                    NodoResultado.Bih_billTotal = RPAD.Bih_billTotal;
                    NodoResultado.Bih_billPay = RPAD.Bih_billPay;
                    NodoResultado.Description = RPAD.Description;
                    Resultado.Add(NodoResultado);
                    sw = 1;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (sw == 1)
            {
                PA_MONTHLY_BILLING_BY_INSURER_Result NodoResultado = new PA_MONTHLY_BILLING_BY_INSURER_Result();

                NodoResultado.Paciente = ".";
                NodoResultado.Cis_caseCode = ".";
                NodoResultado.cierre = ".";
                NodoResultado.Bih_billTotal = 0;
                NodoResultado.Bih_billPay = 0;
                NodoResultado.Description = "0";
                Resultado.Add(NodoResultado);
            }
            return Resultado;
        }
        
        public List<ReporteCasosSinActividad> reporteDeCasosSinActividad()
        { 
            DataRowCollection ColeccionFilas;
            List<ReporteCasosSinActividad> Lista = new List<ReporteCasosSinActividad>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_REPORT_CASES_WITH_NO_ACTIVITY", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        // Comando.Parameters.Add(new SqlParameter("@FechaCierre", SqlDbType.Char, 10)).Value = FechaDeCierre;
                    
                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }
                 
                ColeccionFilas = dt.Rows;
                 
                foreach (DataRow fila in ColeccionFilas)
                {
                    ReporteCasosSinActividad NodoLista = new ReporteCasosSinActividad();
                    DateTime FA_DT; // Almacena la fecha de accidente que viene de la base.
                    string MesFA_STR = "";  // Mes de la fecha de accidente (string).
                    string DiaFA_STR = "";  // Dia de la fecha de accidente (string).
                    string AñoFA_STR = "";  // Año de la fecha de accidente (string).
                    DateTime FR_DT; // Almacena la fecha referencial que viene de la base.
                    string MesFR_STR = "";  // Mes de la fecha referencial (string).
                    string DiaFR_STR = "";  // Dia de la fecha referencial (string).
                    string AñoFR_STR = "";  // Año de la fecha referencial (string).

                    NodoLista.Paciente = fila.ItemArray.GetValue(dt.Columns["Patient"].Ordinal).ToString();
                    NodoLista.NewReferral = fila.ItemArray.GetValue(dt.Columns["NewReferal"].Ordinal).ToString();

                    FA_DT = Convert.ToDateTime(fila.ItemArray.GetValue(dt.Columns["AccidentDate"].Ordinal));
                    MesFA_STR = FA_DT.Month.ToString().PadLeft(2, '0');
                    DiaFA_STR = FA_DT.Day.ToString().PadLeft(2, '0');
                    AñoFA_STR = FA_DT.Year.ToString();
                    NodoLista.fechaDeAccidente = MesFA_STR + '/' + DiaFA_STR + '/' + AñoFA_STR;
                    
                    NodoLista.aseguradora = fila.ItemArray.GetValue(dt.Columns["Insurer"].Ordinal).ToString();

                    FR_DT = Convert.ToDateTime(fila.ItemArray.GetValue(dt.Columns["ReferralDate"].Ordinal));
                    MesFR_STR = FR_DT.Month.ToString().PadLeft(2, '0');
                    DiaFR_STR = FR_DT.Day.ToString().PadLeft(2, '0');
                    AñoFR_STR = FR_DT.Year.ToString();
                    NodoLista.fechaReferencial = MesFR_STR + '/' + DiaFR_STR + '/' + AñoFR_STR;
                    
                    NodoLista.diasPasadosDesdeUltimaEntrada = fila.ItemArray.GetValue(dt.Columns["DaysSinceLastEntry"].Ordinal).GetType().Equals(typeof(System.DBNull)) ? 0 : int.Parse(fila.ItemArray.GetValue(dt.Columns["DaysSinceLastEntry"].Ordinal).ToString());
                    NodoLista.supervisor = fila.ItemArray.GetValue(dt.Columns["Supervisor"].Ordinal).ToString();
                    
                    Lista.Add(NodoLista); 
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }        
    }
}


