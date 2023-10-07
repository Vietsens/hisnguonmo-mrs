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
using MRS.Proccessor.Mrs00555;

namespace MRS.Processor.Mrs00555
{
    public partial class ManagerSql : BusinessBase
    {
        public List<System.Data.DataTable> GetSum(Mrs00555Filter filter, string query)
        {
            List<System.Data.DataTable> result = new List<DataTable>();
            try
            {
                PropertyInfo[] p = typeof(Mrs00555Filter).GetProperties();
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

        public List<TREATMENT> GetTreatment(Mrs00555Filter filter)
        {
            List<TREATMENT> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach ho so dieu tri co nhap vien\n");
                query += string.Format("select\n");
                query += string.Format("trea.id,\n");
                query += string.Format("trea.in_time,\n");
                query += string.Format("trea.out_time,\n");
                query += string.Format("trea.in_date,\n");
                query += string.Format("trea.out_date,\n");
                query += string.Format("trea.is_pause,\n");
                query += string.Format("trea.clinical_in_time,\n");
                query += string.Format("trea.tdl_patient_dob, \n");
                query += string.Format("trea.tdl_patient_gender_id,\n");
                query += string.Format("trea.treatment_end_type_id,\n");
                query += string.Format("trea.treatment_result_id,\n");
                query += string.Format("trea.tran_pati_form_id,\n");
                query += string.Format("trea.tdl_hein_card_number,\n");
                query += string.Format("trea.tdl_patient_national_name,\n");
                //query += string.Format("trea.branch_id,\n");
                query += string.Format("trea.in_room_id,\n");
                query += string.Format("trea.is_emergency,\n");
                query += string.Format("trea.treatment_day_count,\n");
                query += string.Format("trea.death_within_id,\n");
                query += string.Format("trea.tdl_patient_type_id,\n");
                query += string.Format("trea.tdl_treatment_type_id,\n");
                query += string.Format("trea.TDL_PATIENT_CLASSIFY_ID,\n");
                query += string.Format("trea.treatment_code\n");
                query += string.Format("from his_treatment trea\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);

                query += string.Format("and trea.clinical_in_time <={0}\n", filter.TIME_TO);
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                }
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<HIS_TREATMENT> GetTreatmentAll(Mrs00555Filter filter)
        {
            List<HIS_TREATMENT> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach ho so dieu tri co nhap vien\n");
                query += string.Format("select\n");
                query += string.Format("trea.*\n");
                
                query += string.Format("from his_treatment trea\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);

                query += string.Format("and trea.clinical_in_time <={0}\n", filter.TIME_TO);
                if (filter.BRANCH_ID != null)
                {       
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                }
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


        public List<TREATMENT_BED_ROOM> GetTreatmentBedRoom(Mrs00555Filter filter)
        {
            List<TREATMENT_BED_ROOM> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach dieu tri tai buong\n");
                query += string.Format("select\n");
                query += string.Format("tbr.treatment_id,\n");
                query += string.Format("tbr.add_time,\n");
                query += string.Format("ro.department_id,\n");
                query += string.Format("br.room_id\n");
                
                query += string.Format("from his_treatment_bed_room tbr\n");
                query += string.Format("join his_treatment trea on trea.id=tbr.treatment_id\n");
                query += string.Format("join his_bed_room br on br.id=tbr.bed_room_id\n");
                query += string.Format("join his_room ro on ro.id=br.room_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);

                query += string.Format("and trea.clinical_in_time <={0}\n", filter.TIME_TO);
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                }
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
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


        public List<DEPA_COUNT_BED> GetDepaCountBed(Mrs00555Filter filter)
        {
            List<DEPA_COUNT_BED> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach giuong dang co su dung\n");
                query += string.Format("select\n");
                query += string.Format("bl.department_id,\n");
                query += string.Format("count(distinct(bl.bed_id)) count_bed\n");

                query += string.Format("from his_treatment trea\n");
                query += string.Format("join v_his_bed_log bl on bl.treatment_id = trea.id and bl.bed_type_code = 'H'\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);

                query += string.Format("and trea.clinical_in_time <={0}\n", filter.TIME_TO);
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                }
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                query += string.Format("group by\n");
                query += string.Format("bl.department_id\n");
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<DEPA_COUNT_BED>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<DEPARTMENT_TRAN> GetDepartmentTran(Mrs00555Filter filter)
        {
            List<DEPARTMENT_TRAN> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach chuyen khoa nhap vien\n");
                query += string.Format("select\n");
                query += string.Format("dpt.treatment_id,\n");
                query += string.Format("dpt.department_in_time,\n");
                query += string.Format("dpt.department_id,\n");
                query += string.Format("dpt.id,\n");
                query += string.Format("dpt.previous_id\n");
                query += string.Format("from his_department_tran dpt\n");
                query += string.Format("join his_treatment trea on trea.id=dpt.treatment_id\n");
                query += string.Format("where 1=1 and dpt.department_in_time is not null\n");
                query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);
                query += string.Format("and trea.clinical_in_time <={0}\n", filter.TIME_TO);
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                }
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<DEPARTMENT_TRAN>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<HIS_TREATMENT_BED_ROOM> GetBedRoom(Mrs00555Filter filter)
        {
            List<HIS_TREATMENT_BED_ROOM> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach buong nhap vien\n");
                query += string.Format("select\n");
                query += string.Format("br.treatment_id,\n");
                query += string.Format("br.bed_room_id,\n");
                query += string.Format("from his_treatment_bed_room br\n");
                query += string.Format("join his_treatment trea on trea.id=br.treatment_id\n");
                query += string.Format("where 1=1 \n");
                query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);
                query += string.Format("and trea.clinical_in_time <={0}\n", filter.TIME_TO);
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                }
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT_BED_ROOM>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<PATIENT_TYPE_ALTER> GetPatientTypeAlter(Mrs00555Filter filter)
        {
            List<PATIENT_TYPE_ALTER> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach chuyen doi doi tuong\n");
                query += string.Format("select\n");
                query += string.Format("pta.treatment_id,\n");
                query += string.Format("pta.log_time,\n");
                query += string.Format("pta.department_tran_id,\n");
                query += string.Format("(case when pta.treatment_type_id={1} then {0} else pta.treatment_type_id end) treatment_type_id,\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY);
                query += string.Format("pta.right_route_code,\n");
                query += string.Format("(case when substr(pta.hein_card_number,1,2) ='CA' then 1 else null end) IS_POLICE,\n");
                query += string.Format("pta.patient_type_id\n");
                query += string.Format("from his_patient_type_alter pta\n");
                query += string.Format("join his_treatment trea on trea.id=pta.treatment_id\n");
                query += string.Format("where 1=1 \n");
                query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);
                query += string.Format("and trea.clinical_in_time <={0}\n", filter.TIME_TO);
                query += string.Format("and pta.treatment_type_id in ({0},{1},{2})\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY); 
                if (filter.IS_TREAT_IN.HasValue)
                {
                    if (filter.IS_TREAT_IN.Value == true)
                    {
                        query += string.Format("and pta.treatment_type_id in ({0},{1})\n",IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY); 
                    }
                    else
                    {
                        query += string.Format("and pta.treatment_type_id in ({0})\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU); 
                    }
                }
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<PATIENT_TYPE_ALTER>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamPatient(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " --So benh nhan kham da chi dinh tu phong tiep don theo thoi gian chi dinh\n";
                query += "select \n";
                query += "count(distinct(case when trea.tdl_patient_type_id =1 then trea.treatment_code else '' end)) as CountBhyt, \n";
                query += "count(distinct(case when trea.tdl_patient_type_id <>1 then trea.treatment_code else '' end)) as CountVp \n";

                query += "from his_service_req sr  \n";
                query += "join his_treatment trea on sr.treatment_id = trea.id  \n";
                

                query += "where 1=1 \n";
                query += "and sr.is_no_execute is null  \n";
                query += "and sr.is_delete =0  \n";
                query += "and sr.service_req_type_id =1  \n";
                query += string.Format("and trea.tdl_treatment_type_id = {0} ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                query += string.Format("AND sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs!=null)
                {
                     query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExam(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " --So yeu cau kham da chi dinh tu phong tiep don theo thoi gian chi dinh\n";
                query += "select \n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, \n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp \n";

                query += "from his_service_req sr  \n";
                query += "join his_treatment trea on sr.treatment_id = trea.id  \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "join v_his_room exro on sr.execute_room_id = exro.id  \n";
                    query += "join his_room rero on sr.request_room_id = rero.id \n";
                    query += "join v_his_bed_room br on trea.end_room_id = br.room_id\n";
                }
                
                query += "where 1=1 \n";
                query += "and sr.is_no_execute is null  \n";
                query += "and sr.is_delete =0  \n";
                query += "and sr.service_req_type_id =1  \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "and rero.room_type_id = 3 \n";
                }
                if (filter.IS_EXAM == true)
                {
                    query += string.Format("and trea.tdl_treatment_type_id = {0} ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                }
                query += string.Format("AND sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamCc(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " -- so yeu cau kham da chi dinh tu phong tiep don theo thoi gian chi dinh, phong cap cuu chi dinh\n";
                query += "SELECT \n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, \n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp \n";

                query += "from his_service_req sr  \n";
                query += "join his_treatment trea on sr.treatment_id = trea.id  \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "join v_his_room exro on sr.execute_room_id = exro.id  \n";
                    query += "join his_room rero on sr.request_room_id = rero.id \n";
                    query += "join v_his_bed_room br on trea.end_room_id = br.room_id\n";
                }

                query += "where 1=1 \n";
                query += "and sr.is_no_execute is null  \n";
                query += "and sr.is_delete =0  \n";
                query += "and sr.service_req_type_id =1  \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "and rero.room_type_id = 3 \n";
                }

                query += string.Format("AND sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                query += string.Format("AND exro.ROOM_CODE = '{0}' \n", filter.ROOM_CODE__CC??"");

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamSan(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " -- so yeu cau kham da chi dinh tu phong tiep don theo thoi gian chi dinh, phong kham san chi dinh\n";
                query += "SELECT \n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, \n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp \n";

                query += "from his_service_req sr  \n";
                query += "join his_treatment trea on sr.treatment_id = trea.id  \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "join v_his_room exro on sr.execute_room_id = exro.id  \n";
                    query += "join his_room rero on sr.request_room_id = rero.id \n";
                    query += "join v_his_bed_room br on trea.end_room_id = br.room_id\n";
                }

                query += "where 1=1 \n";
                query += "and sr.is_no_execute is null  \n";
                query += "and sr.is_delete =0  \n";
                query += "and sr.service_req_type_id =1  \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "and rero.room_type_id = 3 \n";
                }

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                query += string.Format("AND sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                query += string.Format("AND exro.ROOM_CODE = '{0}' \n", filter.ROOM_CODE__SAN ?? "");
               
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamFinishPatient(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " -- so yeu cau kham ket thuc kham va hen kham hoac cap toa cho ve tai phong\n";
                query += "SELECT \n";
                query += "count(distinct(case when trea.tdl_patient_type_id =1 then trea.treatment_code else '' end)) as CountBhyt, \n";
                query += "count(distinct(case when trea.tdl_patient_type_id <>1 then trea.treatment_code else '' end)) as CountVp \n";

                query += "from his_treatment trea  \n";
                query += "join his_service_req sr on (sr.execute_room_id=trea.end_room_id and sr.treatment_id=trea.id) \n";
                
                query += "WHERE 1=1 \n";
                query += "and trea.is_pause=1  \n";
                query += "and sr.service_req_stt_id=3\n";
                query += "and sr.is_no_execute is null \n";
                query += "and sr.service_req_type_id =1 \n";
                query += "and sr.is_delete =0\n";
                query += string.Format("and trea.tdl_treatment_type_id = {0} ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                query += "and trea.treatment_end_type_id in (3,4) \n";//loai ra vien la cap toa hoac hen kham

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }
                query += string.Format("and sr.finish_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);


                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamFinish(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " -- so yeu cau kham ket thuc kham va hen kham hoac cap toa cho ve tai phong\n";
                query += "SELECT \n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, \n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp \n";

                query += "from his_treatment trea  \n";
                query += "join his_service_req sr on (sr.execute_room_id=trea.end_room_id and sr.treatment_id=trea.id) \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "join v_his_room ro on trea.end_room_id = ro.id \n";
                    query += "join v_his_bed_room br on trea.end_room_id = br.room_id\n";
                }
                query += "WHERE 1=1 \n";
                query += "and trea.is_pause=1  \n";
                query += "and sr.service_req_stt_id=3\n";
                query += "and sr.is_no_execute is null \n";
                query += "and sr.service_req_type_id =1 \n";
                query += "and sr.is_delete =0\n";
                if (filter.IS_EXAM == true)
                {
                    query += string.Format("and trea.tdl_treatment_type_id = {0} ", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                }
                query += "and trea.treatment_end_type_id in (3,4) \n";//loai ra vien la cap toa hoac hen kham

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }
                query += string.Format("and sr.finish_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);


                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamTreatOutPatient(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " -- so yeu cau kham vao dieu tri ngoai tru tai phong kham theo thoi gian ket thuc kham\n";
                query += "SELECT \n";
                query += "count(distinct(case when trea.tdl_patient_type_id =1 then trea.treatment_code else '' end)) as CountBhyt, \n";
                query += "count(distinct(case when trea.tdl_patient_type_id <>1 then trea.treatment_code else '' end)) as CountVp \n";

                query += "from his_treatment trea  \n";
                query += "join his_service_req sr on (sr.execute_room_id=trea.in_room_id and sr.treatment_id=trea.id )  \n";
                
                query += "WHERE 1=1 \n";
                query += "and trea.in_treatment_type_id =2 \n";
                query += "and sr.service_req_stt_id=3 \n";
                query += "and sr.is_no_execute is null  \n";
                query += "and sr.is_delete =0\n";
                query += "and sr.service_req_type_id =1 \n";

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }
                query += string.Format("and sr.finish_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);


                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


        public CountWithPatientType CountExamTreatOut(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " -- so yeu cau kham vao dieu tri ngoai tru tai phong kham theo thoi gian ket thuc kham\n";
                query += "SELECT \n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, \n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp \n";

                query += "from his_treatment trea  \n";
                query += "join his_service_req sr on (sr.execute_room_id=trea.in_room_id and sr.treatment_id=trea.id )  \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "join v_his_room ro on trea.end_room_id = ro.id \n";
                    query += "join v_his_bed_room br on trea.end_room_id = br.room_id\n";
                }
                query += "WHERE 1=1 \n";
                query += "and trea.in_treatment_type_id =2 \n";
                query += "and sr.service_req_stt_id=3 \n";
                query += "and sr.is_no_execute is null  \n";
                query += "and sr.is_delete =0\n";
                query += "and sr.service_req_type_id =1 \n";

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }
                query += string.Format("and sr.finish_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);


                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamTran(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = " --so yeu cau kham ket thuc va chuyen tuyen tai phong theo thoi gian ket thuc\n";
                query += "SELECT\n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt,\n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp\n";

                query += "from his_treatment trea \n";
                query += "join his_service_req sr on (sr.execute_room_id=trea.end_room_id and sr.treatment_id=trea.id) \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "join v_his_room ro on trea.end_room_id = ro.id \n";
                    query += "join v_his_bed_room br on trea.end_room_id = br.room_id\n";
                }
                query += "WHERE 1=1\n";
                query += "and trea.is_pause=1 \n";
                query += "and sr.service_req_stt_id=3\n";
                query += "and sr.is_no_execute is null\n";
                query += "and sr.service_req_type_id =1 \n";
                query += "and sr.is_delete =0\n";
                query += "and trea.treatment_end_type_id =2\n";

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }
                query += string.Format("and sr.finish_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);


                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamDeath(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = "--so yeu cau kham tai phong va tu vong tai phong theo thoi gian ket thuc kham\n";
                query += "SELECT\n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt,\n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp\n";

                query += "from his_treatment trea \n";
                query += "join his_service_req sr on (sr.execute_room_id=trea.end_room_id and sr.treatment_id=trea.id) \n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    query += "join v_his_room ro on trea.end_room_id = ro.id \n";
                    query += "join v_his_bed_room br on trea.end_room_id = br.room_id\n";
                }
                query += "WHERE 1=1 \n";
                query += "and trea.is_pause=1\n";
                query += "and (trea.treatment_end_type_id =1) \n";
                query += "and sr.service_req_stt_id=3\n";
                query += "and sr.is_no_execute is null\n";
                query += "and sr.service_req_type_id =1\n";
                query += "and sr.is_delete =0\n";

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }
                query += string.Format("and sr.finish_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);


                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public CountWithPatientType CountExamOther(Mrs00555Filter filter)
        {
            CountWithPatientType result = null;
            try
            {
                string query = "--so yeu cau kham ket thuc tai phong voi cac loai ra vien khac\n";
                query += "SELECT\n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt,\n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp\n";

                query += "from his_treatment trea\n"; 
                query += "join his_service_req sr on (sr.execute_room_id=trea.end_room_id and sr.treatment_id=trea.id)\n";
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                    query += "join v_his_room ro on trea.end_room_id = ro.id\n";

                query += "WHERE 1=1\n";
                query += "and trea.is_pause=1\n";
                query += "and trea.treatment_end_type_id not in (1,2,3,4)\n";
                query += "and sr.service_req_stt_id=3\n";
                query += "and sr.is_no_execute is null\n";
                query += "and sr.service_req_type_id =1\n";
                query += "and sr.is_delete =0\n";

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.TDL_REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.TDL_REQUEST_DEPARTMENT_IDs));
                }
                query += string.Format("and sr.finish_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);


                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<Patient> GetSave(Mrs00555Filter filter)
        {
            List<Patient> result = null;
            try
            {
                string query = "--cac benh nhan luu tai phong\n";
                query += "SELECT DISTINCT\n";
                query += "TREA.TREATMENT_CODE,\n";
                query += "TREA.TDL_PATIENT_NAME\n";

                query += "FROM his_rs.his_department_tran dt\n";
                query += "JOIN his_rs.his_department in_depa ON dt.department_id = in_depa.id\n";
                query += "JOIN his_rs.his_treatment trea ON dt.treatment_id = trea.id\n";
                query += "LEFT JOIN his_rs.his_department_tran dt1 ON dt1.previous_id = dt.id\n";
                query += "where 1 =1 \n";
                query += "and trea.Is_emergency = 1 \n";

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                query += string.Format("and dt.department_in_time <= {0}  AND (dt1.department_in_time >= {0} OR (dt1.department_in_time is null AND ( trea.is_pause IS NULL OR ( trea.is_pause = 1 AND trea.out_time >= {0}))))  ", filter.TIME_TO);
                query += string.Format("AND in_depa.DEPARTMENT_CODE IN ('{0}') ", string.Join("','", (filter.DEPARTMENT_CODE__KCCs??"").Split(',').ToList()));


                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                List<string> error = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Patient>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<HIS_TREATMENT_D> GetDeath(Mrs00555Filter filter)
        {
            List<HIS_TREATMENT_D> result = null;
            try
            {
                string query = " --danh sach benh nhan (noi tru:ket qua tu vong - kham ngoai tru: loai ra vien tu vong)\n";
                query += "SELECT\n";
                query += "TREA.TREATMENT_CODE,\n";//Mã điều trị
                query += "TREA.TDL_PATIENT_NAME,\n";//Họ và tên
                query += "DP.DEPARTMENT_NAME,\n";//Khoa kết thúc điều trị
                query += "TREA.ICD_CODE,\n";//Mã Chẩn đoán chính
                query += "TREA.ICD_NAME,\n";//Tên Chẩn đoán chính
                query += "TREA.ICD_CAUSE_CODE,\n";//Mã NNN
                query += "TREA.ICD_CAUSE_NAME,\n";//Tên NNN
                query += "TREA.DEATH_TIME,\n";//ngày tử vong
                query += "TREA.TDL_TREATMENT_TYPE_ID,\n";//diện điều trị
                query += "TREA.TREATMENT_RESULT_ID,\n";//kết quả
                query += "TREA.TREATMENT_END_TYPE_ID\n";//loại ra viện

                query += "FROM his_rs.his_treatment trea\n";
                query += "JOIN his_rs.his_department dp ON dp.id = trea.end_department_id\n";

                query += "WHERE trea.is_pause=1 and (trea.treatment_result_id =5 or trea.treatment_end_type_id=1)\n";

                //query += "And trea.tdl_treatment_type_id=3 ";

                List<string> KCCDepartmentCodes = new List<string>();
                if (filter.DEPARTMENT_CODE__OUTPATIENTs != null)
                {
                    KCCDepartmentCodes = filter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
                }
                query += string.Format("and (((trea.tdl_treatment_type_id=3 and dp.department_code not in('{0}')) and trea.treatment_result_id=5)or((trea.tdl_treatment_type_id<>3 or dp.department_code in('{0}')) and trea.treatment_end_type_id=1)) \n", string.Join("','", KCCDepartmentCodes));
                query += string.Format("and trea.OUT_TIME between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);

                if (IsNotNullOrEmpty(filter.PATIENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (IsNotNullOrEmpty(filter.INPUT_DATA_ID_STTs))
                {
                    query += string.Format("and (trea.IS_PAUSE is null and {0} or trea.is_pause=1 and {1} or trea.is_active=0 and {2})\n", filter.INPUT_DATA_ID_STTs.Contains(1) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(2) ? "1=1" : "1=0", filter.INPUT_DATA_ID_STTs.Contains(3) ? "1=1" : "1=0");
                }
               
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                List<string> error = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT_D>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        //public List<string> GetTreatmentImp(Mrs00555Filter filter)
        //{
        //    List<string> result = null;
        //    try
        //    {
        //        List<string> KCCDepartmentCodes = new List<string>();
        //        if (filter.DEPARTMENT_CODE__OUTPATIENTs != null)
        //        {
        //            KCCDepartmentCodes = filter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
        //        }
        //        string query = "\n";
        //        query += "SELECT \n";
        //        query += "trea.treatment_code \n";

        //        query += "from his_rs.his_department_tran dpt \n";
        //        query += "join his_rs.his_treatment trea on (trea.id = dpt.treatment_id and trea.in_room_id is not null) \n";
        //        query += "left join his_rs.his_department_tran dpt2 on dpt2.id = dpt.previous_id \n";
        //        query += "where 1 = 1 \n";
        //        query += "and (case when dpt.department_in_time<trea.clinical_in_time then trea.clinical_in_time else dpt.department_in_time end) between {0} and {1} \n";
        //        query += "AND dpt.department_id not IN (SELECT id FROM his_rs.his_department WHERE department_code IN ('{2}'))\n";
        //        query += "and (dpt.id = (select min(department_tran_id) from his_rs.his_patient_type_alter where treatment_id = dpt.treatment_id and treatment_type_id=3) or dpt2.id = (select min(department_tran_id) from his_rs.his_patient_type_alter where treatment_id = dpt2.treatment_id and treatment_type_id=3 and dpt2.department_id in(select id from his_rs.his_department where department_code in ('{2}'))))\n";

        //        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
        //        result = new MOS.DAO.Sql.SqlDAO().GetSql<string>(query);


        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = null;
        //    }
        //    return result;
        //}

        public List<IN_TREAT_INFO> ExamTreatIn(Mrs00555Filter filter, string query)
        {
            List<IN_TREAT_INFO> result = new List<IN_TREAT_INFO>();
            try
            {
                PropertyInfo[] p = typeof(Mrs00555Filter).GetProperties();
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
                result = new MOS.DAO.Sql.SqlDAO().GetSql<IN_TREAT_INFO>(query) ?? new List<IN_TREAT_INFO>();
                Inventec.Common.Logging.LogSystem.Info(query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<IN_TREAT_INFO>();
            }
            return result;
        }

        public List<Service> GetService()
        {
            List<Service> result = new List<Service>();
            try
            {
                string query = "select pr.id parent_id, pr.service_code parent_code, pr.service_name parent_name, sv.id service_id from his_service pr join his_service sv on pr.id = sv.parent_id";
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Service>(query);
                LogSystem.Info("SQL: " + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = new List<Service>();
            }
            return result;
        }
    }

    public class Service
    {
        public long PARENT_ID { get; set; }
        public string PARENT_CODE { get; set; }
        public string PARENT_NAME { get; set; }
        public long SERVICE_ID { get; set; }

    }

    public class CountWithPatientType
    {
        public long? CountBhyt { get; set; }
        public long? CountVp { get; set; }
    }


    public class Patient
    {
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
    }


    public class HIS_TREATMENT_D
    {
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_CAUSE_CODE { get; set; }
        public string ICD_CAUSE_NAME { get; set; }
        public long? DEATH_TIME { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
    }
    public class IN_TREAT_INFO
    {
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public long? FINISH_TIME { get; set; }
        public string IN_ROOM_NAME { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }
        public string END_ROOM_NAME { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }

    }
}
