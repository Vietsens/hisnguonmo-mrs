using Inventec.Core;
using LIS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00698
{
    class ManagerSql
    {
        internal List<Mrs00698RDO> GetTreatment(Mrs00698Filter filter)
        {
            List<Mrs00698RDO> result = new List<Mrs00698RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("-- thong tin HSDT from QCS_RS\n");

            query += string.Format("select \n");
            query += string.Format("trea.* \n");
            query += string.Format("from his_rs.his_treatment trea\n");
            query += string.Format("left join his_rs.his_ksk_contract kc on kc.id=trea.tdl_ksk_contract_id\n");
            query += string.Format("where 1=1\n");
            query += string.Format("and trea.in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            if (filter.KSK_CONTRACT_ID != null)
            {
                query += string.Format("and kc.id = {0}\n", filter.KSK_CONTRACT_ID);
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.MyAppContext().GetSql<Mrs00698RDO>(query);
            return result;
        }

        internal List<InfoSampleResult> GetInfoSampleResult(Mrs00698Filter filter)
        {
            List<InfoSampleResult> result = new List<InfoSampleResult>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("-- thong tin xet nghiem su dung user from QCS_RS\n");

            query += string.Format("select \n");
            query += string.Format("trea.treatment_code,\n");
            
            query += string.Format("ls.barcode,\n");
            query += string.Format("ls.id sample_id,\n");
            query += string.Format("ls.appointment_time,\n");
            query += string.Format("ls.REQUEST_DEPARTMENT_NAME,\n");
            query += string.Format("ls.REQUEST_ROOM_NAME,\n");
            query += string.Format("nvl(sr.INTRUCTION_TIME,0) as INTRUCTION_TIME,\n");
            query += string.Format("nvl(ls.RESULT_TIME,0) as RESULT_TIME,\n");
            query += string.Format("nvl(ls.START_TIME,0) as START_TIME,\n");
            query += string.Format("ls.RESULT_USERNAME,\n");
            query += string.Format("ls.RESULT_LOGINNAME ,\n");
            query += string.Format("lr.value,\n");
            query += string.Format("lr.test_index_code,\n");
            query += string.Format("lr.test_index_name,\n");
            query += string.Format("lr.sample_service_id,\n");
            query += string.Format("lss.service_code,\n");
            query += string.Format("lss.sample_service_stt_id,\n");
            query += string.Format("lss.service_num_order,\n");
            query += string.Format("lr.old_value\n");
            query += string.Format("from his_rs.his_treatment trea\n");
            query += string.Format("left join his_rs.his_ksk_contract kc on kc.id=trea.tdl_ksk_contract_id\n");
            query += string.Format("left join lis_rs.lis_sample ls on ls.treatment_code=trea.treatment_code\n");
            query += string.Format("left join his_rs.his_service_req sr on sr.service_req_code=ls.service_req_code\n");
            query += string.Format("left join lis_rs.lis_sample_service lss on lss.sample_id=ls.id\n");
            query += string.Format("left join lis_rs.lis_result lr on lr.sample_service_id=lss.id\n");
            query += string.Format("where 1=1\n");
            query += string.Format("and trea.in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            query += string.Format("and ls.id is not null\n");
            if (filter.KSK_CONTRACT_ID != null)
            {
                query += string.Format("and kc.id = {0}\n", filter.KSK_CONTRACT_ID);
            }
            if (filter.REQUEST_DEPARTMENT_ID != null)
            {
                query += string.Format("and sr.request_department_id = {0}\n", filter.REQUEST_DEPARTMENT_ID);
            }
            if (filter.EXECUTE_DEPARTMENT_ID != null)
            {
                query += string.Format("and sr.execute_department_id = {0}\n", filter.EXECUTE_DEPARTMENT_ID);
            }
            if (filter.REQUEST_DEPARTMENT_IDs != null)
            {
                query += string.Format("and sr.request_department_id in ({0})\n", string.Join(",",filter.REQUEST_DEPARTMENT_IDs));
            }
            if (filter.EXECUTE_DEPARTMENT_IDs != null)
            {
                query += string.Format("and sr.execute_department_id  in ({0})\n", string.Join(",",filter.EXECUTE_DEPARTMENT_IDs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.MyAppContext().GetSql<InfoSampleResult>(query);
            return result;
        }

        internal List<LIS_SAMPLE_SERVICE_STT> GetLisSampleServiceStt()
        {
            List<LIS_SAMPLE_SERVICE_STT> result = new List<LIS_SAMPLE_SERVICE_STT>();
            try
            {
                string query = "SELECT * FROM LIS_SAMPLE_SERVICE_STT ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var sample = new LIS.DAO.Sql.SqlDAO().GetSql<LIS_SAMPLE_SERVICE_STT>(query);
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
