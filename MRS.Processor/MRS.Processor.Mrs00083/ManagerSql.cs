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

namespace MRS.Processor.Mrs00083
{
    public partial class Mrs00083RDOManager : BusinessBase
    {
        public string MEDI_ORG_CODE { get; set; }
        public List<Mrs00083RDO> GetRdo(Mrs00083Filter filter)
        {
            try
            {
                List<Mrs00083RDO> result = new List<Mrs00083RDO>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "SS.TDL_INTRUCTION_TIME, \n";//
                query += "SS.TDL_INTRUCTION_DATE, \n";//
                query += "SS.HEIN_CARD_NUMBER,\n";//
                query += "SS.PATIENT_TYPE_ID, \n";//
                query += "SS.VIR_PRICE Price, \n";//
                query += "SS.SERVICE_ID, \n";//
                query += "SS.TDL_SERVICE_NAME SERVICE_NAME, \n";//
                query += "HA.EXECUTE_TIME,";
               // query += "SS.EXECUTE_TIME EXECUTE_DATE_STR,";          
                query += "SSE.CONCLUDE, \n";
                query += "SSE.DESCRIPTION, \n";
                query += "SSE.BEGIN_TIME,";
                query += "SSE.END_TIME,";
                query += "TREA.TDL_PATIENT_NAME PATIENT_NAME, \n";//
                query += "TREA.TREATMENT_CODE, \n";//
                query += "TREA.ID TREATMENT_ID, \n";//
                query += "TREA.STORE_CODE, \n";//
                query += "TREA.IN_CODE, \n";//
                query += "TREA.IN_TIME, \n";//
                query += "TREA.OUT_TIME, \n";//
                query += "TREA.TDL_HEIN_CARD_NUMBER,\n";//
                query += "TREA.TDL_PATIENT_DOB, \n";//
                query += "TREA.TDL_PATIENT_GENDER_ID, \n";//
                query += "TREA.TDL_PATIENT_CODE PATIENT_CODE, \n";//
                query += "TREA.TDL_PATIENT_TYPE_ID, \n";//
                query += "TREA.TDL_PATIENT_ADDRESS VIR_ADDRESS, \n";//
                query += "TREA.ICD_NAME, \n";
                query += "TREA.ICD_TEXT, ";
                query += "SR.REQUEST_ROOM_ID, \n";//
                query += "SR.REQUEST_LOGINNAME, \n";//
                query += "SR.REQUEST_USERNAME, \n";//
                query += "SR.EXECUTE_ROOM_ID, \n";//
                query += "SR.START_TIME TDL_START_TIME, \n";//
                query += "SR.FINISH_TIME, \n";//
                query += "SR.TDL_HEIN_MEDI_ORG_CODE MEDI_ORG_CODE, \n";//
                query += "SR.EXECUTE_LOGINNAME, \n";//
                query += "M.MACHINE_CODE, \n";//
                query += "M.MACHINE_NAME, \n";//
                query += "MA.MACHINE_NAME AS EXECUTE_MACHINE_NAME, \n";//
                query += "MA.MACHINE_CODE AS EXECUTE_MACHINE_CODE, \n";//
                query += "m.MACHINE_ID, \n";//
                query += "PT.PATIENT_TYPE_NAME AS TDL_PATIENT_TYPE_NAME, \n";//
                query += "PT1.PATIENT_TYPE_NAME , \n";//
                
                query += "SR.EXECUTE_USERNAME \n";//
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {

                    query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                    query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
                }



                query += " LEFT JOIN LATERAL (select SM.SERVICE_ID,\n";
                query += "listagg(m.id,'') within group(order by 1) machine_id, \n";
                query += "listagg(m.machine_name,'') within group(order by 1) machine_name, \n";
                query += " listagg(m.machine_code,'') within group(order by 1) machine_code\n";
                query += "FROM HIS_RS.HIS_SERVICE_MACHINE SM \n";
                query += "JOIN HIS_RS.HIS_MACHINE M  ON SM.MACHINE_ID=M.ID\n";
                query += "WHERE SM.SERVICE_ID = SS.SERVICE_ID\n";
                if (filter.MACHINE_IDs != null)
                {
                    query += string.Format("AND M.ID IN ({0}) \n ", string.Join(",", filter.MACHINE_IDs));
                }
                query += "GROUP BY SM.SERVICE_ID \n";
                query += ") M  ON M.SERVICE_ID = SS.SERVICE_ID\n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";

                query += "JOIN HIS_PATIENT_TYPE PT ON PT.ID = TREA.TDL_PATIENT_TYPE_ID   \n";
                query += "LEFT JOIN HIS_PATIENT_TYPE PT1 ON PT1.ID = SS.PATIENT_TYPE_ID   \n";
                

                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_EXT SSE ON SSE.sere_serv_id=ss.ID \n";

                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";

                query += "LEFT JOIN HIS_RS.HIS_MACHINE MA ON SSE.MACHINE_ID = MA.ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_HEIN_APPROVAL HA ON HA.TREATMENT_ID = TREA.ID  \n";

                query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00083') \n";
                query += "WHERE 1=1 ";

                query += "AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL and sr.is_delete =0 \n";

                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN);


                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SSE.END_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
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

                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }


                //Mã máy
              

                if (filter.MACHINE_IDs != null)
                {
                    query += string.Format("AND M.service_id is not null\n ", string.Join(",", filter.MACHINE_IDs));
                }
                if (filter.EXECUTE_MACHINE_IDs != null)
                {
                    query += string.Format("AND SSE.MACHINE_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_MACHINE_IDs));
                
                }


                //khoa chỉ định
                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0} \n", filter.REQUEST_DEPARTMENT_ID);
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }

                //phòng chỉ định
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID = {0} \n", filter.REQUEST_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID IN ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }

                //khoa thực hiện
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                //phòng thực hiện
                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
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
                if (filter.REQUEST_TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND SR.TREATMENT_TYPE_ID = {0} \n", filter.REQUEST_TREATMENT_TYPE_ID);
                }
                if (filter.REQUEST_TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SR.TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.REQUEST_TREATMENT_TYPE_IDs));
                }

                query += string.Format("AND SR.SERVICE_REQ_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00083RDO>(query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
            
        }
        
        //public List<SSE> GetRdoExt(Mrs00083Filter filter)
        //{
        //    try
        //    {
        //        List<SSE> result = new List<SSE>();
        //        string query = "-- from Qcs\n";
        //        query += "SELECT \n";
        //        query += "SS.ID SERE_SERV_ID, \n";
        //        query += "SSE.TEST_INDEX_ID, \n";
        //        query += "SSE.ID, \n";
        //        query += "SSE.VALUE \n";

        //        query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
        //        query += "JOIN HIS_RS.HIS_SERE_SERV_TEIN SSE ON SSE.SERE_SERV_ID=SS.ID  \n";
        //        if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
        //        {

        //            query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
        //            query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
        //        }

        //        if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
        //        {

        //            query += "JOIN LIS_RS.LIS_SAMPLE SAMP ON SAMP.SERVICE_REQ_CODE=SS.TDL_SERVICE_REQ_CODE \n";
        //        }
        //        query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
        //        query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";
        //        // query += "JOIN HIS_SERE_SERV_EXT EXT ON EXT.TDL_TREATMENT_ID=TREA.ID ";
        //        query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00083') \n";
        //        query += "WHERE 1=1 ";

        //        query += "AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL and sr.is_delete =0 \n";


        //        if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
        //        {
        //            query += string.Format("AND SAMP.RESULT_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
        //        }
        //        else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
        //        {
        //            query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
        //        }
        //        else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
        //        {
        //            query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
        //        }
        //        else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
        //        {
        //            query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
        //        }
        //        else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
        //        {
        //            query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
        //        }
        //        else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
        //        {
        //            query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
        //        }
        //        else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
        //        {
        //            query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
        //        }
        //        else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
        //        {
        //            query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
        //        }
        //        else
        //        {
        //            if (filter.TIME_FROM != null)
        //            {
        //                query += string.Format("AND SR.INTRUCTION_TIME >={0} \n", filter.TIME_FROM);
        //            }
        //            if (filter.TIME_TO != null)
        //            {
        //                query += string.Format("AND SR.INTRUCTION_TIME <{0} \n", filter.TIME_TO);
        //            }
        //            if (filter.FINISH_TIME_FROM != null)
        //            {
        //                query += string.Format("AND SR.FINISH_TIME >={0} \n", filter.FINISH_TIME_FROM);
        //            }
        //            if (filter.FINISH_TIME_TO != null)
        //            {
        //                query += string.Format("AND SR.FINISH_TIME <{0}\n ", filter.FINISH_TIME_TO);
        //            }
        //        }
        //        //if (filter.TIME_FROM != null)
        //        //{
        //        //    query += string.Format("AND SR.INTRUCTION_TIME >={0} ", filter.TIME_FROM);
        //        //}
        //        //if (filter.TIME_TO != null)
        //        //{
        //        //    query += string.Format("AND SR.INTRUCTION_TIME <{0} ", filter.TIME_TO);
        //        //}
        //        if (filter.PATIENT_TYPE_ID != null)
        //        {
        //            query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
        //        }
        //        //if (filter.FINISH_TIME_FROM != null)
        //        //{
        //        //    query += string.Format("AND SR.FINISH_TIME >={0} ", filter.FINISH_TIME_FROM);
        //        //}
        //        //if (filter.FINISH_TIME_TO != null)
        //        //{
        //        //    query += string.Format("AND SR.FINISH_TIME <{0} ", filter.FINISH_TIME_TO);
        //        //}
        //        if (filter.REQUEST_DEPARTMENT_ID != null)
        //        {
        //            query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0} \n", filter.REQUEST_DEPARTMENT_ID);
        //        }
        //        if (filter.EXECUTE_DEPARTMENT_ID != null)
        //        {
        //            query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
        //        }
        //        if (filter.EXECUTE_ROOM_ID != null)
        //        {
        //            query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXECUTE_ROOM_ID);
        //        }
        //        if (filter.TREATMENT_TYPE_ID != null)
        //        {
        //            query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
        //        }
        //        if (filter.REPORT_TYPE_CAT_ID != null)
        //        {
        //            query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} \n", filter.REPORT_TYPE_CAT_ID);
        //        }
        //        if (filter.REPORT_TYPE_CAT_IDs != null)
        //        {
        //            query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN ({0}) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
        //        }
        //        if (filter.REQUEST_DEPARTMENT_IDs != null)
        //        {
        //            query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
        //        }
        //        if (filter.EXECUTE_DEPARTMENT_IDs != null)
        //        {
        //            query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
        //        }

        //        query += string.Format("AND SR.SERVICE_REQ_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN);
        //        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
        //        result = new MOS.DAO.Sql.MyAppContext().GetSql<SSE>(query);


        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        param.HasException = true;
        //        return null;
        //    }
        //}

    }
}
