using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Sql;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00812
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00812RDO> Get(Mrs00812Filter filter)
        {
            List<Mrs00812RDO> result = new List<Mrs00812RDO>();
            CommonParam paramGet = new CommonParam();
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "tran.NUM_ORDER, \n";
                query += "tran.ACCOUNT_BOOK_ID, \n";
                query += "tran.PAY_FORM_ID, \n";
                query += "sesv.TDL_SERVICE_NAME, \n";
                query += "sv.PARENT_ID, \n";
                query += "sesv.AMOUNT, \n";
                query += "sesv.PRICE, \n";
                query += "sesv.AMOUNT * sesv.PRICE as THANHTIEN, \n";
                query += "trea.TDL_PATIENT_NAME,\n";
                query += "trea.TDL_PATIENT_CODE,\n";
                query += "trea.TREATMENT_CODE,\n";
                query += "trea.TDL_HEIN_CARD_NUMBER,\n";
                query += "depa.DEPARTMENT_CODE, \n";
                query += "depa.DEPARTMENT_NAME, \n";
                query += "tran.TRANSACTION_TIME INTRUCTION_TIME, \n";
                query += "tran.CASHIER_USERNAME, \n";
                query += "tran.CASHIER_LOGINNAME, \n";
                query += "trantype.TRANSACTION_TYPE_NAME \n";
                query += "from his_sere_serv sesv \n";
                query += "join his_sere_serv_bill bill on bill.SERE_SERV_ID = sesv.ID \n";
                query += "join his_transaction tran on tran.ID = bill.BILL_ID \n";
                query += "left join his_transaction_type trantype on trantype.id = tran.transaction_type_id \n";
                query += "join his_service_req sr on sr.ID = sesv.SERVICE_REQ_ID \n";
                query += "join his_service sv on sv.ID = sesv.SERVICE_ID \n";
                query += "left join his_service pr on pr.ID = sv.parent_id \n";
                query += "join his_treatment trea on trea.ID = tran.TREATMENT_ID \n";
                query += "join his_department depa on depa.ID = sesv.TDL_REQUEST_DEPARTMENT_ID \n";
                query += string.Format("where (sesv.tdl_service_type_id  = {0} OR pr.service_code = 'QD792.AN')\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN);
                query += "and tran.IS_CANCEL is null \n";
                query += "and bill.IS_CANCEL is null \n";
                query += string.Format("AND tran.TRANSACTION_TIME between {0} and {1} ", filter.TIME_FROM,filter.TIME_TO);
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("and tran.cashier_loginname in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.TRANSACTION_TYPE_IDs != null)
                {
                    query += string.Format("and tran.TRANSACTION_TYPE_ID in ('{0}')\n", string.Join("','", filter.TRANSACTION_TYPE_IDs));
                }
                else
                {
                    query += string.Format("and tran.TRANSACTION_TYPE_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT);
                }
                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("and tran.PAY_FORM_ID in ('{0}')\n", string.Join("','", filter.PAY_FORM_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<Mrs00812RDO>(paramGet, query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }
    }
}
