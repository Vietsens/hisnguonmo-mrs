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

namespace MRS.Processor.Mrs00455
{
    public class ManagerSql
    {

        public List<Mrs00455RDO> GetRdo(Mrs00455Filter filter)
        {
            try
            {
                List<Mrs00455RDO> result = new List<Mrs00455RDO>();
                string query = "-- chi tiet dich vu\n";
                query += "SELECT \n";
                query += "SS.ID SERE_SERV_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_TYPE_ID, \n";
                query += "SS.TDL_HEIN_SERVICE_TYPE_ID, \n";
                query += "SS.PATIENT_TYPE_ID, \n";
                query += "SS.HEIN_CARD_NUMBER, \n";
                query += "SS.VIR_TOTAL_PRICE, \n";
                query += "SS.VIR_TOTAL_HEIN_PRICE, \n";
                query += "SS.VIR_TOTAL_PATIENT_PRICE, \n";
                query += "SS.VIR_TOTAL_PATIENT_PRICE_BHYT, \n";
                query += "SS.OTHER_PAY_SOURCE_ID, \n";
                query += "nvl(SS.OTHER_SOURCE_PRICE,0)*SS.AMOUNT TOTAL_OTHER_SOURCE_PRICE, \n";
                query += "(case when ss.is_expend=1 then nvl(SS.VIR_PRICE_NO_EXPEND,0)*SS.AMOUNT else 0 end) TOTAL_EXPEND_PRICE, \n";
                query += "SR.SERVICE_REQ_CODE, \n";
                query += "SR.REQUEST_ROOM_ID, \n";
                query += "SR.EXECUTE_ROOM_ID, \n";
                query += "SR.EXECUTE_DEPARTMENT_ID, \n";
                query += "SR.START_TIME, \n";
                query += "SR.FINISH_TIME, \n";
                query += "SR.INTRUCTION_TIME, \n";
                query += "SR.INTRUCTION_DATE, \n";
                query += "SR.EXECUTE_LOGINNAME, \n";
                query += "SR.EXECUTE_USERNAME, \n";
                query += "DHST.WEIGHT, \n";
                query += "DHST.HEIGHT, \n";
                query += "DHST.BLOOD_PRESSURE_MAX, \n";
                query += "DHST.BLOOD_PRESSURE_MIN, \n";
                query += "DHST.BREATH_RATE, \n";
                query += "DHST.CHEST, \n";
                query += "DHST.BELLY, \n";
                query += "DHST.PULSE, \n";
                query += "DHST.TEMPERATURE, \n";
                query += "DHST.SPO2, \n";
                query += "TRAN.TRANSACTION_CODE,\n";
                query += "TRAN.CASHIER_ROOM_ID,\n";
                query += "TRAN.NUM_ORDER BILL_NUM_ORDER,\n";
                query += "TRAN.CASHIER_LOGINNAME,\n";
                query += "TRAN.CASHIER_USERNAME,\n";
                query += "TRAN.EXEMPTION TOTAL_PRICE_EXEM,\n";
                query += "TREA.ID TREATMENT_ID, \n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                query += "TREA.TDL_PATIENT_CODE PATIENT_CODE, \n";
                query += "TREA.TDL_PATIENT_NAME PATIENT_NAME, \n";
                query += "TREA.TDL_PATIENT_PHONE PHONE, \n";
                query += "TREA.TDL_HEIN_MEDI_ORG_CODE MEDI_ORG_CODE, \n";
                query += "TREA.TDL_HEIN_MEDI_ORG_NAME MEDI_ORG_NAME, \n";
                query += "TREA.TDL_HEIN_CARD_NUMBER, \n";
                query += "TREA.IN_CODE, \n";
                query += "TREA.OUT_CODE, \n";
                query += "TREA.TREATMENT_END_TYPE_ID, \n";
                query += "TREA.TRANSFER_IN_CODE, \n";
                query += "TREA.TREATMENT_CODE, \n";
                query += "TREA.TDL_PATIENT_GENDER_ID, \n";
                query += "TREA.TDL_PATIENT_GENDER_NAME GENDER_NAME, \n";
                query += "TREA.ICD_CODE ICD_10, \n";
                query += "TREA.ICD_SUB_CODE, \n";
                query += "TREA.IN_TIME IN_TIME_NUM, \n";
                query += "TREA.OUT_TIME OUT_TIME_NUM, \n";
                query += "TREA.TDL_PATIENT_TYPE_ID, \n";
                query += "TREA.TDL_TREATMENT_TYPE_ID, \n";
                query += "TREA.TREATMENT_RESULT_ID, \n";
                query += "ROUND(TREA.FEE_LOCK_TIME,-6) FEE_LOCK_DATE,\n";
                query += "TREA.TDL_PATIENT_ADDRESS VIR_ADDRESS, \n";
                query += "TREA.TREATMENT_DAY_COUNT, \n";
                //query += "SV.SERVICE_CODE, \n";
                //query += "SV.SERVICE_NAME, \n";
                //query += "PR.SERVICE_CODE PR_SERVICE_CODE, \n";
                //query += "PR.SERVICE_NAME PR_SERVICE_NAME, \n";
                query += "SRC.CATEGORY_CODE\n";
                //query += "SVT.SERVICE_TYPE_CODE \n";
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_DHST DHST ON DHST.ID=SR.DHST_ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  and tran.is_cancel is null \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";
                //query += "JOIN HIS_RS.HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID \n";
                //query += "LEFT JOIN HIS_RS.HIS_SERVICE PR ON SV.PARENT_ID = PR.ID \n";
                query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00455') \n";
                //query += "JOIN HIS_RS.HIS_SERVICE_TYPE SVT ON SS.TDL_SERVICE_TYPE_ID=SVT.ID  \n";

                query += "WHERE 1=1 ";

                // điều kiện lọc theo sereserv
                filterSereServ(filter,ref query);

                //điều kiện lọc theo y lệnh
                filterServiceReq(filter, ref query);

                //điều kiện lọc theo giao dịch
                filterTransaction(filter, ref query);

                //điều kiện lọc theo điều trị
                filterTreatment(filter, ref query);
 
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<Mrs00455RDO>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }


        public List<HIS_SERE_SERV_EXT> GetSse(Mrs00455Filter filter)
        {
            try
            {
                List<HIS_SERE_SERV_EXT> result = new List<HIS_SERE_SERV_EXT>();
                string query = "-- thong tin thuc hien\n";
                query += "SELECT \n";
                query += "SSE.*\n";
                query += "FROM HIS_RS.HIS_SERE_SERV_EXT SSE \n";
                query += "JOIN HIS_RS.HIS_SERE_SERV SS ON SS.ID=SSE.SERE_SERV_ID  \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  and tran.is_cancel is null \n";
                query += "JOIN LATERAL(select 1 from HIS_RS.HIS_TREATMENT TREA where SS.TDL_TREATMENT_ID=TREA.ID) TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";
                query += "WHERE 1=1 ";

                // điều kiện lọc theo sereserv
                filterSereServ(filter, ref query);

                //điều kiện lọc theo y lệnh
                filterServiceReq(filter, ref query);

                //điều kiện lọc theo giao dịch
                filterTransaction(filter, ref query);

                //điều kiện lọc theo điều trị
                filterTreatment(filter, ref query);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<HIS_SERE_SERV_EXT>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

        public List<HIS_SERE_SERV_TEIN> GetSst(Mrs00455Filter filter)
        {
            try
            {
                List<HIS_SERE_SERV_TEIN> result = new List<HIS_SERE_SERV_TEIN>();
                string query = "-- thong tin xet nghiem\n";
                query += "SELECT \n";
                query += "SST.*\n";
                query += "FROM HIS_RS.HIS_SERE_SERV_TEIN SST \n";
                query += "JOIN HIS_RS.HIS_SERE_SERV SS ON SS.ID=SST.SERE_SERV_ID  \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  and tran.is_cancel is null \n";
                query += "JOIN LATERAL(select 1 from HIS_RS.HIS_TREATMENT TREA where SS.TDL_TREATMENT_ID=TREA.ID) TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";
                query += "WHERE 1=1 ";

                // điều kiện lọc theo sereserv
                filterSereServ(filter, ref query);

                //điều kiện lọc theo y lệnh
                filterServiceReq(filter, ref query);

                //điều kiện lọc theo giao dịch
                filterTransaction(filter, ref query);

                //điều kiện lọc theo điều trị
                filterTreatment(filter, ref query);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<HIS_SERE_SERV_TEIN>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }
        private void filterTreatment(Mrs00455Filter filter, ref string query)
        {
            if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == null || filter.INPUT_DATA_ID_TIME_TYPE == 0)
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);

            }
            if (filter.TDL_PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
            }
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            //if (filter.TDL_OTHER_PAY_SOURCE_IDs != null)
            //{
            //    query += string.Format("AND TREA.OTHER_PAY_SOURCE_ID in ({0}) \n", string.Join(",", filter.TDL_OTHER_PAY_SOURCE_IDs));
            //}
        }

        private void filterTransaction(Mrs00455Filter filter, ref string query)
        {

            if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
            {
                query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.TIME_FROM, filter.TIME_TO);
            }
            if (filter.EXACT_CASHIER_ROOM_IDs != null)
            {
                query += string.Format("AND TRAN.CASHIER_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
            }
            if (filter.CASHIER_LOGINNAMEs != null)
            {
                query += string.Format("AND TRAN.CASHIER_LOGINNAME IN ('{0}') \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
            }
        }

        private void filterServiceReq(Mrs00455Filter filter, ref string query)
        {
            query += "and sr.is_delete =0  \n";
            if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
            {
                query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
            {
                query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            }
            if (filter.REQUEST_ROOM_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID IN ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
            } 
        }

        private void filterSereServ(Mrs00455Filter filter, ref string query)
        {

            query += "AND SS.IS_NO_EXECUTE IS NULL\n";
            if (filter.INPUT_DATA_ID_TIME_TYPE != 6)
            {
                query += string.Format("and ss.is_delete =0\n");
            }

            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND SS.PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            }
            if (filter.INPUT_DATA_ID_PRICE_TYPEs != null && !filter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)1))
            {
                query += string.Format("AND SS.HEIN_CARD_NUMBER IS NOT NULL\n");
            }
            if (filter.INPUT_DATA_ID_PRICE_TYPEs != null && !filter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)2) && !filter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)3))
            {
                query += string.Format("AND SS.HEIN_CARD_NUMBER IS  NULL\n");
            }
            if (filter.SERVICE_IDs != null)
            {
                query += string.Format("AND SS.SERVICE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_IDs));
            }
        }

    }
}
