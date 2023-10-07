using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00398
{
    public class ManagerSql
    {
        public List<TREATMENT_NUMFILM> GetSum(Mrs00398Filter filter, string query)
        {
            List<TREATMENT_NUMFILM> result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
//, (filter.CASHIER_LOGINNAME != null) ? "'" + filter.CASHIER_LOGINNAME+"'" : "''"

//, (filter.EXACT_CASHIER_ROOM_ID != null) ? filter.EXACT_CASHIER_ROOM_ID.ToString() : "''"

//, (filter.ACCOUNT_BOOK_ID != null) ? filter.ACCOUNT_BOOK_ID.ToString() : "''"

//, (filter.IS_BILL_NORMAL != null) ? filter.IS_BILL_NORMAL==true?"1":"2" : "''"

);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT_NUMFILM>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL:" +query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }




        internal List<MOS.EFMODEL.DataModels.HIS_PATIENT_CLASSIFY> GetPatientClassify()
        {
            string query = "select * from his_patient_classify";
            Inventec.Common.Logging.LogSystem.Info("SQL:" + query);
            return new MOS.DAO.Sql.SqlDAO().GetSql<MOS.EFMODEL.DataModels.HIS_PATIENT_CLASSIFY>(query);
        }
    }
}
