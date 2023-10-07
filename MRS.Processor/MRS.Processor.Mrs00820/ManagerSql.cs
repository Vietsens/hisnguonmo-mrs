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
using MRS.Proccessor.Mrs00820;

namespace MRS.Processor.Mrs00820
{
    public partial class ManagerSql
    {
        public List<System.Data.DataTable> GetSum(Mrs00820Filter filter, string query)
        {
            List<System.Data.DataTable> result = new List<DataTable>();
            try
            {
                PropertyInfo[] p = typeof(Mrs00820Filter).GetProperties();
                foreach (var item in p)
                {
                    if (item.PropertyType == typeof(long))
                    {
                        long value = (long)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value.ToString());
                    }
                    else if (item.PropertyType == typeof(long?))
                    {
                        long? value = (long?)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value.HasValue ? value.Value.ToString() : "''");
                    }
                    else if (item.PropertyType == typeof(string))
                    {
                        string value = (string)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, string.IsNullOrWhiteSpace(value) ? "''" : value);
                    }
                    else if (item.PropertyType == typeof(List<long>))
                    {
                        List<long> value = (List<long>)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value != null ? string.Join(",", value) : "''");
                    }
                    else if (item.PropertyType == typeof(List<string>))
                    {
                        List<string> value = (List<string>)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value != null ? "'" + string.Join("','", value) + "'" : "''");
                    }
                    else if (item.PropertyType == typeof(bool?))
                    {
                        bool? value = (bool?)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value != null ? (value == true ? "1" : "0") : "''");
                    }
                }

                Inventec.Common.Logging.LogSystem.Info(query);
                foreach (var item in query.Split(';'))
                {
                    List<string> errors = new List<string>();
                    result.Add(new MOS.DAO.Sql.SqlDAO().Execute(item, ref errors) ?? new DataTable());
                    Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<TREATMENT_BED_ROOM> GetTreatment(Mrs00820Filter filter)
        {
            List<TREATMENT_BED_ROOM> result = null;
            try
            {
                string query = "";
                query += string.Format("-- du lieu vao ra buong\n");
                query += string.Format("select\n");
                query += string.Format("tbr.id,\n");
                query += string.Format("LastTbr.Last_id,\n");
                query += string.Format("tbr.treatment_id,\n");
                query += string.Format("trea.treatment_end_type_id,\n");
                query += string.Format("trea.tdl_treatment_type_id,\n");
                query += string.Format("trea.tdl_patient_type_id,\n");
                query += string.Format("trea.tdl_patient_gender_id,\n");
                query += string.Format("trea.treatment_result_id,\n");
                query += string.Format("trea.TREATMENT_DAY_COUNT,\n");
                query += string.Format("trea.IN_TIME,\n");
                query += string.Format("nvl(trea.CLINICAL_IN_TIME,0) as CLINICAL_IN_TIME,\n");
                query += string.Format("(case when trea.is_pause=1 then trea.out_time else 0 end) as OUT_TIME,\n");
                query += string.Format("trea.TDL_PATIENT_NATIONAL_NAME, \n");
                query += string.Format("trea.IS_EMERGENCY,\n");
                query += string.Format("trea.IS_PAUSE,\n");
                if (!string.IsNullOrWhiteSpace(filter.EXECUTE_ROOM_CODE__PKKs))
                {
                    query += string.Format("(select room_code from v_his_room where id=trea.in_room_id) IN_ROOM_CODE,\n");
                }
                query += string.Format("dea.DEATH_WITHIN_NAME,\n");
                query += string.Format("trea.treatment_code,\n");
                query += string.Format("trea.tdl_patient_dob,\n");
                query += string.Format("pc.patient_classify_code,\n");
                query += string.Format("pc.patient_classify_name,\n");
                query += string.Format("tmet.treatment_end_type_code,\n");
                query += string.Format("tmrs.treatment_result_code,\n");
                query += string.Format("preTbr.previous_department_id,\n");
                query += string.Format("ro.department_id,\n");
                //query += string.Format("case when trea.end_department_id is null then trea.last_department_id else trea.end_department_id end as department_id,\n");
                query += string.Format("nextTbr.next_department_id,\n");
                query += string.Format("(case when tbr.add_time < {0} then {0}-1 else tbr.add_time end) add_time,\n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("(case when tbr.remove_time is null or tbr.remove_time> {1} then {1}+1 else tbr.remove_time end) remove_time,\n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("trea.tdl_hein_card_number,\n");
                query += string.Format("(case when tbr.add_time<{0} and tbr.remove_time between {0} and {1} then 1\n", filter.TIME_FROM, filter.TIME_TO);//vào buồng trước khoảng, ra buồng trong khoảng
                query += string.Format("when tbr.add_time between {0} and {1} and tbr.remove_time between {0} and {1} then 2\n", filter.TIME_FROM, filter.TIME_TO);//vào buồng trong khoảng, ra buồng trong khoảng
                query += string.Format("when tbr.add_time between {0} and {1} and (tbr.remove_time is null or tbr.remove_time>{1}) then 3\n", filter.TIME_FROM, filter.TIME_TO);//vào buồng trong khoảng, ra buồng sau khoảng
                query += string.Format("when tbr.add_time <{0} and (tbr.remove_time is null or tbr.remove_time>{1}) then 4 else 0 end) type\n", filter.TIME_FROM, filter.TIME_TO);//vào buồng trước khoảng, ra buồng sau khoảng

                query += string.Format("from his_treatment_bed_room tbr\n");

                //lấy khoa ra gần nhất trước thời gian time from
                query += string.Format("left join lateral (select max(ro.department_id) keep(dense_rank last order by remove_time) previous_department_id from his_treatment_bed_room preTbr join his_bed_room br on br.id=preTbr.bed_room_id join his_room ro on ro.id=br.room_id where preTbr.remove_time< {0} and preTbr.treatment_id=tbr.treatment_id) preTbr on 1=1\n", filter.TIME_FROM);

                //lấy khoa vào tiếp sau thời gian time to
                query += string.Format("left join lateral (select min(ro.department_id) keep(dense_rank first order by add_time) next_department_id from his_treatment_bed_room nextTbr join his_bed_room br on br.id=nextTbr.bed_room_id join his_room ro on ro.id=br.room_id where nextTbr.add_time> {0} and nextTbr.treatment_id=tbr.treatment_id) nextTbr on 1=1\n", filter.TIME_TO);

                //lấy vào buồng cuối cùng
                query += string.Format("join lateral (select max(lastTbr.id) keep(dense_rank last order by add_time) last_id from his_treatment_bed_room lastTbr where lastTbr.treatment_id=tbr.treatment_id) lastTbr on 1=1\n");

                query += string.Format("join his_treatment trea on trea.id=tbr.treatment_id\n");
                query += string.Format("join his_bed_room br on br.id=tbr.bed_room_id\n");
                query += string.Format("join his_room ro on ro.id=br.room_id\n");
                query += string.Format("left join his_patient_classify pc on pc.id=trea.tdl_patient_classify_id\n");
                query += string.Format("left join his_treatment_end_type tmet on tmet.id=trea.treatment_end_type_id\n");
                query += string.Format("left join his_treatment_result tmrs on tmrs.id=trea.treatment_result_id\n");
                query += string.Format("left join his_death_within dea on dea.id=trea.DEATH_WITHIN_ID\n");
                query += string.Format("join his_department depa on depa.id = trea.last_department_id \n ");
                query += string.Format("where 1=1 and tbr.co_treatment_id is null\n");
                //query += string.Format("and trea.tdl_treatment_type_id = {0}\n",IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                query += string.Format("and (tbr.add_time<{0} and tbr.remove_time between {0} and {1}\n",filter.TIME_FROM,filter.TIME_TO);//vào buồng trước khoảng, ra buồng trong khoảng
                query += string.Format("or tbr.add_time between {0} and {1} and tbr.remove_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);//vào buồng trong khoảng, ra buồng trong khoảng
                query += string.Format("or tbr.add_time between {0} and {1} and (tbr.remove_time is null or tbr.remove_time>{1})\n", filter.TIME_FROM, filter.TIME_TO);//vào buồng trong khoảng, ra buồng sau khoảng
                query += string.Format("or tbr.add_time <{0} and (tbr.remove_time is null or tbr.remove_time>{1})\n", filter.TIME_FROM, filter.TIME_TO);//vào buồng trước khoảng, ra buồng sau khoảng
                query += string.Format("or tbr.id=lastTbr.last_id and tbr.remove_time <{0} and trea.out_time>{0})\n", filter.TIME_FROM, filter.TIME_TO);//là buồng cuối và ra buồng trước khoảng, ra viện trong khoảng hoặc sau khoảng

                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.TDL_PATIENT_TYPE_ID in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }

                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("and trea.TDL_PATIENT_CLASSIFY_ID in ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.TDL_TREATMENT_TYPE_ID in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
              
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT_BED_ROOM>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

       
    }

    public class TREATMENT_BED_ROOM
    {
        public long TREATMENT_ID { get; set; }
        public long? PREVIOUS_DEPARTMENT_ID { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public long? NEXT_DEPARTMENT_ID { get; set; }
        public long? ADD_TIME { get; set; }
        public long? REMOVE_TIME { get; set; }
        public long TYPE { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }

        public string PATIENT_CLASSIFY_CODE { get; set; }
        public string PATIENT_CLASSIFY_NAME { get; set; }

        public string TREATMENT_CODE { get; set; }

        public string TREATMENT_END_TYPE_CODE { get; set; }

        public string TREATMENT_RESULT_CODE { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public string DEATH_WITHIN_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public short? IS_PAUSE { get; set; }
        public short? IS_EMERGENCY { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public decimal? TREATMENT_DAY_COUNT { get; set; }
        public string TDL_PATIENT_NATIONAL_NAME { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public string IN_ROOM_CODE { get; set; }
        public long IN_TIME { get; set; }
        public long OUT_TIME { get; set; }
        public long CLINICAL_IN_TIME { get; set; }
        public long ID { get; set; }
        public long LAST_ID { get; set; }
    }
}
