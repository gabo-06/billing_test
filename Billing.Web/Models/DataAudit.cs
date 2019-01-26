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
    public class DataAudit
    {
        private string CadenaConexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        private OmnimedBDEntities db = new OmnimedBDEntities();

        public List<TrazabilidadDeEntidad> trazabilidadPaciente(int codigoPaciente = 0)
        {
            DataRowCollection ColeccionFilas;
            List<TrazabilidadDeEntidad> Lista = new List<TrazabilidadDeEntidad>();            

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_PATIENT_AUDIT", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Pat_code", SqlDbType.Int)).Value = codigoPaciente;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    TrazabilidadDeEntidad NodoLista = new TrazabilidadDeEntidad();

                    NodoLista.codigoEntidad = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.campo = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.antiguoValor = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.nuevoValor = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.tipoOperacion = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.fechaAuditoria = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.usuarioOperador = Fila.ItemArray.GetValue(6).ToString();

                    Lista.Add(NodoLista);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TrazabilidadDeEntidad> trazabilidadMedico(int codigoMedico = 0)
        {
            DataRowCollection ColeccionFilas;
            List<TrazabilidadDeEntidad> Lista = new List<TrazabilidadDeEntidad>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_MEDICAL_AUDIT", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Med_code", SqlDbType.Int)).Value = codigoMedico;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    TrazabilidadDeEntidad NodoLista = new TrazabilidadDeEntidad();

                    NodoLista.codigoEntidad = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.campo = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.antiguoValor = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.nuevoValor = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.tipoOperacion = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.fechaAuditoria = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.usuarioOperador = Fila.ItemArray.GetValue(6).ToString();

                    Lista.Add(NodoLista);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TrazabilidadDeEntidad> trazabilidadAseguradora(int codigoAseguradora = 0)
        {
            DataRowCollection ColeccionFilas;
            List<TrazabilidadDeEntidad> Lista = new List<TrazabilidadDeEntidad>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_INSURER_AUDIT", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Ins_code", SqlDbType.Int)).Value = codigoAseguradora;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    TrazabilidadDeEntidad NodoLista = new TrazabilidadDeEntidad();

                    NodoLista.codigoEntidad = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.campo = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.antiguoValor = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.nuevoValor = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.tipoOperacion = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.fechaAuditoria = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.usuarioOperador = Fila.ItemArray.GetValue(6).ToString();

                    Lista.Add(NodoLista);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TrazabilidadDeEntidad> trazabilidadAbogado(int codigoAbogado = 0)
        {
            DataRowCollection ColeccionFilas;
            List<TrazabilidadDeEntidad> Lista = new List<TrazabilidadDeEntidad>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_ATTORNEY_AUDIT", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Att_code", SqlDbType.Int)).Value = codigoAbogado;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    TrazabilidadDeEntidad NodoLista = new TrazabilidadDeEntidad();

                    NodoLista.codigoEntidad = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.campo = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.antiguoValor = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.nuevoValor = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.tipoOperacion = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.fechaAuditoria = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.usuarioOperador = Fila.ItemArray.GetValue(6).ToString();

                    Lista.Add(NodoLista);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TrazabilidadDeEntidad> trazabilidadAjustador(int codigoAjustador = 0)
        {
            DataRowCollection ColeccionFilas;
            List<TrazabilidadDeEntidad> Lista = new List<TrazabilidadDeEntidad>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_ADJUSTER_AUDIT", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Adj_code", SqlDbType.Int)).Value = codigoAjustador;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    TrazabilidadDeEntidad NodoLista = new TrazabilidadDeEntidad();

                    NodoLista.codigoEntidad = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.campo = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.antiguoValor = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.nuevoValor = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.tipoOperacion = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.fechaAuditoria = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.usuarioOperador = Fila.ItemArray.GetValue(6).ToString();

                    Lista.Add(NodoLista);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TrazabilidadDeEntidad> trazabilidadProveedor(int codigoProveedor = 0)
        {
            DataRowCollection ColeccionFilas;
            List<TrazabilidadDeEntidad> Lista = new List<TrazabilidadDeEntidad>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_PROVIDER_AUDIT", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Pro_code", SqlDbType.Int)).Value = codigoProveedor;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    TrazabilidadDeEntidad NodoLista = new TrazabilidadDeEntidad();

                    NodoLista.codigoEntidad = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.campo = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.antiguoValor = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.nuevoValor = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.tipoOperacion = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.fechaAuditoria = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.usuarioOperador = Fila.ItemArray.GetValue(6).ToString();

                    Lista.Add(NodoLista);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TrazabilidadDeEntidad> trazabilidadActividad(int codigoActividad = 0)
        {
            DataRowCollection ColeccionFilas;
            List<TrazabilidadDeEntidad> Lista = new List<TrazabilidadDeEntidad>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_ACTIVITY_AUDIT", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Act_code", SqlDbType.Int)).Value = codigoActividad;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    TrazabilidadDeEntidad NodoLista = new TrazabilidadDeEntidad();

                    NodoLista.codigoEntidad = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.campo = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.antiguoValor = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.nuevoValor = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.tipoOperacion = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.fechaAuditoria = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.usuarioOperador = Fila.ItemArray.GetValue(6).ToString();

                    Lista.Add(NodoLista);
                }

                return Lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TrazabilidadDeEntidad> trazabilidadEspecialidad(int codigoEspecialidad = 0)
        {
            DataRowCollection ColeccionFilas;
            List<TrazabilidadDeEntidad> Lista = new List<TrazabilidadDeEntidad>();

            try
            {
                DataTable dt = new DataTable("data");

                using (SqlConnection Conexion = new SqlConnection(CadenaConexion))
                {
                    Conexion.Open();

                    using (SqlCommand Comando = new SqlCommand("SP_LIST_SPECIALTY_AUDIT", Conexion))
                    {
                        Comando.CommandType = System.Data.CommandType.StoredProcedure;
                        Comando.CommandTimeout = 480;
                        Comando.Parameters.Add(new SqlParameter("@Spe_code", SqlDbType.Int)).Value = codigoEspecialidad;

                        using (SqlDataAdapter Adaptador = new SqlDataAdapter(Comando))
                        {
                            Adaptador.Fill(dt);
                        }
                    }
                }

                ColeccionFilas = dt.Rows;

                foreach (DataRow Fila in ColeccionFilas)
                {
                    TrazabilidadDeEntidad NodoLista = new TrazabilidadDeEntidad();

                    NodoLista.codigoEntidad = int.Parse(Fila.ItemArray.GetValue(0).ToString());
                    NodoLista.campo = Fila.ItemArray.GetValue(1).ToString();
                    NodoLista.antiguoValor = Fila.ItemArray.GetValue(2).ToString();
                    NodoLista.nuevoValor = Fila.ItemArray.GetValue(3).ToString();
                    NodoLista.tipoOperacion = Fila.ItemArray.GetValue(4).ToString();
                    NodoLista.fechaAuditoria = Fila.ItemArray.GetValue(5).ToString();
                    NodoLista.usuarioOperador = Fila.ItemArray.GetValue(6).ToString();

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