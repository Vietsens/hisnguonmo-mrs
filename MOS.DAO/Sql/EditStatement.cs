using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
{
    public static class EditStatement
    {
        public static void Replace(ref string sql)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new Exception("sql is null");
                }
                Thread thr;
                thr = Thread.CurrentThread;
                int threadId = thr.ManagedThreadId;
                if (MOS.DAO.Sql.ProcessDelegate.EditSql != null && MOS.DAO.Sql.ProcessDelegate.EditSql.ContainsKey(threadId) && MOS.DAO.Sql.ProcessDelegate.EditSql[threadId]!=null)
                {
                    MOS.DAO.Sql.EditSql editSql = MOS.DAO.Sql.ProcessDelegate.EditSql[threadId];
                    List<string> InforReplaceSql = editSql();
                    if (InforReplaceSql == null || InforReplaceSql.Count == 0)
                    {
                        throw new Exception("InforReplaceSql is null or empty");
                    }
                    for (int i = 0; i < 1000; i++)
                    {
                        if (string.IsNullOrWhiteSpace(InforReplaceSql[3 * i]) || string.IsNullOrWhiteSpace(InforReplaceSql[3 * i + 1]))
                            break;
                        if (InforReplaceSql[3 * i].ToLower() == "replace")
                        {
                            ProcessReplace(ref sql, InforReplaceSql[3 * i + 1], InforReplaceSql[3 * i + 2]);
                        }    
                    }
                }
                LogSystem.Info("SQL: " + sql);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private static void ProcessReplace(ref string sql, string OldStatement, string NewStatement)
        {
            try
            {
                sql = sql.Replace(OldStatement, NewStatement);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
