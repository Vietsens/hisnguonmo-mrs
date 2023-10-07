using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTransaction;

namespace MRS.Processor.Mrs00613
{
    public partial class Mrs00613RDOManager : BusinessBase
    {
        public List<HIS_TRANSACTION_D> GetTransaction(Mrs00613Filter filter)
        {
            try
            {
                List<HIS_TRANSACTION_D> result = new List<HIS_TRANSACTION_D>();
                string query = "";
                query += "SELECT ";
                query += "TRAN.*, ";
                query += "TREA.IN_TIME, ";
                query += "BF.AMOUNT AS BILL_FUND_AMOUNT, ";
                query += "FU.FUND_NAME ";

                query += "FROM HIS_RS.HIS_TRANSACTION TRAN ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TRAN.TREATMENT_ID=TREA.ID  ";
                query += "JOIN HIS_RS.HIS_BILL_FUND BF ON TRAN.ID=BF.BILL_ID  ";
                query += "JOIN HIS_RS.HIS_FUND FU ON BF.FUND_ID=FU.ID  ";
                query += "WHERE 1=1 ";

                query += "AND TRAN.IS_DELETE=0 AND TRAN.IS_CANCEL IS NULL AND TRAN.TRANSACTION_TYPE_ID =3 ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >={0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME <{0} ", filter.TIME_TO);
                }
                if (filter.FUND_ID != null)
                {
                    query += string.Format("AND BF.FUND_ID ={0} ", filter.FUND_ID);
                }
               
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRANSACTION_D>(query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
      
    }
}
