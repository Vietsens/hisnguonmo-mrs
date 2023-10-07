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
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00581
{
    public class ManagerSql
    {
        public List<HIS_BILL_FUND> GetBillFund(Mrs00581Filter filter)
        {
            try
            {
                List<HIS_BILL_FUND> result = new List<HIS_BILL_FUND>();
                string query = "\n";
                query += "SELECT \n";
                query += "BF.* \n";
                
                query += "FROM HIS_RS.HIS_BILL_FUND BF \n";

                query += "WHERE 1=1 ";


                if (filter.FUND_IDs != null)
                {
                    query += string.Format("AND BF.FUND_ID IN ({0})\n", string.Join(",",filter.FUND_IDs));
                }
                 
                    Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<HIS_BILL_FUND>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }
        public List<BILL_BALANCE> GetBalance(Mrs00581Filter filter)
        {
            try
            {
                List<BILL_BALANCE> result = new List<BILL_BALANCE>();
                string query = string.Format(@"select 
tran.id,
sum(case when tran1.transaction_type_id=1 then tran1.amount when tran1.transaction_type_id =2 then -tran1.amount when tran1.transaction_type_id =3 then -nvl(tran1.kc_amount,0) else 0 end) hien_du
from his_transaction tran
left join his_transaction tran1 on tran1.treatment_id = tran.treatment_id and tran1.id<tran.id and (tran1.is_cancel is null or tran1.is_cancel = 1 and tran1.cancel_time>tran.transaction_time)
where 1=1
and (tran.transaction_time between {0} and {1} or tran.is_cancel = 1 and tran.cancel_time between {0} and {1})
group by
tran.id,
1
", filter.TIME_FROM,filter.TIME_TO);
               
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<BILL_BALANCE>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

    }
}
