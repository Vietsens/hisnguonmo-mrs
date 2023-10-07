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
using LIS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00630
{
    public class ManagerSql
    {
        public List<SAMPLE> GetLisSample(Mrs00630Filter filter)
        {
            try
            {
                List<SAMPLE> result = new List<SAMPLE>();
                string query = "-- from Qcs\n";
                query += @"select 
sr.id service_req_id,
ls.CREATE_TIME,
ls.RESULT_TIME
from his_rs.his_treatment trea
join his_rs.his_service_req sr on sr.treatment_id = trea.id
join his_rs.his_sere_serv ss on ss.service_Req_id = sr.id
left
join his_rs.v_his_service_rety_cat src on src.service_id = ss.service_id and report_type_code = 'MRS00630'
left join lis_rs.lis_sample ls on ls.service_req_code = sr.service_req_code
where 1 = 1
and sr.is_delete = 0
and sr.is_no_execute is null
and ss.is_delete = 0
and ss.is_no_execute is null
and ls.result_time is not null 
";

                if (filter.FEE_LOCK_TIME_FROM > 0)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }

                if (filter.IN_TIME_FROM > 0)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1}  \n", filter.IN_TIME_FROM, filter.IN_TIME_TO);
                }

                if (filter.OUT_TIME_FROM > 0)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE = {2}\n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<SAMPLE>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new List<SAMPLE>();
            }
        }

    }
}
