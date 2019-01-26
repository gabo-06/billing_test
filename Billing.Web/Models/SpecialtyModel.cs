using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Data.Objects;

namespace Billing.Web.Models
{
    public class SpecialtyModel
    {
        private OmnimedBDEntities db = new OmnimedBDEntities();

        /* FECHA: 28/09/2016
         * RAY DIAZ
         * LISTA EN UN SOLO PROCEDIMIENTO LAS ESPECIALIDADES DE DOCTORS Y ATTORNEYS
         */

        public List<Specialty> ListaEspecialidades()
        {
            List<Specialty> Lista = new List<Specialty>();
            ObjectResult<SP_LIST_SPECIALTY_FOR_DROPDOWNLIST_Result> Especialidades;

            try
            {
                Especialidades = db.SP_LIST_SPECIALTY_FOR_DROPDOWNLIST();

                foreach (var LectorCiudad in Especialidades)
                {
                    Specialty ObjEspecialidad = new Specialty();

                    ObjEspecialidad.Spe_code = LectorCiudad.Spe_code;
                    ObjEspecialidad.Spe_name = LectorCiudad.Spe_name;
                    ObjEspecialidad.Spe_type = LectorCiudad.Spe_type;

                    Lista.Add(ObjEspecialidad);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Lista;
        }

        public List<Specialty> ListaEspecialidadesDoctor()
        {
            List<Specialty> Lista = new List<Specialty>();
            ObjectResult<SP_LIST_MEDICAL_SPECIALTY_Result> Especialidades;

            try
            {
                Especialidades = db.SP_LIST_MEDICAL_SPECIALTY();

                foreach (var LectorCiudad in Especialidades)
                {
                    Specialty ObjEspecialidad = new Specialty();

                    ObjEspecialidad.Spe_code = LectorCiudad.Spe_code;
                    ObjEspecialidad.Spe_name = LectorCiudad.Spe_name;

                    Lista.Add(ObjEspecialidad);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Lista;
        }

        public List<Specialty> ListaEspecialidadesAbogado()
        {
            List<Specialty> Lista = new List<Specialty>();
            ObjectResult<SP_LIST_ATTORNEY_SPECIALTY_Result> Especialidades;

            try
            {
                Especialidades = db.SP_LIST_ATTORNEY_SPECIALTY();

                foreach (var LectorCiudad in Especialidades)
                {
                    Specialty ObjEspecialidad = new Specialty();

                    ObjEspecialidad.Spe_code = LectorCiudad.Spe_code;
                    ObjEspecialidad.Spe_name = LectorCiudad.Spe_name;

                    Lista.Add(ObjEspecialidad);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Lista;
        }

        public List<Specialty> Listar()
        {
            List<Specialty> Lista = new List<Specialty>();

            var Especialidades = db.SP_LIST_SPECIALTY();

            foreach (var LectorEspecialidad in Especialidades)
            {
                Specialty ObjEspecialidad = new Specialty();

                ObjEspecialidad.Spe_code = LectorEspecialidad.Spe_code;
                ObjEspecialidad.Spe_name = LectorEspecialidad.Spe_name;
                ObjEspecialidad.Spe_type = LectorEspecialidad.Spe_type;

                Lista.Add(ObjEspecialidad);
            }

            return Lista;
        }

        public List<SP_SAVE_SPECIALTY_Result> Insertar(Specialty Especialidad)
        {
            List<SP_SAVE_SPECIALTY_Result> Resultado = new List<SP_SAVE_SPECIALTY_Result>();
            ObjectResult<SP_SAVE_SPECIALTY_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_SAVE_SPECIALTY(Especialidad.Spe_name
                                                                            , Especialidad.Spe_type
                                                                            ,Especialidad.Spe_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_SAVE_SPECIALTY_Result NodoResultado = new SP_SAVE_SPECIALTY_Result();

                    NodoResultado.SpecialtyErrorCode = RPAD.SpecialtyErrorCode;
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

        public Specialty Buscar(int Codigo)
        {
            Specialty ObjEspecialidad = new Specialty();

            try
            {
                var Resultado = db.SP_FIND_SPECIALTY(Codigo);

                foreach (var NodoResultado in Resultado)
                {
                    ObjEspecialidad.Spe_code = NodoResultado.Spe_code;
                    ObjEspecialidad.Spe_name = NodoResultado.Spe_name;
                    ObjEspecialidad.Spe_type = NodoResultado.Spe_type;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjEspecialidad;
        }

        public List<SP_UPDATE_SPECIALTY_Result> Actualizar(Specialty Especialidad)
        {
            List<SP_UPDATE_SPECIALTY_Result> Resultado = new List<SP_UPDATE_SPECIALTY_Result>();
            ObjectResult<SP_UPDATE_SPECIALTY_Result> ResultadoProcedimientoAlmacenadoDuro;

            try
            {
                ResultadoProcedimientoAlmacenadoDuro = db.SP_UPDATE_SPECIALTY(Especialidad.Spe_code
                                                                            , Especialidad.Spe_name
                                                                            , Especialidad.Spe_type
                                                                            , Especialidad.Spe_operatorUser);

                foreach (var RPAD in ResultadoProcedimientoAlmacenadoDuro)
                {
                    SP_UPDATE_SPECIALTY_Result NodoResultado = new SP_UPDATE_SPECIALTY_Result();

                    NodoResultado.SpecialtyErrorCode = RPAD.SpecialtyErrorCode;
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
    }
}