using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System.Collections.Generic;
using System.Linq;

namespace MRS.Processor.Mrs00826
{
    partial class ManagerSql : BusinessBase
    {
        public List<V_HIS_TREATMENT_1> GetTreatmentView1(Mrs00826Filter filter)
        {
            List<V_HIS_TREATMENT_1> result = new List<V_HIS_TREATMENT_1>();


            string query = "";

            query += "SELECT \n";

            query += "TREA.* \n";
            //
            query += "FROM HIS_RS.V_HIS_TREATMENT_1 TREA \n";
            query += "LEFT JOIN HIS_RS.HIS_SERVICE_REQ SR ON TREA.ID = SR.TREATMENT_ID and SR.IS_DELETE =0 AND SR.IS_NO_EXECUTE IS NULL \n";
            //query += "JOIN HIS_RS.HIS_PATIENT PT ON TREA.PATIENT_ID = PT.ID\n";
            if (filter.INPUT_DATA_ID__TIME_TYPE == 5)//Thanh toán
            {
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.TREATMENT_ID = TREA.ID \n";
            }
            if (filter.INPUT_DATA_ID__TIME_TYPE.HasValue == false || filter.INPUT_DATA_ID__TIME_TYPE == 7)//giám định bảo hiểm
            {
                query += "LEFT JOIN V_HIS_HEIN_APPROVAL HAP ON HAP.TREATMENT_ID=TREA.ID ";
            }
            //
            query += "WHERE 1=1\n";

            #region Loại thời gian
            if (filter.INPUT_DATA_ID__TIME_TYPE == 1)//vào viện
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 2)//ra viện
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 3)//chỉ định
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 4)//kết thúc
            {
                query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 5)//Thanh toán
            {
                query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and TRAN.IS_CANCEL is null \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 6)//Khoá viện phí
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 7)//giám định bảo hiểm
            {
                query += "AND HAP.IS_DELETE = 0 ";
                query += string.Format("AND HAP.EXECUTE_TIME BETWEEN {0} and {1} ", filter.TIME_FROM, filter.TIME_TO);
            }
            else//giám định bảo hiểm
            {
                query += "AND HAP.IS_DELETE = 0 ";
                query += string.Format("AND HAP.EXECUTE_TIME BETWEEN {0} and {1} ", filter.TIME_FROM, filter.TIME_TO);
            }
            #endregion

            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            if (filter.REQUEST_ROOM_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID IN ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
            }
            if (filter.EXECUTE_ROOM_IDs != null)
            {
                query += string.Format("AND SR.EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
            }
            //if (filter.PATIENT_TYPE_IDs != null)
            //{
            //    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            //}
            //if (filter.END_DEPARTMENT_IDs != null)
            //{
            //    query += string.Format("AND TREA.END_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
            //}

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

            {
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TREATMENT_1>(query);
            }
            if (IsNotNullOrEmpty(result)) result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();

            Inventec.Common.Logging.LogSystem.Info("Finish Query ");

            return result;
        }

        public List<V_HIS_SERE_SERV_2> GetSereServView2(Mrs00826Filter filter, List<long> treatmentIds)
        {
            List<V_HIS_SERE_SERV_2> result = new List<V_HIS_SERE_SERV_2>();
            
            string query = "";

            query += "SELECT \n";
            query += "SS.* \n";
            //
            query += "FROM HIS_RS.V_HIS_SERE_SERV_2 SS \n";
            //query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID and SR.IS_DELETE =0 AND SR.IS_NO_EXECUTE IS NULL \n";
            query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
            //query += "JOIN HIS_RS.HIS_PATIENT PT ON TREA.PATIENT_ID = PT.ID\n";
            if (filter.INPUT_DATA_ID__TIME_TYPE == 5)//Thanh toán
            {
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.TREATMENT_ID = TREA.ID \n";
            }
            if (filter.INPUT_DATA_ID__TIME_TYPE.HasValue == false || filter.INPUT_DATA_ID__TIME_TYPE == 7)//giám định bảo hiểm
            {
                query += "LEFT JOIN V_HIS_HEIN_APPROVAL HAP ON HAP.TREATMENT_ID=TREA.ID \n";
            }

            if (IsNotNullOrEmpty(filter.EXACT_CHILD_SERVICE_IDs) || filter.SERVICE_TYPE_ID.HasValue || filter.EXACT_PARENT_SERVICE_ID.HasValue)
            {
                query += "LEFT JOIN HIS_SERVICE SV ON SS.SERVICE_ID=SV.ID \n";
                query += "LEFT JOIN HIS_SERVICE SV1 ON SV.PARENT_ID=SV1.ID \n";
            }
            
            //
            query += "WHERE 1=1\n";
            if (IsNotNullOrEmpty(treatmentIds))
                query += string.Format("AND TREA.ID IN ({0}) \n", string.Join(",", treatmentIds));
            #region Loại thời gian
            if (filter.INPUT_DATA_ID__TIME_TYPE == 1)//vào viện
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 2)//ra viện
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 3)//chỉ định
            {
                query += string.Format("AND SS.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 4)//kết thúc
            {
                query += string.Format("AND SS.FINISH_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 5)//Thanh toán
            {
                query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and TRAN.IS_CANCEL is null \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 6)//Khoá viện phí
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
            }
            else if (filter.INPUT_DATA_ID__TIME_TYPE == 7)//giám định bảo hiểm
            {
                query += "AND HAP.IS_DELETE = 0 ";
                query += string.Format("AND HAP.EXECUTE_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else//giám định bảo hiểm
            {
                query += "AND HAP.IS_DELETE = 0 ";
                query += string.Format("AND HAP.EXECUTE_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            #endregion

            query += "and ss.is_delete=0 \n";
            query += "and ss.is_no_execute is null \n";
            if (IsNotNullOrEmpty(filter.EXACT_CHILD_SERVICE_IDs))
            {
                query += string.Format("and (sv.id in({0})) \n", string.Join(",", filter.EXACT_CHILD_SERVICE_IDs));
            }
            if (filter.EXACT_PARENT_SERVICE_ID.HasValue)
            {
                query += string.Format("and (sv1.id ={0} ) \n", filter.EXACT_PARENT_SERVICE_ID);
            }
            if (filter.SERVICE_TYPE_ID.HasValue)
            {
                query += string.Format("and (sv.service_type_id ={0} ) \n", filter.SERVICE_TYPE_ID);
            }

            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            if (filter.REQUEST_ROOM_IDs != null)
            {
                query += string.Format("AND SS.TDL_REQUEST_ROOM_ID IN ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
            }
            if (filter.EXECUTE_ROOM_IDs != null)
            {
                query += string.Format("AND SS.TDL_EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
            }

            //if (filter.PATIENT_TYPE_IDs != null)
            //{
            //    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            //}
            //if (filter.END_DEPARTMENT_IDs != null)
            //{
            //    query += string.Format("AND TREA.END_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
            //}

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

            {
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_SERE_SERV_2>(query);
            }
            if (IsNotNullOrEmpty(result)) result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();

            Inventec.Common.Logging.LogSystem.Info("Finish Query ");

            return result;
        }
    }
}
