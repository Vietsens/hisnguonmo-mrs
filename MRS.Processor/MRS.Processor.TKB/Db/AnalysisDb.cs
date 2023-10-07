using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.TKB.Db
{
    public class AnalysisDb
    {
        private DBConnect dbConnect;
        private string querry;

        public AnalysisDb(string query)
        {
            if (!String.IsNullOrWhiteSpace(query))
            {
                string entities = GetEntitiesName.Get(query);

                if (!String.IsNullOrWhiteSpace(entities))
                {
                    dbConnect = new DBConnect(entities);
                    this.querry = query;
                }
            }
        }

        public DataTable GetDataSql(ref List<string> errorMessage)
        {
            DataTable result = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(querry))
                {
                    if (dbConnect != null && dbConnect.OpenConnection())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter())
                        {
                            try
                            {
                                Inventec.Common.Logging.LogSystem.Info("AnalysisDb query: " + querry);
                                adapter.SelectCommand = new OracleCommand(querry, dbConnect.connection);
                                DataSet Mydata = new DataSet();
                                adapter.Fill(Mydata);
                                result = Mydata.Tables[0];
                                dbConnect.CloseConnection();
                            }
                            catch (Exception ex)
                            {
                                dbConnect.CloseConnection();
                                Inventec.Common.Logging.LogSystem.Error(ex);

                                if (errorMessage == null)
                                    errorMessage = new List<string>();

                                errorMessage.Add(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("khong ket noi duoc db");
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            finally
            {
                if (dbConnect != null) dbConnect.CloseConnection();
            }
            return result;
        }
    }
}
