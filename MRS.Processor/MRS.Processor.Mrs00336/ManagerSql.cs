
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00336
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_TRANSACTION> GetTransaction(Mrs00336Filter castFilter)
        {
            List<HIS_TRANSACTION> result = new List<HIS_TRANSACTION>();
            try
            {
                string query = "select repay.* \n";
                query += "from his_transaction repay \n";
                query += string.Format("left join his_transaction bill on bill.transaction_type_id ={0} and repay.treatment_id = bill.treatment_id \n", IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT);
                query += "where 1=1 ";
                query += string.Format("and repay.transaction_type_id= {0} \n", IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU);
                query += string.Format("AND  repay.transaction_time  >= {0} \n", castFilter.TIME_FROM);
                query += string.Format("AND  repay.transaction_time  <= {0} \n", castFilter.TIME_TO);
                query += "and bill.id is null \n";

                if (castFilter.ACCOUNT_BOOK_IDs != null)
                {
                    query += string.Format("AND  repay.account_book_id in ({0})\n", string.Join(",",castFilter.ACCOUNT_BOOK_IDs));
                }
                if (castFilter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("AND  repay.CASHIER_LOGINNAME in ('{0}')\n", string.Join("','", castFilter.CASHIER_LOGINNAMEs));
                }
                if (castFilter.LOGINNAMEs != null)
                {
                    query += string.Format("AND  repay.CASHIER_LOGINNAME in ('{0}')\n", string.Join("','", castFilter.LOGINNAMEs));
                }
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRANSACTION>(query);
                LogSystem.Info(query);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
