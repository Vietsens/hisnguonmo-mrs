using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00662
{
    class ManagerSql
    {
        internal List<Mrs00662RDO> GetSS(Mrs00662Filter filter)
        {
            List<Mrs00662RDO> result = new List<Mrs00662RDO>();
            try
            {
                string query = "--danh sach phau thuat thu thuat theo thoi gian ket thuc o man hinh xu ly pttt\n";
                query += "SELECT\n";
                query += "SS.*,\n";
                query += "s.notice, \n";
                query += "sr.note, \n";
                query += "SR.INTRUCTION_TIME, \n";
                query += "PTTT.PTTT_METHOD_ID, \n";
                query += "PTTT.REAL_PTTT_METHOD_ID, \n";
                query += "PTTT.PTTT_PRIORITY_ID, \n";
                query += "PTTT.PTTT_TABLE_ID, \n";
                query += "PTTT.EMOTIONLESS_RESULT_ID, \n";
                query += "PTTT.EMOTIONLESS_METHOD_SECOND_ID, \n";
                query += "PTTT.EMOTIONLESS_METHOD_ID, \n";
                query += "PTTT.BEFORE_PTTT_ICD_NAME, \n";
                query += "PTTT.AFTER_PTTT_ICD_NAME, \n";
                query += "PTTT.PTTT_CATASTROPHE_ID, \n";
                query += "PTTT.PTTT_CONDITION_ID, \n";
                query += "PTTT.DEATH_WITHIN_ID, \n";
                query += "PTTT.PTTT_GROUP_ID, \n";
                query += "PTTT.MANNER, \n";
                query += "SSE.END_TIME, \n";
                query += "SSE.BEGIN_TIME, \n";
                query += string.Format("(case when SR.TREATMENT_TYPE_ID ={0} then 'DT' else 'KH' end) SR_TREATMENT_TYPE_CODE, \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                query += "S.PTTT_GROUP_ID SV_PTTT_GROUP_ID, \n";
                query += "T.TDL_TREATMENT_TYPE_ID, \n";
                query += "T.TDL_PATIENT_TYPE_ID, \n";
                query += "T.TDL_PATIENT_CODE, \n";
                query += "T.TDL_PATIENT_DOB, \n";
                query += "T.TDL_PATIENT_ADDRESS, \n";
                query += "T.TDL_PATIENT_GENDER_ID, \n";
                query += "T.TDL_PATIENT_NAME, \n";
                query += "SR.EXECUTE_LOGINNAME, \n";
                query += "SR.REQUEST_LOGINNAME, \n";
                query += "T.TDL_PATIENT_FIRST_NAME, \n";
                query += "T.TDL_PATIENT_LAST_NAME, \n";
                query += "T.TDL_PATIENT_CLASSIFY_ID, \n";
                query += "T.ICD_CODE, \n";
                query += "T.ICD_NAME, \n";
                query += "T.ICD_SUB_CODE, \n";
                query += "T.ICD_TEXT, \n";
                query += "S.test_type_id TEST_COVID_TYPE, \n";
                query += "T.IN_CODE \n";
                query += "FROM HIS_SERE_SERV SS \n";

                query += "LEFT JOIN HIS_SERE_SERV_EXT SSE ON SSE.SERE_SERV_ID = SS.ID \n";
                query += "join lateral (select 1 from his_treatment t where t.id=ss.TDL_TREATMENT_ID) t on t.id=ss.TDL_TREATMENT_ID\n";
                query += "JOIN HIS_SERVICE S ON S.ID = SS.SERVICE_ID \n";
                query += "LEFT JOIN HIS_PTTT_GROUP PG ON PG.ID = S.PTTT_GROUP_ID \n";
                query += @"LEFT JOIN lateral(select PTTT.PTTT_METHOD_ID, 
PTTT.REAL_PTTT_METHOD_ID, 
PTTT.PTTT_PRIORITY_ID, 
PTTT.PTTT_TABLE_ID, 
PTTT.EMOTIONLESS_RESULT_ID, 
PTTT.EMOTIONLESS_METHOD_SECOND_ID, 
PTTT.EMOTIONLESS_METHOD_ID, 
PTTT.BEFORE_PTTT_ICD_NAME, 
PTTT.AFTER_PTTT_ICD_NAME, 
PTTT.PTTT_CATASTROPHE_ID, 
PTTT.PTTT_CONDITION_ID, 
PTTT.DEATH_WITHIN_ID, 
PTTT.PTTT_GROUP_ID, 
PTTT.MANNER, 1
from HIS_SERE_SERV_PTTT PTTT WHERE SS.ID = PTTT.SERE_SERV_ID) PTTT ON 1=1
";
                query += "JOIN HIS_SERVICE_REQ SR ON SR.ID = ss.SERVICE_REQ_ID \n";
                //query += "JOIN HIS_DEPARTMENT D ON SS.TDL_REQUEST_DEPARTMENT_ID = D.ID \n";
                //query += "LEFT JOIN HIS_HEIN_APPROVAL H ON SS.TDL_TREATMENT_ID = H.TREATMENT_ID \n";
                query += "WHERE SS.IS_DELETE = 0  and sr.is_delete=0 and sr.is_no_execute is null\n";
                if (!string.IsNullOrWhiteSpace(filter.PATIENT_CLASSIFY_CODE__TMs))
                {
                    query += string.Format("and (t.TDL_PATIENT_CLASSIFY_ID is null or t.TDL_PATIENT_CLASSIFY_ID not in (select id from his_patient_classify where patient_classify_code in ('{0}') ) ) \n", (filter.PATIENT_CLASSIFY_CODE__TMs ?? "").Replace(",", "','"));
                }
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID IN ({0})\n ", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID IN ({0})\n ", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.EXCLUDE_EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID NOT IN ({0})\n ", string.Join(",", filter.EXCLUDE_EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 1)
                {
                    query += "AND EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 2)
                {
                    query += "AND NOT EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAUSE == 1)
                {
                    query += "AND T.IS_PAUSE=1 \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAUSE == 2)
                {
                    query += "AND T.IS_PAUSE IS NULL \n";
                }
                if (filter.IS_NOT_FEE.HasValue)
                {
                    if (filter.IS_NOT_FEE == 1)
                    {
                        query += "AND (SSE.IS_FEE IS NULL OR SSE.IS_FEE <> 1) ";
                    }
                    else
                    {
                        query += "AND SSE.IS_FEE = 1 ";
                    }
                }

                if (filter.IS_NOT_GATHER_DATA.HasValue)
                {
                    if (filter.IS_NOT_GATHER_DATA == 1)
                    {
                        query += "AND (SSE.IS_GATHER_DATA IS NULL OR SSE.IS_GATHER_DATA <> 1) ";
                    }
                    else
                    {
                        query += "AND SSE.IS_GATHER_DATA = 1 ";
                    }
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                //loai pt tt theo danh muc
                else if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                        {
                            if (filter.IS_SPLIT_PT_TT == true)
                            {
                                query += string.Format("AND PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                            }
                            else
                            {
                                query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}') and ss.tdl_service_type_id <> {1}\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                            }

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",",filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                        {
                            if (filter.IS_SPLIT_PT_TT == true)
                            {
                                query += string.Format("AND PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                            }
                            else
                            {
                                query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}') and ss.tdl_service_type_id <>{1}\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                            }

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                }
                else
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                    {
                        query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                    }
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                    {
                        query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                    } 
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                if (filter.IS_NT_NGT_0.HasValue)
                {
                    if (filter.IS_NT_NGT_0.Value == 0)
                    {
                        query += string.Format("AND (T.TDL_TREATMENT_TYPE_ID <> {0} or SR.INTRUCTION_TIME<T.CLINICAL_IN_TIME) ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                    else if (filter.IS_NT_NGT_0.Value == 1)
                    {
                        query += string.Format("AND T.TDL_TREATMENT_TYPE_ID = {0} AND SR.INTRUCTION_TIME>=T.CLINICAL_IN_TIME ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                }

                //noi tru ngoai tru theo khoa chi dinh
                //Dien dieu tri duoc dung khi tinh cong PTTT doi voi khoa chi dinh dich vu
                if (filter.IS_NT_NGT.HasValue)
                {
                    if (filter.IS_NT_NGT.Value == 0)
                    {
                        query += string.Format("AND ss.tdl_request_department_id in (select id from his_department where req_surg_treatment_type_id = {0}) ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                    }
                    else if (filter.IS_NT_NGT.Value == 1)
                    {
                        query += string.Format("AND ss.tdl_request_department_id in (select id from his_department where req_surg_treatment_type_id = {0}) ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                }

                FilterTime(ref query, filter);

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND T.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND T.TDL_PATIENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_CLASSIFY_ID != null)
                {
                    query += string.Format("AND T.TDL_PATIENT_CLASSIFY_ID = {0} \n", filter.PATIENT_CLASSIFY_ID);
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n ", string.Join(",", filter.SERVICE_IDs));
                }
                if (filter.EXCLUDE_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID  NOT IN ({0})\n ", string.Join(",", filter.EXCLUDE_SERVICE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00662RDO>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private void FilterTime(ref string query, Mrs00662Filter filter)
        {
            if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
            {
                if (filter.TAKE_MONTH == true)
                {
                    if (filter.ONLY_MONTH_FROM != null && filter.ONLY_MONTH_TO != null)
                    {
                        filter.TIME_FROM = (filter.ONLY_MONTH_FROM ?? 0);
                        DateTime timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.ONLY_MONTH_TO ?? 0) ?? new DateTime();
                        filter.TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(timeTo.Year, timeTo.Month, DateTime.DaysInMonth(timeTo.Year, timeTo.Month), 23, 59, 59));
                        query += string.Format("AND SSE.END_TIME <= {0} ", filter.TIME_TO);
                        query += string.Format("AND SSE.END_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
                else
                {
                    if (filter.TIME_TO.HasValue)
                    {
                        query += string.Format("AND SSE.END_TIME <= {0} ", filter.TIME_TO);
                    }
                    if (filter.TIME_FROM.HasValue)
                    {
                        query += string.Format("AND SSE.END_TIME >= {0} ", filter.TIME_FROM);
                    }
                }

            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
            {
                if (filter.TAKE_MONTH == true)
                {
                    if (filter.ONLY_MONTH_FROM != null && filter.ONLY_MONTH_TO != null)
                    {
                        filter.TIME_FROM = (filter.ONLY_MONTH_FROM ?? 0);
                        DateTime timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.ONLY_MONTH_TO ?? 0) ?? new DateTime();
                        filter.TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(timeTo.Year, timeTo.Month, DateTime.DaysInMonth(timeTo.Year, timeTo.Month), 23, 59, 59));
                        query += string.Format("AND T.OUT_TIME <= {0} ", filter.TIME_TO);
                        query += string.Format("AND T.OUT_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
                else
                {
                    if (filter.TIME_TO.HasValue)
                    {
                        query += string.Format("AND T.OUT_TIME <= {0} ", filter.TIME_TO);
                    }
                    if (filter.TIME_FROM.HasValue)
                    {
                        query += string.Format("AND T.OUT_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
            {
                if (filter.TAKE_MONTH == true)
                {
                    if (filter.ONLY_MONTH_FROM != null && filter.ONLY_MONTH_TO != null)
                    {
                        filter.TIME_FROM = (filter.ONLY_MONTH_FROM ?? 0);
                        DateTime timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.ONLY_MONTH_TO ?? 0) ?? new DateTime();
                        filter.TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(timeTo.Year, timeTo.Month, DateTime.DaysInMonth(timeTo.Year, timeTo.Month), 23, 59, 59));
                        query += string.Format("AND T.IN_TIME <= {0} ", filter.TIME_TO);
                        query += string.Format("AND T.IN_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
                else
                {
                    if (filter.TIME_TO.HasValue)
                    {
                        query += string.Format("AND T.IN_TIME <= {0} ", filter.TIME_TO);
                    }
                    if (filter.TIME_FROM.HasValue)
                    {
                        query += string.Format("AND T.IN_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
            {
                if (filter.TAKE_MONTH == true)
                {
                    if (filter.ONLY_MONTH_FROM != null && filter.ONLY_MONTH_TO != null)
                    {
                        filter.TIME_FROM = (filter.ONLY_MONTH_FROM ?? 0);
                        DateTime timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.ONLY_MONTH_TO ?? 0) ?? new DateTime();
                        filter.TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(timeTo.Year, timeTo.Month, DateTime.DaysInMonth(timeTo.Year, timeTo.Month), 23, 59, 59));
                        query += string.Format("AND SS.TDL_INTRUCTION_TIME <= {0} ", filter.TIME_TO);
                        query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
                else
                {
                    if (filter.TIME_TO.HasValue)
                    {
                        query += string.Format("AND SS.TDL_INTRUCTION_TIME <= {0} ", filter.TIME_TO);
                    }
                    if (filter.TIME_FROM.HasValue)
                    {
                        query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
            {
                if (filter.TAKE_MONTH == true)
                {
                    if (filter.ONLY_MONTH_FROM != null && filter.ONLY_MONTH_TO != null)
                    {
                        filter.TIME_FROM = (filter.ONLY_MONTH_FROM ?? 0);
                        DateTime timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.ONLY_MONTH_TO ?? 0) ?? new DateTime();
                        filter.TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(timeTo.Year, timeTo.Month, DateTime.DaysInMonth(timeTo.Year, timeTo.Month), 23, 59, 59));
                        query += string.Format("AND T.HEIN_LOCK_TIME <= {0} ", filter.TIME_TO);
                        query += string.Format("AND T.HEIN_LOCK_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
                else
                {
                    if (filter.TIME_TO.HasValue)
                    {
                        query += string.Format("AND T.HEIN_LOCK_TIME <= {0} ", filter.TIME_TO);
                    }
                    if (filter.TIME_FROM.HasValue)
                    {
                        query += string.Format("AND T.HEIN_LOCK_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
            {
                if (filter.TAKE_MONTH == true)
                {
                    if (filter.ONLY_MONTH_FROM != null && filter.ONLY_MONTH_TO != null)
                    {
                        filter.TIME_FROM = (filter.ONLY_MONTH_FROM ?? 0);
                        DateTime timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.ONLY_MONTH_TO ?? 0) ?? new DateTime();
                        filter.TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(timeTo.Year, timeTo.Month, DateTime.DaysInMonth(timeTo.Year, timeTo.Month), 23, 59, 59));
                        query += string.Format("AND T.FEE_LOCK_TIME <= {0} ", filter.TIME_TO);
                        query += string.Format("AND T.FEE_LOCK_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
                else
                {
                    if (filter.TIME_TO.HasValue)
                    {
                        query += string.Format("AND T.FEE_LOCK_TIME <= {0} ", filter.TIME_TO);
                    }
                    if (filter.TIME_FROM.HasValue)
                    {
                        query += string.Format("AND T.FEE_LOCK_TIME >= {0} ", filter.TIME_FROM);
                    }
                }
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
            {
                if (filter.TAKE_MONTH == true)
                {
                    if (filter.ONLY_MONTH_FROM != null && filter.ONLY_MONTH_TO != null)
                    {
                        filter.TIME_FROM = (filter.ONLY_MONTH_FROM ?? 0);
                        DateTime timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.ONLY_MONTH_TO ?? 0) ?? new DateTime();
                        filter.TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(timeTo.Year, timeTo.Month, DateTime.DaysInMonth(timeTo.Year, timeTo.Month), 23, 59, 59));
                        query += string.Format("AND ss.id in (select sere_serv_id from his_sere_serv_bill where bill_id in (select id from his_transaction where is_cancel is null and transaction_time between {0} and {1}))\n ", filter.TIME_FROM, filter.TIME_TO);
                    }
                }
                else
                {
                    if (filter.TIME_TO.HasValue)
                    {
                        query += string.Format("AND ss.id in (select sere_serv_id from his_sere_serv_bill where bill_id in (select id from his_transaction where is_cancel is null and transaction_time between {0} and {1}))\n ", filter.TIME_FROM, filter.TIME_TO);
                    }
                }
            }
            else
            {
                if (filter.TAKE_MONTH == true)
                {
                    if (filter.ONLY_MONTH_FROM != null && filter.ONLY_MONTH_TO != null)
                    {
                        filter.TIME_FROM = (filter.ONLY_MONTH_FROM ?? 0);
                        DateTime timeTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.ONLY_MONTH_TO ?? 0) ?? new DateTime();
                        filter.TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(timeTo.Year, timeTo.Month, DateTime.DaysInMonth(timeTo.Year, timeTo.Month), 23, 59, 59));

                    }
                }

                query += string.Format("AND (SSE.END_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.INPUT_DATA_ID__TIME_TYPE == 1)
                {
                    query += string.Format("OR T.IN_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID__TIME_TYPE == 2)
                {
                    query += string.Format("OR T.OUT_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID__TIME_TYPE == 3)
                {
                    query += string.Format("OR SR.INTRUCTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID__TIME_TYPE == 4)
                {
                    query += string.Format("OR T.HEIN_LOCK_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                query += string.Format(") \n", filter.TIME_FROM, filter.TIME_TO);
                


            }
        }

        internal List<Mrs00662RDOPTTM> GetPTTM(Mrs00662Filter filter)
        {
            List<Mrs00662RDOPTTM> result = new List<Mrs00662RDOPTTM>();
            try
            {
                string query = "--danh sach phau thuat thu thuat theo thoi gian \n";
                query += "select \n";
                query += "ss.*, \n";
                query += "sse.note, \n";

                query += "T.TDL_TREATMENT_TYPE_ID, \n";
                query += "T.TDL_PATIENT_TYPE_ID, \n";
                query += "t.TREATMENT_CODE, \n";
                query += "t.TDL_PATIENT_NAME, \n";
                query += "t.TDL_PATIENT_ADDRESS, \n";
                query += "t.TDL_PATIENT_DOB, \n";
                query += "SR.EXECUTE_LOGINNAME, \n";
                query += "SR.REQUEST_LOGINNAME, \n";
                query += "t.TDL_PATIENT_GENDER_ID, \n";
                query += "t.TDL_PATIENT_WORK_PLACE, \n";
                query += "s.test_type_id TEST_COVID_TYPE \n";
                query += "from his_sere_serv ss \n";
                query += "left join his_sere_serv_ext sse on sse.SERE_SERV_ID = ss.ID \n";
                query += "JOIN HIS_SERVICE S ON S.ID = SS.SERVICE_ID \n";
                query += "LEFT JOIN HIS_PTTT_GROUP PG ON PG.ID = S.PTTT_GROUP_ID \n";
                query += "join his_service_req sr on sr.id = ss.service_req_id \n";
                query += "join lateral (select 1 from his_treatment t where t.id=ss.TDL_TREATMENT_ID) t on t.id=ss.TDL_TREATMENT_ID\n";
                query += "where 1=1 and ss.is_delete=0 and sr.is_delete=0 and sr.is_no_execute is null\n";

                query += string.Format("and (t.TDL_PATIENT_CLASSIFY_ID  in (select id from his_patient_classify where patient_classify_code in ('{0}') ) or t.icd_code in ('N64','Z41.1')) \n", (filter.PATIENT_CLASSIFY_CODE__TMs ?? "").Replace(",", "','"));
                query += "and (ss.is_no_execute is null or ss.is_no_execute <> 1) \n";
                //query += "ss.is_no_execute is null or ss.is_no_execute <> 1 \n";

                FilterTime(ref query, filter);

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND t.TDL_PATIENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID IN ({0})\n ", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_ROOM_ID IN ({0})\n ", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_ROOM_IDs IN ({0})\n ", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n ", string.Join(",", filter.SERVICE_IDs));
                }
                if (filter.EXCLUDE_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID  NOT IN ({0})\n ", string.Join(",", filter.EXCLUDE_SERVICE_IDs));
                }
                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                //loai pt tt theo danh muc
                else if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                        {
                            if (filter.IS_SPLIT_PT_TT == true)
                            {
                                query += string.Format("AND PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                            }
                            else
                            {
                                query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}') and ss.tdl_service_type_id <>{1}\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                            }

                        } 
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                        {
                            if (filter.IS_SPLIT_PT_TT == true)
                            {
                                query += string.Format("AND PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                            }
                            else
                            {
                                query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}') and ss.tdl_service_type_id <>{1}\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                            }

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                }
                else
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                    {
                        query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                    }
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                    {
                        query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                    }
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00662RDOPTTM>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<Mrs00662RDO> GetChild(Mrs00662Filter filter)
        {
            List<Mrs00662RDO> result = new List<Mrs00662RDO>();
            try
            {
                string query = "--danh sach phau thuat thu thuat theo thoi gian ket thuc o man hinh xu ly pttt\n";
                query += "SELECT\n";
                query += string.Format("SUM(CASE WHEN CHILD.TDL_SERVICE_TYPE_ID = {0} AND CHILD.IS_EXPEND = 1 THEN (CHILD.PRICE*CHILD.AMOUNT*(1 + NVL(CHILD.VAT_RATIO,0))) ELSE 0 END) AS MEDICINE_HP_PRICE,\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC);
                query += string.Format("SUM(CASE WHEN CHILD.TDL_SERVICE_TYPE_ID = {0} AND CHILD.IS_EXPEND = 1 THEN (CHILD.PRICE*CHILD.AMOUNT*(1 + NVL(CHILD.VAT_RATIO,0))) ELSE 0 END) AS MATERIAL_HP_PRICE,\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT);
                query += string.Format("SUM(CASE WHEN CHILD.TDL_SERVICE_TYPE_ID = {0} AND CHILD.IS_EXPEND IS NULL THEN (CHILD.PRICE*CHILD.AMOUNT*(1 + NVL(CHILD.VAT_RATIO,0))) ELSE 0 END) AS MEDICINE_DK_PRICE,\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC);
                query += string.Format("SUM(CASE WHEN CHILD.TDL_SERVICE_TYPE_ID = {0} AND CHILD.IS_EXPEND IS NULL THEN (CHILD.PRICE*CHILD.AMOUNT*(1 + NVL(CHILD.VAT_RATIO,0))) ELSE 0 END) AS MATERIAL_DK_PRICE,\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT);
                query += "CHILD.TDL_TREATMENT_ID CHILD_TREATMENT_ID, \n";
                query += "SS.SERVICE_REQ_ID TDL_SERVICE_REQ_ID \n";
                query += "FROM HIS_SERE_SERV SS \n";

                query += "join lateral (select 1 from his_treatment t where t.id=ss.TDL_TREATMENT_ID) t on t.id=ss.TDL_TREATMENT_ID\n";
                query += "JOIN HIS_SERVICE S ON S.ID = SS.SERVICE_ID \n";
                query += "LEFT JOIN HIS_PTTT_GROUP PG ON PG.ID = S.PTTT_GROUP_ID \n";

                query += "JOIN HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID \n";
                query += "JOIN HIS_SERE_SERV_EXT SSE ON SSE.SERE_SERV_ID = SS.ID \n";
                query += "JOIN HIS_SERE_SERV CHILD ON SS.ID = CHILD.PARENT_ID \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";
                query += string.Format("AND CHILD.TDL_SERVICE_TYPE_ID IN ({0},{1})\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC);
                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0},{1})\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID IN ({0})\n ", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 1)
                {
                    query += "AND EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 2)
                {
                    query += "AND NOT EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                //if (filter.INPUT_DATA_ID_STT_PAUSE ==1)
                //{
                //    query += "AND T.IS_PAUSE=1 \n";
                //}
                //if (filter.INPUT_DATA_ID_STT_PAUSE ==2)
                //{
                //    query += "AND T.IS_PAUSE IS NULL \n";
                //}

                FilterTime(ref query, filter);

                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n ", string.Join(",", filter.SERVICE_IDs));
                }
                if (filter.EXCLUDE_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID  NOT IN ({0})\n ", string.Join(",", filter.EXCLUDE_SERVICE_IDs));
                }
                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                //loai pt tt theo danh muc
                else if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                        {
                            if (filter.IS_SPLIT_PT_TT == true)
                            {
                                query += string.Format("AND PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                            }
                            else
                            {
                                query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}') and ss.tdl_service_type_id <>{1}\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                            }

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                        {
                            if (filter.IS_SPLIT_PT_TT == true)
                            {
                                query += string.Format("AND PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                            }
                            else
                            {
                                query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}') and ss.tdl_service_type_id <>{1}\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                            }

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                }
                else
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                    {
                        query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                    }
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                    {
                        query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                    }
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                query += "GROUP BY SS.SERVICE_REQ_ID, CHILD.TDL_TREATMENT_ID \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00662RDO>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        internal List<HIS_DEBATE> GetDebate(Mrs00662Filter filter)
        {
            List<HIS_DEBATE> result = new List<HIS_DEBATE>();
            try
            {
                string query = "--hoi chan\n";
                query += "SELECT\n";
                query += "DISTINCT DB.*\n";

                query += "FROM HIS_SERE_SERV SS \n";

                query += "LEFT JOIN HIS_SERE_SERV_EXT SSE ON SSE.SERE_SERV_ID = SS.ID \n";
                query += "join lateral (select 1 from his_treatment t where t.id=ss.TDL_TREATMENT_ID) t on t.id=ss.TDL_TREATMENT_ID\n";
                query += "JOIN HIS_DEBATE DB ON T.ID = DB.TREATMENT_ID \n";
                //query += "JOIN HIS_SERVICE S ON S.ID = SS.SERVICE_ID \n";
                //query += "LEFT JOIN HIS_PTTT_GROUP PG ON PG.ID = S.PTTT_GROUP_ID \n";
                //query += @"LEFT JOIN HIS_SERE_SERV_PTTT PTTT ON SS.ID = PTTT.SERE_SERV_ID ";
                query += "JOIN HIS_SERVICE_REQ SR ON SR.ID = ss.SERVICE_REQ_ID \n";
                //query += "JOIN HIS_DEPARTMENT D ON SS.TDL_REQUEST_DEPARTMENT_ID = D.ID \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";

                if (filter.INPUT_DATA_ID_STT_PAY == 1)
                {
                    query += "AND EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 2)
                {
                    query += "AND NOT EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAUSE == 1)
                {
                    query += "AND T.IS_PAUSE=1 \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAUSE == 2)
                {
                    query += "AND T.IS_PAUSE IS NULL \n";
                }
                if (filter.IS_NOT_FEE.HasValue)
                {
                    if (filter.IS_NOT_FEE == 1)
                    {
                        query += "AND (SSE.IS_FEE IS NULL OR SSE.IS_FEE <> 1) ";
                    }
                    else
                    {
                        query += "AND SSE.IS_FEE = 1 ";
                    }
                }

                if (filter.IS_NOT_GATHER_DATA.HasValue)
                {
                    if (filter.IS_NOT_GATHER_DATA == 1)
                    {
                        query += "AND (SSE.IS_GATHER_DATA IS NULL OR SSE.IS_GATHER_DATA <> 1) ";
                    }
                    else
                    {
                        query += "AND SSE.IS_GATHER_DATA = 1 ";
                    }
                }
                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                //loai pt tt theo danh muc
                else if (filter.SERVICE_TYPE_IDs == null)
                {
                    if (filter.IS_PT_TT == 1)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                        {
                            if (filter.IS_SPLIT_PT_TT == true)
                            {
                                query += string.Format("AND ss.service_id in (select id from v_his_service where PTTT_GROUP_CODE in ('{0}'))\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                            }
                            else
                            {
                                query += string.Format("OR ss.service_id in (select id from v_his_service where PTTT_GROUP_CODE in ('{0}')) and SS.TDL_SERVICE_TYPE_ID <> {1}\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                            }

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                    else if (filter.IS_PT_TT == 0)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                        {
                            if (filter.IS_SPLIT_PT_TT == true)
                            {
                                query += string.Format("AND ss.service_id in (select id from v_his_service where PTTT_GROUP_CODE in ('{0}'))\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                            }
                            else
                            {
                                query += string.Format("OR ss.service_id in (select id from v_his_service where PTTT_GROUP_CODE in ('{0}')) and ss.tdl_service_type_id <>{1}\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                            }

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                }
                else
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                    {
                        query += string.Format("OR ss.service_id in (select id from v_his_service where PTTT_GROUP_CODE in ('{0}'))\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                    }
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                    {
                        query += string.Format("OR ss.service_id in (select id from v_his_service where PTTT_GROUP_CODE in ('{0}'))\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                    }
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }

                if (filter.IS_NT_NGT_0.HasValue)
                {
                    if (filter.IS_NT_NGT_0.Value == 0)
                    {
                        query += string.Format("AND (T.TDL_TREATMENT_TYPE_ID <> {0} or SR.INTRUCTION_TIME<T.CLINICAL_IN_TIME) ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                    else if (filter.IS_NT_NGT_0.Value == 1)
                    {
                        query += string.Format("AND T.TDL_TREATMENT_TYPE_ID = {0} AND SR.INTRUCTION_TIME>=T.CLINICAL_IN_TIME ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                }

                //noi tru ngoai tru theo khoa chi dinh
                //Dien dieu tri duoc dung khi tinh cong PTTT doi voi khoa chi dinh dich vu
                if (filter.IS_NT_NGT.HasValue)
                {
                    if (filter.IS_NT_NGT.Value == 0)
                    {
                        query += string.Format("AND ss.tdl_request_department_id in (select id from his_department where req_surg_treatment_type_id = {0}) ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                    }
                    else if (filter.IS_NT_NGT.Value == 1)
                    {
                        query += string.Format("AND ss.tdl_request_department_id in (select id from his_department where req_surg_treatment_type_id = {0}) ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                }


                FilterTime(ref query, filter);

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND T.TDL_PATIENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_CLASSIFY_ID != null)
                {
                    query += string.Format("AND T.TDL_PATIENT_CLASSIFY_ID = {0} \n", filter.PATIENT_CLASSIFY_ID);
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n ", string.Join(",", filter.SERVICE_IDs));
                }
                if (filter.EXCLUDE_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID  NOT IN ({0})\n ", string.Join(",", filter.EXCLUDE_SERVICE_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEBATE>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<HIS_DEBATE_TYPE> GetDebateType()
        {
            List<HIS_DEBATE_TYPE> result = new List<HIS_DEBATE_TYPE>();
            try
            {
                string query = "select * from his_debate_type";
                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEBATE_TYPE>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }

        public List<HIS_HEIN_APPROVAL> GetHeinApproval(Mrs00662Filter filter)
        {
            List<HIS_HEIN_APPROVAL> result = new List<HIS_HEIN_APPROVAL>();
            try
            {
                string query = string.Format("select * from his_hein_approval where execute_time between {0} and {1}", filter.TIME_FROM, filter.TIME_TO);
                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_HEIN_APPROVAL>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }
        internal List<HIS_PATIENT_CLASSIFY> GetPatientClassify()
        {
            List<HIS_PATIENT_CLASSIFY> result = new List<HIS_PATIENT_CLASSIFY>();
            try
            {
                string query = "SELECT * FROM HIS_PATIENT_CLASSIFY";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;

        }

        public List<HIS_SERVICE> GetService()
        {
            List<HIS_SERVICE> result = new List<HIS_SERVICE>();
            try
            {
                string query = "SELECT * FROM HIS_SERVICE";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
