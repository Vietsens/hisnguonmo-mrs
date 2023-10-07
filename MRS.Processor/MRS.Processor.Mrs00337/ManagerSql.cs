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

namespace MRS.Processor.Mrs00337
{
    public  class ManagerSql
    {
        public List<RdoGet> GetRdo(Mrs00337Filter filter,string examDepartmentIds)
        {
            List<RdoGet> result = null;
            try
            {
                string query = "";
                query += string.Format("--du lieu ho so dieu tri co y lenh kham\n");
                query += string.Format("select icd.icd_code,\n");
                query += string.Format("icd.icd_name,\n");
                query += string.Format("nvl(icdgr.id,0) icd_group_id,\n");
                query += string.Format("icdgr.icd_group_code,\n");
                query += string.Format("icdgr.icd_group_name,\n");
                query += string.Format("count(distinct(case when sr.request_department_id in ('{0}') and trea.treatment_end_type_id = {1} and trea.treatment_result_id ={2} then sr.treatment_id else null end)) TOTAL_NANG_XINVE,\n", examDepartmentIds,IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN,IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG);
                query += string.Format("count(distinct(case when sr.request_department_id in ('{0}') and trea.death_time < trea.in_time then sr.treatment_id else null end)) TOTAL_DEAD_BEFORE,\n", examDepartmentIds);
                query += string.Format("count(distinct(case when sr.request_department_id in ('{0}') then sr.treatment_id else null end)) TOTAL_EXAM,\n", examDepartmentIds);
                query += string.Format("count(distinct(case when sr.request_department_id in ('{0}') and sr.tdl_patient_gender_id =1 then sr.treatment_id else null end)) FEMALE_EXAM,\n", examDepartmentIds);
                query += string.Format("count(distinct(case when sr.request_department_id in ('{0}') and sr.tdl_patient_dob+150000000000 > sr.intruction_time then sr.treatment_id else null end)) CHILD_UNDER_15_AGE_EXAM,\n", examDepartmentIds);
                query += string.Format("count(distinct(case when sr.request_department_id in ('{0}') and trea.treatment_end_type_id=1 then sr.treatment_id else null end)) TOTAL_DEAD_EXAM,\n", examDepartmentIds);
                query += string.Format("count(distinct(case when trea.tdl_treatment_type_id=3 then sr.treatment_id else null end)) TOTAL_SICK,\n");
                query += string.Format("count(distinct(case when trea.tdl_treatment_type_id=3 and sr.tdl_patient_gender_id =1  then sr.treatment_id else null end)) TOTAL_FEMALE_SICK,\n");
                query += string.Format("count(distinct(case when trea.tdl_treatment_type_id=3 and trea.treatment_end_type_id=1  then sr.treatment_id else null end)) TOTAL_DEAD,\n");
                query += string.Format("count(distinct(case when trea.tdl_treatment_type_id=3 and trea.treatment_end_type_id=1 and sr.tdl_patient_gender_id =1   then sr.treatment_id else null end)) TOTAL_FEMALE_DEAD,\n");
                query += string.Format("count(distinct(case when trea.tdl_treatment_type_id=3  and sr.tdl_patient_dob+150000000000 > sr.intruction_time   then sr.treatment_id else null end)) TOTAL_CHILD_UNDER_15_AGE_SICK,\n");
                query += string.Format("count(distinct(case when trea.tdl_treatment_type_id=3  and sr.tdl_patient_dob+50000000000 > sr.intruction_time   then sr.treatment_id else null end)) TOTAL_CHILD_UNDER_5_AGE_SICK,\n");
                query += string.Format("count(distinct(case when trea.tdl_treatment_type_id=3  and sr.tdl_patient_dob+150000000000 > sr.intruction_time   and trea.treatment_end_type_id=1 then sr.treatment_id else null end)) TOTAL_CHILD_UNDER_15_AGE_DEAD,\n");
                query += string.Format("count(distinct(case when trea.tdl_treatment_type_id=3  and sr.tdl_patient_dob+50000000000 > sr.intruction_time   and trea.treatment_end_type_id=1 then sr.treatment_id else null end)) TOTAL_CHILD_UNDER_5_AGE_DEAD\n");
                query += string.Format("from his_service_req sr\n");
                query += string.Format("join his_treatment trea on trea.id=sr.treatment_id\n");
                query += string.Format("join his_icd icd on icd.icd_code=sr.icd_code\n");
                query += string.Format("left join his_icd_group icdgr on icdgr.id=icd.icd_group_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and trea.is_pause=1 and sr.is_no_execute is null\n");
                if (filter.DATE_TIME_FROM > 0)
                {
                    query += string.Format("and trea.out_time>= {0}\n", filter.DATE_TIME_FROM);
                }
                if (filter.DATE_TIME_TO > 0)
                {
                    query += string.Format("and trea.out_time <= {0}\n", filter.DATE_TIME_TO);
                }
                query += string.Format("group by\n");
                query += string.Format("icd.icd_code,\n");
                query += string.Format("icd.icd_name,\n");
                query += string.Format("icdgr.id ,\n");
                query += string.Format("icdgr.icd_group_code,\n");
                query += string.Format("icdgr.icd_group_name\n");

               
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<RdoGet>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

       
    }

   

    public class RdoGet
    {
        public long ICD_GROUP_ID { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_GROUP_CODE { get; set; }
        public string ICD_GROUP_NAME { get; set; }
        public int? TOTAL_EXAM { get; set; }//tổng số ca tại khoa khám bệnh
        public int? FEMALE_EXAM { get; set; }//nữ tại khoa khám bệnh
        public int? CHILD_UNDER_15_AGE_EXAM { get; set; }//số trẻ em dưới 15 tuổi mắc bệnh
        public int? TOTAL_DEAD_EXAM { get; set; }//số ca tử vong
        public int? TOTAL_DEAD_BEFORE { get; set; } //số ca tử vong trước
        public int? TOTAL_NANG_XINVE { get; set; }//nặng xin về

        public int? TOTAL_SICK { get; set; }//tổng số bệnh nhân mắc bệnh điều trị nội trú
        public int? TOTAL_FEMALE_SICK { get; set; }//tổng số nữ mắc bệnh điều trị nội trú
        public int? TOTAL_DEAD { get; set; }//tổng số bệnh nhân tử vong điều trị nội trú
        public int? TOTAL_FEMALE_DEAD { get; set; }//tổng số nữ tử vong điều trị nội trú

        public int? TOTAL_CHILD_UNDER_15_AGE_SICK { get; set; }//trẻ em dưới 15 tuổi mắc bệnh điều trị nội trú
        public int? TOTAL_CHILD_UNDER_5_AGE_SICK { get; set; }//trẻ em dưới 5 tuổi mắc bệnh điều trị nội trú
        public int? TOTAL_CHILD_UNDER_15_AGE_DEAD { get; set; }//tổng số trẻ em dưới 15 tuổi chết điều trị nội trú
        public int? TOTAL_CHILD_UNDER_5_AGE_DEAD { get; set; }//tổng số trẻ em dưới 5 tuổi chết điều trị nội trú

    }
   
}
