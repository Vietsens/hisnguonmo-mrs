using Inventec.Core;
using Oracle.DataAccess.Client;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
{
    partial class AnalysisDb : EntityBase
    {
        public DataTable GetDataTable(string query, ref List<string> errorMessage)
        {
            DataTable list = new DataTable();
            try
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    DBConnect dbConnect = new DBConnect(MRS.ConfigManager.ConfigUtil.DataSource, MRS.ConfigManager.ConfigUtil.IdDb, MRS.ConfigManager.ConfigUtil.PassDb);
                    if (dbConnect != null && dbConnect.OpenConnection())
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter())
                        {
                            try
                            {
                                Inventec.Common.Logging.LogSystem.Info("AnalysisDb query: " + query);
                                adapter.SelectCommand = new OracleCommand(query, dbConnect.connection);
                                DataSet Mydata = new DataSet();
                                adapter.Fill(Mydata);
                                list = Mydata.Tables[0];
                                dbConnect.CloseConnection();
                            }
                            catch (Exception ex)
                            {
                                dbConnect.CloseConnection();
                                Inventec.Common.Logging.LogSystem.Error(ex);

                                if (errorMessage == null)
                                    errorMessage = new List<string>();

                                errorMessage.Add(Translate.TranslateMessage(ex.Message));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return list;
        }

        public long? GetDataCount(string query)
        {
            long? result = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    DBConnect dbConnect = new DBConnect(MRS.ConfigManager.ConfigUtil.DataSource, MRS.ConfigManager.ConfigUtil.IdDb, MRS.ConfigManager.ConfigUtil.PassDb);
                    if (dbConnect.OpenConnection())
                    {
                        string countQuery = String.Format("select count(*) from ({0})", query);
                        OracleCommand cmd = new OracleCommand(countQuery, dbConnect.connection);
                        try
                        {
                            using (OracleDataReader dataReader = cmd.ExecuteReader())
                            {
                                long rs = 0;
                                bool outRs = false;
                                while (dataReader.Read())
                                {
                                    outRs = long.TryParse(dataReader.GetValue(0).ToString(), out rs);
                                }

                                if (outRs)
                                {
                                    result = rs;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Inventec.Common.Logging.LogSystem.Error(ex);
                        }
                        dbConnect.CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
