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

namespace MRS.Processor.Mrs00041
{
    public partial class Mrs00041RDOManager : BusinessBase
    {
        public List<Mrs00041RDO> GetRdo(Mrs00041Filter filter)
        {
            try
            {
                List<LIS_SAMPLE_TYPE> countLisSample = CountSample();
                List<Mrs00041RDO> result = new List<Mrs00041RDO>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "SS.ID SERE_SERV_ID, \n";
                query += "TREA.TDL_PATIENT_NAME PATIENT_NAME, \n";
                query += "TREA.TDL_PATIENT_TYPE_ID,";
                query += "TREA.TREATMENT_CODE, \n";
                query += "TREA.IN_TIME, \n";
                query += "TREA.OUT_TIME, \n";
                query += "TREA.TDL_HEIN_CARD_NUMBER HEIN_CARD_NUMBER,\n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                query += "NVL(TREA.TDL_PATIENT_PHONE,TREA.TDL_PATIENT_MOBILE) TDL_PATIENT_PHONE, \n";
                query += "TREA.TDL_PATIENT_GENDER_ID, \n";
                query += "TREA.TDL_TREATMENT_TYPE_ID, \n";
                query += "SS.SERVICE_REQ_ID, \n";
                query += "TREA.TDL_PATIENT_CODE PATIENT_CODE, \n";
                query += "SS.PATIENT_TYPE_ID, \n";
                query += "TREA.TDL_PATIENT_ADDRESS VIR_ADDRESS, \n";
                query += "SS.VIR_PRICE, \n";
                query += "TREA.ICD_NAME ICD_TEST, \n";
                query += "SR.REQUEST_ROOM_ID, \n";
                query += "SR.REQUEST_DEPARTMENT_ID,\n";
                query += "SR.SERVICE_REQ_CODE, \n";
                query += "SR.REQUEST_LOGINNAME, \n";
                query += "SR.REQUEST_USERNAME, \n";
                query += "SR.BLOCK, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_CODE SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME SERVICE_NAME, \n";
                query += "SS.IS_EXPEND, \n";
                query += "SS.TDL_HEIN_SERVICE_BHYT_CODE, \n";
                query += "SS.TDL_HEIN_SERVICE_BHYT_NAME, \n";
                query += "SS.PATIENT_TYPE_ID, \n";
                query += "SS.AMOUNT, \n";

                //query += "TREA.ICD_TEXT, ";
                query += "SR.EXECUTE_ROOM_ID, \n";
                if (countLisSample != null && countLisSample.Count>0)
                {
                    query += "SAMP.RESULT_TIME, \n";
                    query += "SAMP.BARCODE, \n";
                    query += "SAMP.SAMPLE_TIME SAMPLE_TIME, \n";
                    //query += "SAMP.NOTE SAMPLE_NOTE, \n";
                    //query += "SAMP.SAMPLE_LOGINNAME, \n";
                    //query += "SAMP.SAMPLE_USERNAME, \n";
                }
                //query += "EXT.BEGIN _TIME";
                // query += "EXT.END_TIME";

                query += "SR.START_TIME EXECUTE_TIME, \n";
                query += "SR.START_TIME, \n";
                query += "SR.FINISH_TIME, \n";
                query += "SS.TDL_INTRUCTION_TIME, \n";
                query += "SS.TDL_INTRUCTION_DATE, \n";
                query += "SR.EXECUTE_LOGINNAME, \n";
                if (filter.IS_USER_FROM_ACS == true)
                {
                    query += "AU.USERNAME EXECUTE_USERNAME, \n";
                }
                else
                {
                    query += "SR.EXECUTE_USERNAME, \n";
                }
                query += "1 \n";
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {

                    query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                    query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
                }

                if (countLisSample != null && countLisSample.Count > 0)
                {

                    query += "LEFT JOIN LIS_RS.LIS_SAMPLE SAMP ON SAMP.SERVICE_REQ_CODE=SS.TDL_SERVICE_REQ_CODE \n";
                }
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                if (filter.IS_USER_FROM_ACS == true)
                {
                    query += "LEFT JOIN ACS_RS.ACS_USER AU ON SR.EXECUTE_LOGINNAME=AU.LOGINNAME  \n";
                }
               
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";
                //query += "LEFT JOIN HIS_RS.HIS_SERVICE_MACHINE SM ON SS.SERVICE_ID = SM.SERVICE_ID AND SM.MACHINE_ID=SR.MACHINE_ID\n";
                query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00041') \n";
                query += "WHERE 1=1 ";

                query += "AND sr.IS_NO_EXECUTE IS NULL  and sr.is_delete =0 \n";

                query += "AND SS.IS_NO_EXECUTE IS NULL  and ss.is_delete =0 \n";


                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SAMP.RESULT_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
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
                    if (filter.FINISH_TIME_FROM != null)
                    {
                        query += string.Format("AND SR.FINISH_TIME >={0} \n", filter.FINISH_TIME_FROM);
                    }
                    if (filter.FINISH_TIME_TO != null)
                    {
                        query += string.Format("AND SR.FINISH_TIME <{0}\n ", filter.FINISH_TIME_TO);
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
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("AND trea.BRANCH_ID IN ({0}) \n", string.Join(",", filter.BRANCH_IDs));
                }

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                //if (filter.FINISH_TIME_FROM != null)
                //{
                //    query += string.Format("AND SR.FINISH_TIME >={0} ", filter.FINISH_TIME_FROM);
                //}
                //if (filter.FINISH_TIME_TO != null)
                //{
                //    query += string.Format("AND SR.FINISH_TIME <{0} ", filter.FINISH_TIME_TO);
                //}
                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0} \n", filter.REQUEST_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.REPORT_TYPE_CAT_ID != null)
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
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_IDs));
                }
                if (filter.EXACT_PARENT_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN (select id from his_rs.his_service where parent_id in ({0})) \n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
                }
                if (filter.SERVICE_FULL_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_FULL_IDs));
                }
                if(filter.IS_ADD_GP==true)
                {
                    query += string.Format("AND SR.SERVICE_REQ_TYPE_ID in ({0},{1}) ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__GPBL);
                }    
                else
                {
                    query += string.Format("AND SR.SERVICE_REQ_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN);
                }    
                
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<Mrs00041RDO>(query);
                

                //result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00041RDO>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
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
        public List<SSE> GetRdoExt(Mrs00041Filter filter)
        {
            try
            {
                List<SSE> result = new List<SSE>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "SS.ID SERE_SERV_ID, \n";
                query += "SSE.TEST_INDEX_ID, \n";
                query += "SSE.ID, \n";
                query += "SSE.LEAVEN, \n";
                query += "SSE.NOTE, \n";
                query += "nvl(SSE.MACHINE_ID,ext.MACHINE_ID) machine_id, \n";
                query += "EXT.DESCRIPTION, \n";
                query += "EXT.CONCLUDE, \n";
                query += "SSE.VALUE \n";

                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_TEIN SSE ON SSE.SERE_SERV_ID=SS.ID  \n";
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
               query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_EXT EXT ON EXT.SERE_SERV_ID=SS.ID ";
                query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00041') \n";
                query += "WHERE 1=1 ";

                query += "AND sr.IS_NO_EXECUTE IS NULL  and sr.is_delete =0 \n";

                query += "AND SS.IS_NO_EXECUTE IS NULL  and ss.is_delete =0 \n";


                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SAMP.RESULT_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
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
                    if (filter.FINISH_TIME_FROM != null)
                    {
                        query += string.Format("AND SR.FINISH_TIME >={0} \n", filter.FINISH_TIME_FROM);
                    }
                    if (filter.FINISH_TIME_TO != null)
                    {
                        query += string.Format("AND SR.FINISH_TIME <{0}\n ", filter.FINISH_TIME_TO);
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
                //if (filter.FINISH_TIME_FROM != null)
                //{
                //    query += string.Format("AND SR.FINISH_TIME >={0} ", filter.FINISH_TIME_FROM);
                //}
                //if (filter.FINISH_TIME_TO != null)
                //{
                //    query += string.Format("AND SR.FINISH_TIME <{0} ", filter.FINISH_TIME_TO);
                //}
                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0} \n", filter.REQUEST_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.REPORT_TYPE_CAT_ID != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} \n", filter.REPORT_TYPE_CAT_ID);
                }
                if (filter.REPORT_TYPE_CAT_IDs != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN ({0}) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_IDs));
                }
                if (filter.SERVICE_FULL_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_FULL_IDs));
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                if (filter.IS_ADD_GP == true)
                {
                    query += string.Format("AND SR.SERVICE_REQ_TYPE_ID in ({0},{1}) ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__GPBL);
                }
                else
                {
                    query += string.Format("AND SR.SERVICE_REQ_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

                 //result = new MOS.DAO.Sql.SqlDAO().GetSql<SSE>(query);
               result = new MOS.DAO.Sql.MyAppContext().GetSql<SSE>(query);
                

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

    }
}
