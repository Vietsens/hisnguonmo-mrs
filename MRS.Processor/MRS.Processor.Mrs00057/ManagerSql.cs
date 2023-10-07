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

namespace MRS.Processor.Mrs00057
{
    public  class ManagerSql 
    {
        internal List<Mrs00057RDO> GetRdo(Mrs00057Filter filter)
        {
            try
            {
                List<Mrs00057RDO> result = new List<Mrs00057RDO>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "SS.*, \n";
                query += "TREA.TDL_TREATMENT_TYPE_ID \n";
                //query += "TREA.TDL_PATIENT_NAME, \n";
                //query += "TREA.TDL_PATIENT_CODE, \n";
                //query += "TREA.TREATMENT_CODE, \n";
                //query += "TREA.TDL_HEIN_CARD_NUMBER HEIN_CARD_NUMBER, \n";
                //query += "TREA.TDL_PATIENT_DOB, \n";
                //query += "TREA.ICD_NAME, \n";
                ////query += "TREA.ICD_TEXT, ";
                //query += "TREA.TDL_PATIENT_TYPE_ID, \n";
                //query += "TREA.TDL_PATIENT_ADDRESS, \n";
                //query += "TREA.TDL_PATIENT_DOB, \n";
                //query += "TREA.TDL_PATIENT_GENDER_ID, \n";
                //query += "SR.START_TIME, \n";
                //query += "SR.FINISH_TIME, \n";
                //query += "SR.EXECUTE_LOGINNAME, \n";
                //if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                //{
                //    query += "SAMP.RESULT_TIME, \n";
                //    query += "SAMP.BARCODE, \n";
                //}
                //query += "SR.EXECUTE_USERNAME \n";
                //query += "EXT.BEGIN _TIME";
                // query += "EXT.END_TIME";

                query += "FROM HIS_RS.V_HIS_SERE_SERV SS \n";
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
                // query += "JOIN HIS_SERE_SERV_EXT EXT ON EXT.TDL_TREATMENT_ID=TREA.ID ";
                //query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00057') \n";
                query += "WHERE 1=1 ";

                query += "AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL and sr.is_delete =0 \n";


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
                    if (filter.TIME_FROM != null)
                    {
                        query += string.Format("AND SR.INTRUCTION_TIME >={0} \n", filter.TIME_FROM);
                    }
                    if (filter.TIME_TO != null)
                    {
                        query += string.Format("AND SR.INTRUCTION_TIME <{0} \n", filter.TIME_TO);
                    }
                   
                }
                //if (filter.TIME_FROM != null)
                //{
                //    query += string.Format("AND SR.INTRUCTION_TIME >={0} ", filter.TIME_FROM);
                //}
                //if (filter.TIME_TO != null)
                //{
                //    query += string.Format("AND SR.INTRUCTION_TIME <{0} ", filter.TIME_TO);
                //}
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("AND trea.BRANCH_ID IN ({0}) \n", string.Join(",", filter.BRANCH_IDs));
                }
                //if (filter.FINISH_TIME_FROM != null)
                //{
                //    query += string.Format("AND SR.FINISH_TIME >={0} ", filter.FINISH_TIME_FROM);
                //}
                //if (filter.FINISH_TIME_TO != null)
                //{
                //    query += string.Format("AND SR.FINISH_TIME <{0} ", filter.FINISH_TIME_TO);
                //}
                //if (filter.REQUEST_DEPARTMENT_ID != null)
                //{
                //    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0} \n", filter.REQUEST_DEPARTMENT_ID);
                //}
                //if (filter.EXECUTE_DEPARTMENT_ID != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                //}
                //if (filter.EXECUTE_ROOM_ID != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXECUTE_ROOM_ID);
                //}
                //if (filter.TREATMENT_TYPE_ID != null)
                //{
                //    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
                //}
                //if (filter.REPORT_TYPE_CAT_ID != null)
                //{
                //    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} \n", filter.REPORT_TYPE_CAT_ID);
                //}
                //if (filter.REPORT_TYPE_CAT_IDs != null)
                //{
                //    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN ({0}) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                //}
                //if (filter.REQUEST_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                //}
                //if (filter.EXECUTE_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                //}

                query += string.Format("AND SR.SERVICE_REQ_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<Mrs00057RDO>(query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

    }
}
