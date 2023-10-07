using Inventec.Common.Logging;
using LIS.EFMODEL.DataModels;
using MOS.DAO.Sql;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00617
{
    class ManagerSql
    {
        public List<DATA_GET> GetMain(Mrs00617Filter filter, List<long> SERVICE_TYPE_IDs)
        {
            List<DATA_GET> result = new List<DATA_GET>();

            //List<LIS_SAMPLE_TYPE> countLisSample = CountSample();
            string query = "";
            //if (countLisSample != null && countLisSample.Count > 0)
            //{
            //    query += "-- from Qcs\n";
            //}
            query += "SELECT \n";
            query += @"ss.ID,

    ss.SERVICE_ID,
    ss.SERVICE_REQ_ID,
    ss.PATIENT_TYPE_ID,
    ss.AMOUNT,
    ss.PRICE,
    ss.AMOUNT*nvl(ss.PRICE,0)*(1+nvl(ss.vat_ratio,0)) TOTAL_PRICE,
    ss.VIR_TOTAL_PRICE,
    ss.TDL_SERVICE_CODE,
    ss.TDL_SERVICE_NAME,
    ss.TDL_SERVICE_TYPE_ID,
    ss.TDL_REQUEST_ROOM_ID,
    ss.TDL_REQUEST_DEPARTMENT_ID,
    ss.TDL_EXECUTE_ROOM_ID,
    ss.TDL_REQUEST_LOGINNAME,
    ss.TDL_REQUEST_USERNAME,
    ss.TDL_TREATMENT_CODE,
    ss.TDL_INTRUCTION_TIME,
    ss.TDL_SERVICE_UNIT_ID,
    sr.CREATE_TIME,
    SSE.NUMBER_OF_FILM,
    SSE.MACHINE_ID,
     ";

            query += "(CASE WHEN EXISTS (SELECT 1 FROM HIS_RS.HIS_EXECUTE_ROOM WHERE ROOM_ID = SS.TDL_EXECUTE_ROOM_ID AND IS_EXAM=1) THEN 1 ELSE 0 END) AMOUNT_EXAM_ROOM, \n";

            query += "(CASE WHEN EXISTS (SELECT 1 FROM HIS_RS.HIS_SERE_SERV_TEIN WHERE SERE_SERV_ID = SS.ID AND VALUE IS NOT NULL) THEN 1 ELSE NULL END) HAS_TEST_VALUE, \n";
            query += "PR.ID PARENT_SERVICE_ID, \n";
            query += "PR.SERVICE_CODE PARENT_SERVICE_CODE, \n";
            query += "PR.SERVICE_NAME PARENT_SERVICE_NAME, \n";

            query += "TREA.IN_TIME, \n";
            query += "TREA.TDL_PATIENT_CODE, \n";
            query += "TREA.TDL_PATIENT_DOB, \n";
            query += "TREA.TDL_TREATMENT_TYPE_ID, \n";
            query += "TREA.TDL_PATIENT_TYPE_ID, \n";
            query += "TREA.TREATMENT_END_TYPE_ID, ";
            query += "nvl(TREA.CLINICAL_IN_TIME,0) as CLINICAL_IN_TIME, \n";
            query += "TREA.TDL_PATIENT_NAME, \n";
            query += "PATY.PATIENT_TYPE_CODE, \n";
            query += "PATY.PATIENT_TYPE_NAME \n";
            query += "FROM HIS_RS.HIS_SERE_SERV SS \n";

            query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID \n";

            query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
            query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID AND SSB.IS_CANCEL IS NULL \n";
            query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  AND TRAN.IS_CANCEL IS NULL \n";
            query += "LEFT JOIN HIS_RS.HIS_PATIENT_TYPE PATY ON PATY.ID=SS.PATIENT_TYPE_ID \n";
            query += "JOIN HIS_RS.HIS_SERVICE SV ON SV.ID = SS.SERVICE_ID\n";
            query += "LEFT JOIN HIS_RS.HIS_SERVICE PR ON SV.PARENT_ID = PR.ID\n";
            query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_EXT SSE ON SSE.SERE_SERV_ID = SS.ID\n";
            //if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
            //{
            //    query += "JOIN HIS_RS.HIS_SERE_SERV_TEIN SST ON SST.SERE_SERV_ID = SS.ID\n";
            //}
            query += "WHERE SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL \n";
            if (filter.TDL_PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
            {
                query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
            {
                query += string.Format("AND SR.FINISH_TIME BETWEEN {0} AND {1} AND SR.SERVICE_REQ_STT_ID = 3 \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
            {
                query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else
            {
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME >={0} \n", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME <{0} \n", filter.TIME_TO);
                }

            }

            if (filter.REQUEST_ROOM_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID IN ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
            }

            if (filter.EXECUTE_ROOM_IDs != null)
            {
                query += string.Format("AND SR.EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
            }

            if (filter.BRANCH_IDs != null)
            {
                query += string.Format("AND TREA.BRANCH_ID IN ({0}) \n", string.Join(",", filter.BRANCH_IDs));
            }

            if (filter.BRANCH_ID != null)
            {
                query += string.Format("AND trea.BRANCH_ID = {0} \n", filter.BRANCH_ID);
            }


            if (filter.REQUEST_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
            }

            if (filter.EXECUTE_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
            }

            if (filter.SERVICE_REQ_STT_IDs != null)
            {
                query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0}) \n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
            }

            if (SERVICE_TYPE_IDs != null)
            {
                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) \n", string.Join(",", SERVICE_TYPE_IDs));
            }


            if (filter.EXACT_CHILD_SERVICE_IDs != null)
            {
                query += string.Format("AND SS.SERVICE_ID IN ({0}) \n", string.Join(",", filter.EXACT_CHILD_SERVICE_IDs));
            }
            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            }
            if (filter.PATIENT_TYPE_ID!=null)
            {
                query += string.Format("AND SS.PATIENT_TYPE_ID = {0} \n", filter.PATIENT_TYPE_ID);
            }
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                query += string.Format("AND SV.PARENT_ID IN ({0}) \n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
            }
            if (filter.EXECUTE_MACHINE_IDs != null)
            {
                query += string.Format("AND SSE.MACHINE_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_MACHINE_IDs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

            result = new SqlDAO().GetSql<DATA_GET>(query);
            result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();

            return result;
        }

        private List<LIS_SAMPLE_TYPE> CountSample()
        {
            List<LIS_SAMPLE_TYPE> result = null;
            try
            {
                string query = "-- from Qcs\n";
                query += "select *  from lis_rs.lis_sample_type";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<LIS_SAMPLE_TYPE>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        public List<Mrs00617NewRDO> GetParentService()
        {
            List<Mrs00617NewRDO> result = new List<Mrs00617NewRDO>();
            string query = "select distinct pr.id parent_service_id, pr.service_code parent_service_code, pr.service_name parent_service_name from his_service pr join his_service sv on pr.id = sv.parent_id";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00617NewRDO>(query);
            Inventec.Common.Logging.LogSystem.Info("Finish Query ");

            return result;
        }

        public List<HIS_SERVICE> GetService()
        {
            List<HIS_SERVICE> result = new List<HIS_SERVICE>();
            string query = "select * from his_service";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<HIS_SERVICE>(query);
            Inventec.Common.Logging.LogSystem.Info("Finish Query ");

            return result;
        }
    }
    
}
