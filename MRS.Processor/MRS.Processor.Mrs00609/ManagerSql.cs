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
using System.Reflection;

namespace MRS.Processor.Mrs00609
{
    public partial class ManagerSql
    {
        public List<Mrs00609RDO> GetTreatment(Mrs00609Filter filter)
        {
            List<Mrs00609RDO> result = null;
            try
            {
                string query = "--Thong ke benh nhan su dung dich vu\n";
                query += "select \n";
                query += "distinct \n";
                query += "trea.id TREATMENT_ID, \n";
                query += "trea.TREATMENT_CODE, \n";
                query += "trea.TDL_PATIENT_CODE PATIENT_CODE, \n";
                query += "trea.TDL_PATIENT_ADDRESS ADDRESS, \n";
                query += "trea.TDL_HEIN_CARD_NUMBER HEIN_CARD_NUMBER, \n";
                query += "trea.TDL_PATIENT_NAME PATIENT_NAME, \n";
                query += "trea.TDL_PATIENT_DOB, \n";
                query += "trea.TDL_PATIENT_GENDER_NAME, \n";
                query += "trea.TREATMENT_DAY_COUNT, \n";
                query += "trea.TDL_PATIENT_TYPE_ID, \n";
                query += "trea.TDL_TREATMENT_TYPE_ID, \n";
                query += "trea.IN_TIME, \n";
                query += "trea.OUT_TIME, \n";
                query += "trea.FEE_LOCK_TIME, \n";
                query += "trea.ICD_NAME, \n";
                query += "trea.ICD_CODE, \n";
                query += "trea.ICD_TEXT, \n";
                query += "trea.ICD_SUB_CODE \n";

                query += "from \n";
                query += "his_rs.his_sere_serv ss \n";
                query += "join his_rs.his_treatment trea on trea.id = ss.tdl_treatment_id\n";
                query += "join his_rs.his_service_req sr on sr.id = ss.service_req_id \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID AND TRAN.IS_CANCEL IS NULL \n";
                query += "where ss.is_delete =0 and ss.is_no_execute is null \n";


                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO);
                }
                else
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);

                }
                if (filter.OTHER_PAY_SOURCE_IDs != null)
                {
                    query += string.Format("and ss.OTHER_PAY_SOURCE_ID in({0}) \n", string.Join(",", filter.OTHER_PAY_SOURCE_IDs));
                }

                if (filter.END_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and trea.LAST_DEPARTMENT_ID in({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
                }

                if (filter.ICD_CODEs != null)
                {
                    query += string.Format("and trea.ICD_CODE in('{0}') \n", string.Join("','", filter.ICD_CODEs));
                }

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.TDL_PATIENT_TYPE_ID in({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }

                if (filter.TDL_TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.TDL_TREATMENT_TYPE_ID in({0}) \n", string.Join(",", filter.TDL_TREATMENT_TYPE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00609RDO>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public List<SERE_SERV> GetSereServ(Mrs00609Filter filter)
        {
            List<SERE_SERV> result = null;
            try
            {

                string query = "--Thong ke dich vu cua benh nhan\n";
                query += "select \n";
                query += "ss.tdl_treatment_id, \n";
                query += "ss.tdl_service_code, \n";
                query += "ss.tdl_service_name, \n";
                query += "ss.service_id, \n";
                query += "ss.tdl_service_type_id, \n";
                query += "ss.other_pay_source_id, \n";
                query += "nvl(ss.tdl_hein_service_type_id,0) tdl_hein_service_type_id, \n";
                query += "sr.request_loginname, \n";
                query += "sr.request_username, \n";
                query += "sum(nvl(ss.VIR_TOTAL_PRICE,0)) VIR_TOTAL_PRICE,\n";
                query += "sum(nvl(ss.VIR_TOTAL_PATIENT_PRICE,0)) VIR_TOTAL_PATIENT_PRICE,\n";
                query += "sum(nvl(ss.VIR_TOTAL_HEIN_PRICE,0)) VIR_TOTAL_HEIN_PRICE,\n";
                query += "sum(nvl(ss.VIR_TOTAL_PATIENT_PRICE_BHYT,0)) VIR_TOTAL_PATIENT_PRICE_BHYT,\n";
                query += "sum(ss.amount*nvl(ss.OTHER_SOURCE_PRICE,0)) TOTAL_OTHER_SOURCE_PRICE\n";


                query += "from \n";
                query += "his_rs.his_sere_serv ss \n";
                query += "join his_rs.his_treatment trea on trea.id = ss.tdl_treatment_id\n";
                query += "join his_rs.his_service_req sr on sr.id = ss.service_req_id \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID AND TRAN.IS_CANCEL IS NULL \n";
                query += "where ss.is_delete =0 and ss.is_no_execute is null \n";


                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO);
                }
                else
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.OUT_TIME_FROM, filter.OUT_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);

                }
                if (filter.OTHER_PAY_SOURCE_IDs != null)
                {
                    query += string.Format("and ss.OTHER_PAY_SOURCE_ID in({0}) \n", string.Join(",", filter.OTHER_PAY_SOURCE_IDs));
                }

                if (filter.END_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and trea.LAST_DEPARTMENT_ID in({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
                }

                if (filter.ICD_CODEs != null)
                {
                    query += string.Format("and trea.ICD_CODE in('{0}') \n", string.Join("','", filter.ICD_CODEs));
                }

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.TDL_PATIENT_TYPE_ID in({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }

                if (filter.TDL_TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.TDL_TREATMENT_TYPE_ID in({0}) \n", string.Join(",", filter.TDL_TREATMENT_TYPE_IDs));
                }

                query += "group by \n";
                query += "ss.tdl_treatment_id, \n";
                query += "ss.tdl_service_code, \n";
                query += "ss.tdl_service_name, \n";
                query += "ss.service_id, \n";
                query += "ss.other_pay_source_id, \n";
                query += "ss.tdl_service_type_id, \n";
                query += "ss.tdl_hein_service_type_id, \n";
                query += "sr.request_loginname, \n";
                query += "sr.request_username \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERE_SERV>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


    }
    public class SERE_SERV
    {
        public long? TDL_TREATMENT_ID { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public long SERVICE_ID { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public decimal VIR_TOTAL_PATIENT_PRICE { get; set; }
        public decimal VIR_TOTAL_HEIN_PRICE { get; set; }
        public decimal VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public long? OTHER_PAY_SOURCE_ID { get; set; }

        public long TDL_SERVICE_TYPE_ID { get; set; }

        public long TDL_HEIN_SERVICE_TYPE_ID { get; set; }
    }

}
