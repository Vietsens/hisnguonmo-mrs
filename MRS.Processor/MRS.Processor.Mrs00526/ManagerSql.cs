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
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;
using System.Data;
using MRS.Processor.Mrs00526;

namespace MRS.Processor.Mrs00526
{
    public partial class ManagerSql : BusinessBase
    {

        public List<REQUEST_DEPARTMENT_ID> GetRequestDepartmentId(Mrs00526Filter filter)
        {
            List<REQUEST_DEPARTMENT_ID> result = null;
            try
            {
                string query = " -- khoa đang ở trước khi giao dịch\n";
                query += "SELECT \n";

                query += "tran.id TRAN_ID, \n";
                query += "dpt.department_id REQ_ID \n";

                query += "from his_transaction tran \n";
                query += "join his_treatment trea on trea.id=tran.treatment_id \n";
                query += "join his_department_tran dpt on dpt.treatment_id = tran.treatment_id \n";
                query += "where 1=1 and tran.is_cancel is null \n";

                query += string.Format("and trea.fee_lock_time between {0} and {1} and trea.is_active = 0\n", filter.TIME_FROM, filter.TIME_TO);
                query += "and dpt.department_in_time<=tran.transaction_time\n";
                query += "order by\n";
                query += "tran.id,\n";
                query += "dpt.department_in_time,\n";
                query += "dpt.id\n";

                result = new MOS.DAO.Sql.SqlDAO().GetSql<REQUEST_DEPARTMENT_ID>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

       
    }
}
