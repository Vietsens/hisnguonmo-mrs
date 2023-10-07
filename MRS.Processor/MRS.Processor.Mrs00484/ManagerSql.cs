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
using MOS.DAO.Sql;

namespace MRS.Processor.Mrs00484
{
    public class ManagerSql
    {
        public List<Mrs00484RDO> GetRdo(Mrs00484Filter filter)
        {
            try
            {
                List<Mrs00484RDO> list = new List<Mrs00484RDO>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "SS.ID SERE_SERV_ID, \n";
                query += "TREA.TDL_PATIENT_NAME VIR_PATIENT_NAME, \n";
                query += "TREA.TREATMENT_CODE, \n";
                query += "TREA.ID TREATMENT_ID, \n";
                query += "TREA.PATIENT_ID, \n";
                query += "TREA.IN_TIME, \n";
                query += "TREA.OUT_TIME, \n";
                query += "TREA.TDL_HEIN_CARD_NUMBER HEIN_CARD_NUMBER,\n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                query += "TREA.TDL_PATIENT_GENDER_ID, \n";
                query += "SS.SERVICE_REQ_ID, \n";
                query += "TREA.TDL_PATIENT_CODE PATIENT_CODE, \n";
                query += "SS.PATIENT_TYPE_ID, \n";
                query += "TREA.TDL_PATIENT_TYPE_ID, \n";
                query += "TREA.TDL_TREATMENT_TYPE_ID TREATMENT_TYPE_ID, \n";
                query += "TREA.TDL_PATIENT_ADDRESS VIR_ADDRESS, \n";
                query += "SS.VIR_PRICE, \n";
                query += "(case when SS.IS_EXPEND = 1 then SS.VIR_PRICE_NO_EXPEND else 0 end) as VIR_EXPEND_PRICE, \n";
                query += "TREA.ICD_CODE, \n";
                query += "TREA.ICD_NAME, \n";
                query += "TREA.ICD_TEXT ICD_MAIN_TEXT, \n";
                query += "TREA.IN_CODE, \n";
                query += "SR.REQUEST_ROOM_ID, \n";
                query += "SR.REQUEST_LOGINNAME, \n";
                query += "SR.REQUEST_USERNAME, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_NAME SERVICE_NAME, \n";
                query += "SR.EXECUTE_ROOM_ID, \n";
                query += "SAMP.SAMPLE_TIME, \n";
                query += "SAMP.APPROVAL_TIME, \n";
                query += "SAMP.RESULT_TIME, \n";
                query += "SAMP.BARCODE, \n";
                query += "SAMP.SAMPLE_USERNAME, \n";
                query += "SAMP.APPROVAL_USERNAME, \n";
                query += "SR.START_TIME, \n";
                query += "SR.FINISH_TIME FINISH_TIME_NUM, \n";
                query += "SR.INTRUCTION_TIME INTRUCTION_TIME_NUM, \n";
                query += "SR.SERVICE_REQ_CODE, \n";
                query += "SR.EXECUTE_LOGINNAME, \n";
                query += "SR.EXECUTE_USERNAME \n";
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                    query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
                }
                query += "LEFT JOIN LIS_RS.LIS_SAMPLE SAMP ON SAMP.SERVICE_REQ_CODE=SS.TDL_SERVICE_REQ_CODE \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";
                query += " JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00484') \n";
                query += "WHERE 1=1 ";
                //query += "AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL and sr.is_delete =0 \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL and sr.is_delete =0 \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SAMP.RESULT_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
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
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
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
                    query += string.Format("AND SR.INTRUCTION_TIME >={0} \n", filter.TIME_FROM);
                    query += string.Format("AND SR.INTRUCTION_TIME <{0} \n", filter.TIME_TO);
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("AND TREA.BRANCH_ID ={0} \n", filter.BRANCH_ID);
                }
                if (filter.PATIENT_TYPE_ID.HasValue)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                }
                if (filter.DEPARTMENT_ID.HasValue)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0} \n", filter.DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_ID.HasValue)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_ROOM_ID.HasValue)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.TREATMENT_TYPE_ID.HasValue)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.CHOOSE_TREATMENT_TYPE.HasValue)
                {
                    if (filter.CHOOSE_TREATMENT_TYPE == 1)
                    {
                        query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                    }
                    else
                    {
                        query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY);
                    }
                }
                if (filter.REPORT_TYPE_CAT_ID.HasValue)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} \n", filter.REPORT_TYPE_CAT_ID);
                }
                if (filter.REPORT_TYPE_CAT_IDs != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN ({0}) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND ss.patient_type_id IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND trea.tdl_patient_type_id IN ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                query += string.Format("AND SR.SERVICE_REQ_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN);
                LogSystem.Info("SQL: " + query);
                return new MyAppContext().GetSql<Mrs00484RDO>(query, new object[0]);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

        public List<SSE> GetRdoExt(Mrs00484Filter filter)
        {
            //IL_05f8: Unknown result type (might be due to invalid IL or missing references)
            try
            {
                List<SSE> list = new List<SSE>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "SS.ID SERE_SERV_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SSE.TEST_INDEX_ID, \n";
                query += "SSE.ID, \n";
                query += "SSE.MACHINE_ID, \n";
                query += "SSE.VALUE \n";
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_SERE_SERV_TEIN SSE ON SSE.SERE_SERV_ID=SS.ID  \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                    query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += "JOIN LIS_RS.LIS_SAMPLE SAMP ON SAMP.SERVICE_REQ_CODE=SS.TDL_SERVICE_REQ_CODE \n";
                }
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";
                query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00484') \n";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_NO_EXECUTE IS NULL and sr.is_delete =0 \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SAMP.RESULT_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
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
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
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
                    query += string.Format("AND SR.INTRUCTION_TIME >={0} \n", filter.TIME_FROM);
                    query += string.Format("AND SR.INTRUCTION_TIME <{0} \n", filter.TIME_TO);
                }
                if (filter.PATIENT_TYPE_ID.HasValue)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                }
                if (filter.DEPARTMENT_ID.HasValue)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0} \n", filter.DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_ID.HasValue)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_ROOM_ID.HasValue)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.TREATMENT_TYPE_ID.HasValue)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.CHOOSE_TREATMENT_TYPE.HasValue)
                {
                    if (filter.CHOOSE_TREATMENT_TYPE == 1)
                    {
                        query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                    }
                    else
                    {
                        query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY);
                    }
                }
                if (filter.REPORT_TYPE_CAT_ID.HasValue)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} \n", filter.REPORT_TYPE_CAT_ID);
                }
                if (filter.REPORT_TYPE_CAT_IDs != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN ({0}) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND ss.patient_type_id IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND trea.tdl_patient_type_id IN ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                query += string.Format("AND SR.SERVICE_REQ_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN);
                LogSystem.Info("SQL: " + query);
                return new MyAppContext().GetSql<SSE>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

    }
}
