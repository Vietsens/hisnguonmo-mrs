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
using MRS.Processor.Mrs00043;

namespace MRS.Processor.Mrs00043
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00043RDO> GetRdo(Mrs00043Filter filter,List<long> CLS_SERVICE_TYPE_IDs)
        {
            try
            {
                List<Mrs00043RDO> result = new List<Mrs00043RDO>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "ss.ID SS_ID,\n";////
                query += "ss.TDL_PATIENT_ID,\n";////
                query += "ss.HEIN_CARD_NUMBER,\n";////
                query += "SS.PATIENT_TYPE_ID, \n";////
                query += "NVL(SS.VIR_PRICE,0) PRICE, \n";////
                query += "NVL(SS.VIR_TOTAL_PRICE,0) VIR_TOTAL_PRICE, \n";////
                query += "NVL(SS.VIR_TOTAL_PATIENT_PRICE,0) VIR_TOTAL_HEIN_PRICE, \n";////
                query += "NVL(SS.VIR_TOTAL_HEIN_PRICE,0) VIR_TOTAL_HEIN_PRICE, \n";////
                query += "SS.SERVICE_ID, \n";////
                query += "SS.AMOUNT, \n";////
                query += "SS.TDL_SERVICE_NAME SERVICE_NAME, \n";////
                query += "SS.TDL_INTRUCTION_TIME INTRUCTION_TIME_NUM, \n";////
                //query += "SS.TDL_INTRUCTION_DATE, \n";//
                query += "TREA.TDL_PATIENT_NAME PATIENT_NAME, \n";////
                query += "TREA.TDL_PATIENT_CODE PATIENT_CODE, \n";////
                query += "TREA.TDL_PATIENT_CODE, \n";////
                query += "TREA.TREATMENT_CODE, \n";////
                query += "TREA.ID TREATMENT_ID, \n";//
                query += "TREA.STORE_CODE, \n";////
                query += "TREA.IN_CODE, \n";////
                query += "TREA.MEDI_ORG_CODE, \n";////
                query += "TREA.IN_TIME, \n";////
                query += "TREA.OUT_TIME, \n";////
                query += "TREA.TDL_HEIN_CARD_NUMBER,\n";////
                query += "TREA.TDL_PATIENT_DOB, \n";////
                query += "TREA.TDL_PATIENT_GENDER_ID, \n";////
                query += "TREA.TDL_PATIENT_TYPE_ID, \n";////
                query += "TREA.TDL_PATIENT_ADDRESS VIR_ADDRESS, \n";////
                query += "SS.TDL_HEIN_SERVICE_BHYT_CODE, \n";////
                query += "SS.TDL_HEIN_SERVICE_BHYT_NAME, \n";////
                query += "SSE.CONCLUDE ENDO_RESULT, \n";////
                query += "SSE.DESCRIPTION, \n";////
                query += "SSE.BEGIN_TIME,";////
                query += "SSE.MACHINE_ID,";////
                query += "SSE.END_TIME,";////
                query += "SSE.FILM_SIZE_ID,";////
                query += "SSE.NUMBER_OF_FILM,";////

                query += "SR.ICD_NAME, \n";////

                query += "SR.ICD_TEXT, \n";////
                query += "SR.SERVICE_REQ_CODE, \n";////
                query += "SR.START_TIME, \n";////
                query += "SR.EXECUTE_ROOM_ID, \n";////
                query += "SR.REQUEST_ROOM_ID, \n";////
                query += "SR.REQUEST_LOGINNAME, \n";////
                query += "SR.REQUEST_USERNAME, \n";////
                query += "SR.FINISH_TIME, \n";////
                //query += "SR.TDL_HEIN_MEDI_ORG_CODE MEDI_ORG_CODE, \n";//
                query += "SR.EXECUTE_LOGINNAME, \n";////
                query += "SR.EXECUTE_USERNAME \n";////
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {

                    query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                    query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
                }

                //if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";

                    query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_EXT SSE ON SSE.sere_serv_id=ss.ID ";
                }
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";

                query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00043') \n";
                query += "WHERE 1=1 ";

                query += "AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL and sr.is_delete =0 and ss.is_delete=0 \n";

                //query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN);


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
                //if (filter.TIME_FROM != null)
                //{
                //    query += string.Format("AND SR.INTRUCTION_TIME >={0} ", filter.TIME_FROM);
                //}
                //if (filter.TIME_TO != null)
                //{
                //    query += string.Format("AND SR.INTRUCTION_TIME <{0} ", filter.TIME_TO);
                //}
                if (CLS_SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", CLS_SERVICE_TYPE_IDs));
                }

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} \n", filter.PATIENT_TYPE_ID);
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
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }
                //if (filter.REQUEST_ROOM_ID != null)
                //{
                //    query += string.Format("AND SR.REQUEST_ROOM_ID = {0} \n", filter.REQUEST_ROOM_ID);
                //}
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
                }
                //if (filter.REPORT_TYPE_CAT_ID != null)
                //{
                //    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} \n", filter.REPORT_TYPE_CAT_ID);
                //}
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
                //if (filter.REQUEST_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                //}
                //if (filter.EXECUTE_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                //}

                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("AND TREA.BRANCH_ID = {0} \n", filter.BRANCH_ID);
                }

                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("AND TREA.BRANCH_ID IN ({0}) \n", string.Join(",", filter.BRANCH_IDs));
                }

                if (filter.EXECUTE_MACHINE_IDs != null)
                {
                    query += string.Format("AND SSE.MACHINE_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_MACHINE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00043RDO>(query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        //public List<REQUEST_FILM_SIZE> GetRequestFilmSize(long min, long max)
        //{
        //    List<REQUEST_FILM_SIZE> result = null;
        //    try
        //    {
        //        string query = "";
        //        query += "select ";
        //        query += "sr.parent_id service_req_id, ";
        //        query += "maty.film_size_id ";
        //        query += "from ";
        //        query += "v_his_exp_mest_material exma ";
        //        query += "join his_service_req sr on sr.id=exma.tdl_service_req_id ";
        //        query += "join his_material_type maty on maty.id=exma.material_type_id ";
        //        query += "where ";
        //        query += "exma.is_export =1 ";
        //        query += "and sr.parent_id between {0} and {1} ";
        //        query += "and maty.film_size_id is not null ";
        //        query += "group by ";
        //        query += "sr.parent_id, ";
        //        query += "maty.film_size_id ";
        //        query = string.Format(query, min, max);
        //        result = new MOS.DAO.Sql.SqlDAO().GetSql<REQUEST_FILM_SIZE>(query);
        //        Inventec.Common.Logging.LogSystem.Info("SQL:" + query);
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = null;
        //    }
        //    return result;
        //}

        //internal List<HIS_SERE_SERV> GetSereServExpend(List<long> sereServIds)
        //{
        //    List<HIS_SERE_SERV> result = null;
        //    try
        //    {
        //        if (sereServIds != null && sereServIds.Count > 0)
        //        {
        //            result = new List<HIS_SERE_SERV>();
        //            string sql = "SELECT * FROM HIS_SERE_SERV WHERE IS_EXPEND = 1 AND IS_DELETE = 0 AND SERVICE_REQ_ID IS NOT NULL AND PARENT_ID IN ({0})";
        //            int skip = 0;
        //            while (sereServIds.Count - skip > 0)
        //            {
        //                List<long> listIds = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
        //                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

        //                var query = string.Format(sql, string.Join(",", listIds));
        //                Inventec.Common.Logging.LogSystem.Info("SQL:" + query);
        //                var ss = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);
        //                if (ss != null && ss.Count > 0)
        //                {
        //                    result.AddRange(ss);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = null;
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //    }
        //    return result;
        //}
    }
}
