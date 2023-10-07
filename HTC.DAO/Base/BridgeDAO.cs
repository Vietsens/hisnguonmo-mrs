using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using HTC.EFMODEL.DataModels;

namespace HTC.DAO.Base
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
                    using (var tranScope = new System.Transactions.TransactionScope())
                    {
                        using (var ctx = new AppContext())
                        {
                            var count = ctx.Database.ExecuteSqlCommand(sql);
                            result = true;
                        }
                        tranScope.Complete();
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
    }
}