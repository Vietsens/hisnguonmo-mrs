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
using MRS.Proccessor.Mrs00556;
using MRS.MANAGER.Config;
using MOS.DAO.Sql;

namespace MRS.Processor.Mrs00556
{
    public class ManagerSql
    {
        public List<Mrs00556RDO> GetRdo(Mrs00556Filter filter)
        {
            try
            {
                List<Mrs00556RDO> result = new List<Mrs00556RDO>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "SS.*, \n";
                query += "(select sv.pttt_group_id from his_service sv where sv.id=ss.service_id) PTTT_GROUP_ID, \n";
                //
                //query += "SRC.CATEGORY_CODE, \n";//
                //query += "SRC.CATEGORY_NAME, \n";//
                //query += "SRC.NUM_ORDER CATEGORY_NUM_ORDER, \n";
                //

                query += "SR.EXECUTE_USERNAME TDL_EXECUTE_USERNAME, \n";//
                query += "SR.EXECUTE_LOGINNAME TDL_EXECUTE_LOGINNAME, \n";//
                query += "SR.SERVICE_REQ_STT_ID, \n";//
                query += "nvl(TREA.TDL_FIRST_EXAM_ROOM_ID,(select min(execute_room_id) keep(dense_rank first order by id) from his_rs.his_service_req where treatment_id=trea.id and service_Req_type_id=1 and is_delete=0 and is_no_execute is null)) TDL_FIRST_EXAM_ROOM_ID, \n";//
                query += "TREA.TDL_TREATMENT_TYPE_ID, \n";//
                query += "TREA.TREATMENT_CODE, \n";//
                query += "TREA.TDL_PATIENT_CODE PATIENT_CODE, \n";//
                query += "TREA.TDL_PATIENT_NAME PATIENT_NAME, \n";//
                query += "nvl(TREA.TDL_PATIENT_DOB,0) DOB, \n";//
                query += "TREA.TDL_PATIENT_GENDER_NAME GENDER_NAME, \n";//
                query += "TREA.TDL_PATIENT_ADDRESS, \n";//
                query += "nvl(TRAN.EXEMPTION,0) as EXEMPTION,\n";//
                query += "nvl(TRAN.AMOUNT,0) BILL_SUM,\n";//
                query += "nvl(TRAN.TRANSACTION_TIME,0) as TRANSACTION_TIME,\n";//
                query += "nvl(TRAN.NUM_ORDER,0) as NUM_ORDER,\n";//
                query += "TREA.TDL_HEIN_CARD_NUMBER,";//
                query += "TREA.TDL_HEIN_MEDI_ORG_CODE, \n";//
                query += "TREA.IN_TIME, \n";//
                query += "TREA.OUT_TIME, \n";//
                query += "TREA.ICD_NAME, \n";//
                query += "TREA.TDL_PATIENT_TYPE_ID \n";//

                query += "FROM HIS_RS.V_HIS_SERE_SERV SS \n";
                //query += "JOIN HIS_RS.HIS_SERVICE SV ON SS.SERVICE_ID=SV.ID  \n";
                //query += "JOIN HIS_RS.HIS_SERVICE_TYPE SVT ON SS.TDL_SERVICE_TYPE_ID=SVT.ID  \n";

                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";


                query += "JOIN lateral\n";
                query += "(\n";
                query += "select \n";
                query += "1\n";
                query += "from HIS_RS.HIS_SERVICE_REQ SR where 1=1\n";
                query += "and SS.SERVICE_REQ_ID=SR.ID\n";
                query += "and sr.is_delete =0 \n";
                query += ") sr ON SS.SERVICE_REQ_ID=SR.ID\n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";
                //comment
                //query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00556') \n";

                query += "WHERE 1=1 ";

                query += "AND SS.IS_NO_EXECUTE IS NULL\n";

                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null and tran.sale_type_id is null\n", filter.TIME_FROM, filter.TIME_TO);
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
                    query = query.Replace("from HIS_RS.HIS_SERVICE_REQ SR where 1=1\n",string.Format("from HIS_RS.HIS_SERVICE_REQ SR where 1=1 \n AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT));
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query = query.Replace("from HIS_RS.HIS_SERVICE_REQ SR where 1=1\n",string.Format("from HIS_RS.HIS_SERVICE_REQ SR where 1=1 \n AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL));
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query = query.Replace("from HIS_RS.HIS_SERVICE_REQ SR where 1=1\n",string.Format("from HIS_RS.HIS_SERVICE_REQ SR where 1=1 \n AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO));
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null and tran.sale_type_id is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                if (filter.CASHIER_LOGINNAMEs != null)//
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME in ('{0}') \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.IS_FEE_OF_BHYT == true)//
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID={0} \n", HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                }


                if (filter.TDL_PATIENT_TYPE_IDs != null)//
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.TREATMENT_TYPE_IDs != null)//
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TREATMENT_TYPE_ID != null)//
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID ={0} \n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.PATIENT_TYPE_IDs != null)//
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_TYPE_ID != null)//
                {
                    query += string.Format("AND ss.PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                }

                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND ss.TDL_REQUEST_DEPARTMENT_ID ={0} \n", filter.DEPARTMENT_ID);
                }

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND ss.TDL_REQUEST_DEPARTMENT_ID  in ({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND ss.TDL_SERVICE_TYPE_ID  in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND ss.TDL_REQUEST_ROOM_ID  in ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.INPUT_DATA_ID_STT_TYPE != null)
                {
                    query += string.Format("AND {0} \n", filter.INPUT_DATA_ID_STT_TYPE == 1 ? "trea.is_pause=1" : "trea.is_pause is null");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                //result = new MOS.DAO.Sql.MyAppContext().GetSql<Mrs00556RDO>(query);
                result = new SqlDAO().GetSql<Mrs00556RDO>(new CommonParam(), query);
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
