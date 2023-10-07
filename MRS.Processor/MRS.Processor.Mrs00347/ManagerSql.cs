using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Logging;
using MOS.DAO.Sql;

namespace MRS.Processor.Mrs00347
{
    public partial class ManagerSql : BusinessBase 
    {
        internal List<HIS_TREATMENT_STT> GetTreatmentSTT()
        {
            List<HIS_TREATMENT_STT> result = new List<HIS_TREATMENT_STT>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT * FROM HIS_RS.HIS_TREATMENT_STT";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<HIS_TREATMENT_STT>(paramGet, query);
            return result;
        }
    }
}
