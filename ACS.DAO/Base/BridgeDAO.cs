using Inventec.Backend.EFMODEL;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

namespace ACS.DAO.Base
{
    /// <summary>   
    /// </summary>
    /// <typeparam name="DTO"></typeparam>
    /// <typeparam name="RAW"></typeparam>
    class BridgeDAO<RAW> : Inventec.Core.EntityBase
        where RAW : class
    {
        public BridgeDAO()
        {

        }

        public bool ExecuteSql(string sql)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(sql))
                {
                    using (var ctx = new AppContext())
                    {
                        var count = ctx.Database.ExecuteSqlCommand(sql);
                        if (count == 0)
                        {
                            Inventec.Common.Logging.LogSystem.Info("SQL thuc hien thanh cong tuy nhien khong co du lieu duoc tac dong." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql));
                        }
                        result = true;
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Logging(LogUtil.TraceDbException(ex), LogType.Error);
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public static List<RAW> QuerySql(string sql, params object[] parameters)
        {
            List<RAW> result = null;
            try
            {
                if (!String.IsNullOrEmpty(sql))
                {
                    using (var tranScope = new System.Transactions.TransactionScope())
                    {
                        using (var ctx = new AppContext())
                        {
                            result = ctx.Database.SqlQuery<RAW>(sql, parameters).ToList();
                        }
                        tranScope.Complete();
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
