
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Billing.Entity;
using Billing.Data;
using Billing.Entity.Generics;

namespace Billing.Business
{
    public class ActivityCN : Singleton<ActivityCN>
    {

        public List<Activity> listartodos()
        {
            try
            {
                return Activitydalc.Instancia.listartodos();
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
                return Activitydalc.Instancia.GetByPrimaryKey(Act_code);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Activity> Getcoleccion(string wheresql, string orderbysql)
        {
            try
            {
                return Activitydalc.Instancia.Getcoleccion(wheresql, orderbysql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*  public virtual int Insert(Activity Activity)
          {
              try
              {
                  return Activitydalc.Instancia.Insert(Activity);
              }
              catch (Exception ex)
              {
                  throw new Exception(ex.Message);
              }
          }

          public virtual void Update(Activity Activity)
          {
              try
              {
                  Activitydalc.Instancia.Update(Activity);
              }
              catch (Exception ex)
              {
                  throw new Exception(ex.Message);
              }
          }
  */
    }
}

