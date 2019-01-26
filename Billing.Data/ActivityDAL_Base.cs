

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Billing.Entity;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using Billing.Entity.Generics;


namespace Billing.Data
{
    public class Activitydalc : Singleton<Activitydalc>
    {
        private const String nombreprocedimiento = "_Activity";
        private const String NombreTabla = "Activity";
        private Database db = DatabaseFactory.CreateDatabase();

        public List<Activity> listartodos()
        {
            try
            {
                var coleccion = new List<Activity>();
                DbCommand SQL = db.GetStoredProcCommand(nombreprocedimiento);
                db.AddInParameter(SQL, "Tipoconsulta", DbType.Byte, 2);
                using (var lector = db.ExecuteReader(SQL))
                {
                    while (lector.Read())
                    {
                        coleccion.Add(new Activity
                        {
                            Act_code = lector.GetInt32(lector.GetOrdinal("Act_code")),
                            Act_code_old = lector.GetString(lector.GetOrdinal("Act_code_old")),
                            Act_description = lector.GetString(lector.GetOrdinal("Act_description")),
                            Aty_code = lector.GetInt32(lector.GetOrdinal("Aty_code")),
                            Act_price = lector.IsDBNull(lector.GetOrdinal("Act_price")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("Act_price"))

                        });
                    }
                }
                SQL.Dispose();
                return coleccion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public List<Activity> Getcoleccion(string wheresql, string orderbysql)
        {
            int totalRecordCount = -1;
            return Getcoleccion(wheresql, orderbysql, 0, int.MaxValue, totalRecordCount);
        }


        public virtual List<Activity> Getcoleccion(string wheresql, string orderbysql, int startIndex, int length, int totalRecordCount)
        {
            try
            {
                var coleccion = new List<Activity>();
                DbCommand SQL = db.GetSqlStringCommand(CreateGetCommand(wheresql, orderbysql));
                using (var lector = db.ExecuteReader(SQL))
                {
                    while (lector.Read())
                    {
                        coleccion.Add(new Activity
                        {
                            Act_code = lector.GetInt32(lector.GetOrdinal("Act_code")),
                            Act_code_old = lector.GetString(lector.GetOrdinal("Act_code_old")),
                            Act_description = lector.GetString(lector.GetOrdinal("Act_description")),
                            Aty_code = lector.GetInt32(lector.GetOrdinal("Aty_code")),
                            Act_price = lector.IsDBNull(lector.GetOrdinal("Act_price")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("Act_price"))

                        });
                    }
                }
                SQL.Dispose();
                return coleccion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<Activity> ObtenerPorAty_code(Int32 Aty_code)
        {
            try
            {
                DbCommand SQL = db.GetStoredProcCommand(nombreprocedimiento);
                db.AddInParameter(SQL, "Tipoconsulta", DbType.Byte, 10);
                db.AddInParameter(SQL, "Aty_code", DbType.Int32);
                var coleccion = new List<Activity>();
                using (var lector = db.ExecuteReader(SQL))
                {
                    while (lector.Read())
                    {
                        coleccion.Add(new Activity
                        {
                            Act_code = lector.GetInt32(lector.GetOrdinal("Act_code")),
                            Act_code_old = lector.GetString(lector.GetOrdinal("Act_code_old")),
                            Act_description = lector.GetString(lector.GetOrdinal("Act_description")),
                            Aty_code = lector.GetInt32(lector.GetOrdinal("Aty_code")),
                            Act_price = lector.IsDBNull(lector.GetOrdinal("Act_price")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("Act_price"))

                        });
                    }
                }
                SQL.Dispose();
                return coleccion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public virtual bool EliminarPorAty_code(Int32 Aty_code)
        {
            try
            {
                DbCommand SQL = db.GetStoredProcCommand(nombreprocedimiento);
                db.AddInParameter(SQL, "Tipoconsulta", DbType.Byte, 6);
                db.AddInParameter(SQL, "Aty_code", DbType.Int32);
                int huboexito = db.ExecuteNonQuery(SQL);
                if (huboexito == 0)
                {
                    throw new Exception("Error");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual Activity GetByPrimaryKey(Int32 Act_code)
        {
            try
            {
                DbCommand SQL = db.GetStoredProcCommand(nombreprocedimiento);
                db.AddInParameter(SQL, "Tipoconsulta", DbType.Byte, 3);
                db.AddInParameter(SQL, "Act_code", DbType.Int32, Act_code);
                var Activity = default(Activity);
                using (var lector = db.ExecuteReader(SQL))
                {
                    while (lector.Read())
                    {
                        Activity = new Activity
                        {
                            Act_code = lector.GetInt32(lector.GetOrdinal("Act_code")),
                            Act_code_old = lector.GetString(lector.GetOrdinal("Act_code_old")),
                            Act_description = lector.GetString(lector.GetOrdinal("Act_description")),
                            Aty_code = lector.GetInt32(lector.GetOrdinal("Aty_code")),
                            Act_price = lector.IsDBNull(lector.GetOrdinal("Act_price")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("Act_price"))

                        };
                    }
                }
                SQL.Dispose();
                return Activity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        protected virtual string CreateGetCommand(string whereSql, string orderBySql)
        {
            string sql = "SELECT * FROM [dbo].[Activity]";
            if ((whereSql != null) && (whereSql.Trim().Length > 0))
            {
                sql += " WHERE " + whereSql;
            }
            if ((orderBySql != null) && (orderBySql.Trim().Length > 0))
            {
                sql += " ORDER BY " + orderBySql;
            }
            return sql;
        }

        /*   public virtual int Insert(Activity act) {
   try  {
   DbCommand SQL=db.GetStoredProcCommand(nombreprocedimiento);
   db.AddInParameter(SQL, "Act_code_old", DbType.String, act.Act_code_old);
   db.AddInParameter(SQL, "Act_description", DbType.String, act.Act_description);
   db.AddInParameter(SQL, "Aty_code", DbType.Int32, act.Aty_code);
   db.AddInParameter(SQL, "Act_price", DbType.decimal, act.Act_price);
   db.AddInParameter(SQL, "Tipoconsulta", DbType.String, 4);
   int huboexito  = db.ExecuteNonQuery(SQL);
   if( huboexito == 0 ){
    throw new Exception("Error al agregar al"); }
    var numerogenerado=(int)db.GetParameterValue(SQL,"Act_code");
   SQL.Dispose();
   return numerogenerado;
   }
   catch ( Exception ex ) {
    throw new Exception(ex.Message);
   }
   }


           public virtual void Update(Activity Act) {
   try {
   DbCommand SQL=db.GetStoredProcCommand(nombreprocedimiento);
   db.AddInParameter(SQL, "Act_code", DbType.Int32, Act.Act_code);
   db.AddInParameter(SQL, "Act_code_old", DbType.String, Act.Act_code_old);
   db.AddInParameter(SQL, "Act_description", DbType.String, Act.Act_description);
   db.AddInParameter(SQL, "Aty_code", DbType.Int32, Act.Aty_code);
   db.AddInParameter(SQL, "Act_price", DbType.decimal, Act.Act_price);
   db.AddInParameter(SQL, "Tipoconsulta", DbType.String, 1);
   int huboexito  = db.ExecuteNonQuery(SQL);
   if( huboexito == 0 ){
    throw new Exception("Error"); }
   }
   catch ( Exception ex ) {
    throw new Exception(ex.Message);
   }
   }
   */

        public virtual bool DeleteByPrimaryKey(Int32 Act_code)
        {
            try
            {
                DbCommand SQL = db.GetStoredProcCommand(nombreprocedimiento);
                db.AddInParameter(SQL, "Act_code", DbType.Int32, Act_code);
                db.AddInParameter(SQL, "Tipoconsulta", DbType.String, 5);
                int huboexito = db.ExecuteNonQuery(SQL);
                if (huboexito == 0)
                {
                    throw new Exception("Error");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public virtual bool DeleteAll()
        {
            try
            {
                DbCommand SQL = db.GetStoredProcCommand(nombreprocedimiento);
                db.AddInParameter(SQL, "Tipoconsulta", DbType.String, 6);
                int huboexito = db.ExecuteNonQuery(SQL);
                if (huboexito == 0)
                {
                    throw new Exception("Error");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}

