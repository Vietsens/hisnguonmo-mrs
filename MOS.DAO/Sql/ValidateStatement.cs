using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
{
    public static class ValidateStatement
    {
        private static string[] validateOut = new string[] { "insert", "update", "delete", "create", "drop", "replace", "index", "alter", "lock", "exec", "sqlexec", "execute", "immediate", "declare", "begin", "shutdown", "identity", "rename", "grant" };
        public static bool Valid(string sql)
        {
            bool result=true;
            try
            {
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new Exception("sql is null");
                    return false;
                }
                var statement = statementsSeperate(sql);
                if (statement == null)
                {
                    throw new Exception("Statement is null");
                    return false;
                }
                foreach (var item in statement)
                {
                    if(validateOut.Contains(item.ToLower()))
                    {
                        throw new Exception(string.Format("{0} \r\n Statement is {1}", sql,item));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private static string[] statementsSeperate(string sql)
        {
            string[] result = null;
            try
            {
                var fixedInput = System.Text.RegularExpressions.Regex.Replace(sql, "[^a-zA-Z0-9% ._]", " ");
                result = fixedInput.ToLower().Split(' ');
                result = result.Where(o => !String.IsNullOrWhiteSpace(o)).ToArray();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }
    }
}
