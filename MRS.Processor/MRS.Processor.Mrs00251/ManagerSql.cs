using Inventec.Common.Logging;
using MOS.DAO.Sql;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Data;

namespace MRS.Processor.Mrs00251
{
	public class ManagerSql : BusinessBase
	{
		public DataTable GetSum(Mrs00251Filter filter, string query)
		{
			DataTable result = null;
			try
			{
				string arg_31_0 = query;
				long arg_0B_0 = filter.TIME_TO;
				object arg_31_1 = filter.TIME_TO.ToString();
				long arg_21_0 = filter.TIME_FROM;
				query = string.Format(arg_31_0, arg_31_1, filter.TIME_FROM.ToString());
				List<string> values = new List<string>();
				result = new SqlDAO().Execute(query, ref values);
				LogSystem.Info(string.Join(", ", values));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				result = null;
            }
            if (ContainsSelectClause(query) && NullOrEmptyRow(result))
            {
                result = new DataTable();
                DataRow row = result.NewRow();
                result.Rows.Add(row);
            }
            return result;
        }

        private bool NullOrEmptyRow(DataTable result)
        {
            return (result == null || result.Rows == null || result.Rows.Count == 0);
        }

        private bool ContainsSelectClause(string query)
        {
            return string.IsNullOrWhiteSpace(query) == false && query.ToUpper().Contains("SELECT");
        }
	}
}
