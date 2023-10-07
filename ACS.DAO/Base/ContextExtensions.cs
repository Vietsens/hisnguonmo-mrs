using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ACS.DAO.Base
{
    public static class ContextExtensions
    {
        public static string GetTableName<T>(this System.Data.Entity.DbContext context) where T : class
        {
            System.Data.Objects.ObjectContext objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this System.Data.Objects.ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("FROM (?<table>.*) AS");
            System.Text.RegularExpressions.Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
}
