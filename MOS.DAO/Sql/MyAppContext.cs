using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(string EntitiesName)
            : base(string.Format("name={0}", EntitiesName))
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }
        public MyAppContext()
        {

        }

        public List<RAW> GetSql<RAW>(string sql, params object[] parameters)
        {
            List<RAW> result = null;
            try
            {
                if (string.IsNullOrWhiteSpace(sql))
                {
                    throw new Exception("sql null");
                }
                string entities = GetEntitiesName.Get(sql);

                EditStatement.Replace(ref sql);

                if (!ValidateStatement.Valid(sql))
                {
                    return null;
                }
                if (string.IsNullOrWhiteSpace(entities))
                {
                    throw new Exception("entities null");
                }

                if (!String.IsNullOrEmpty(sql))
                {
                    using (var ctx = new MyAppContext(entities))
                    {
                        result = ctx.Database.SqlQuery<RAW>(sql, parameters).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }
        public DataTable GetSqlToDataTable(string sql, ref List<string> errorMessage)
        {
            DataTable result = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    result = new DataTable();
                    string entities = GetEntitiesName.Get(sql);

                    EditStatement.Replace(ref sql);

                    if (!ValidateStatement.Valid(sql))
                    {
                        return null;
                    }
                    if (string.IsNullOrWhiteSpace(entities))
                    {
                        throw new Exception("entities null");
                    }
                    using (var context = new MyAppContext(entities))
                    {

                        context.Database.Connection.Open();
                        var cmd = context.Database.Connection.CreateCommand();

                        var transaction = context.Database.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
                        // Assign transaction object for a pending local transaction
                        cmd.Transaction = transaction;

                        try
                        {
                            cmd.CommandText = sql;
                            Inventec.Common.Logging.LogSystem.Info("Start query: " + sql);
                            var reader = cmd.ExecuteReader();
                            do
                            {
                                result.Load(reader);
                            } while (!reader.IsClosed);
                        }
                        catch (Exception ex)
                        {
                            //Inventec.Common.Logging.LogSystem.Error(ex);
                            if (errorMessage == null)
                                errorMessage = new List<string>();
                            errorMessage.Add(ex.ToString());
                        }
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception)
                        {
                            
                        }
                            
                            context.Database.Connection.Close();
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }
    }
}
