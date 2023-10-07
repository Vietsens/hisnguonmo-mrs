using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00624
{
    internal class ManagerSql
    {
        internal List<Mrs00624RDO> GetTransaction(Mrs00624Filter filter)
        {
            List<Mrs00624RDO> result = new List<Mrs00624RDO>();
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "TREA.TDL_HEIN_CARD_NUMBER, \n";
                query += "TRAN.* \n";
                query += "FROM HIS_RS.V_HIS_TRANSACTION TRAN \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA \n";
                query += "WHERE TRAN.IS_CANCEL IS NOT NULL \n";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME >= {0} \n", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME < {0} \n", filter.TIME_TO);
                }
                if (filter.CANCEL_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CANCEL_LOGINNAME = '{0}' \n", filter.CANCEL_LOGINNAME);
                }
                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CANCEL_CASHIER_ROOM_ID = {0} \n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.TRANSACTION_TYPE_IDs != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TYPE_ID in ({0}) \n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00624RDO>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }

            return result;
        }
    }
}
