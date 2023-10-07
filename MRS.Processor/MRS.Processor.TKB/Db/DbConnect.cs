using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.TKB.Db
{
    class DBConnect
    {
        private static string CONNECTION_STRING = "Data Source={0};User Id={1};Password={2};";
        public OracleConnection connection;

        public DBConnect(string entitiesNames)
        {
            string conn = ConfigurationManager.ConnectionStrings[entitiesNames].ToString();
            var source = getValue(conn, "data source");
            var id = getValue(conn, "User Id");
            var password = getValue(conn, "Password");
            this.connection = new OracleConnection(string.Format(CONNECTION_STRING, source, id, password));
        }

        private string getValue(string conn, string key)
        {
            string result = "";
            if (!String.IsNullOrWhiteSpace(conn) && !String.IsNullOrWhiteSpace(key))
            {
                int indexKey = conn.ToLower().IndexOf(key.ToLower());
                int indexValue = indexKey + key.Length + 1; //+ index dấu =
                result = conn.Substring(indexValue, conn.IndexOf(";", indexKey) - indexValue);
            }
            return result;
        }

        public bool OpenConnection()
        {
            try
            {
                this.connection.Open();
                return true;
            }
            catch (OracleException ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                this.connection.Close();
                return true;
            }
            catch (OracleException ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
        }
    }
}
