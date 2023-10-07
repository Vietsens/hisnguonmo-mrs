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

namespace MRS.Processor.Mrs00464
{
    public class ManagerSql
    {
        public List<Mrs00464RDO> GetTreatment(Mrs00464Filter filter, List<string> IcdCodeDttts, List<string> IcdCodeMos, List<string> IcdCodeQus, List<string> IcdCodeGls)
        {
            List<Mrs00464RDO> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach ho so dieu tri co nhap vien\n");
                query += string.Format("select\n");
                query += string.Format("(case when trea.icd_code in ('{0}') then 'DTTT'\n", string.Join("','", IcdCodeDttts));
                query += string.Format("when trea.icd_code in ('{0}') then 'MO'\n", string.Join("','", IcdCodeMos));
                query += string.Format("when trea.icd_code in ('{0}') then 'QU'\n", string.Join("','", IcdCodeQus));
                query += string.Format("when trea.icd_code in ('{0}') then 'GL' else 'KHAC' end) icd_group_code,\n", string.Join("','", IcdCodeGls));
                query += string.Format("trea.*,\n");
                query += string.Format("tr.treatment_result_name,\n");
                query += string.Format("pt.id,\n");
                query += string.Format("pt.PHONE,\n");
                query += string.Format("pt.relative_type,\n");
                // xử lý thông tin phương pháp phẫu thuật thủ thuật
                if (filter.RELATIONSHIP_METHOD == true)
                {
                    query += string.Format("ssp.pttt_method_ids,\n");
                    query += string.Format("ssp.manner,\n");
                }
                //xử lý thông tin thị lực khi khám
                if (filter.RELATIONSHIP_EXAM_EYE == true)
                {
                    query += string.Format("sr.PART_EXAM_EYESIGHT_GLASS_RIGHT,\n");
                    query += string.Format("sr.PART_EXAM_EYESIGHT_GLASS_LEFT,\n");
                    query += string.Format("sr.PART_EXAM_EYESIGHT_RIGHT,\n");
                    query += string.Format("sr.PART_EXAM_EYESIGHT_LEFT,\n");
                    query += string.Format("sr.PART_EXAM_EYE_TENSION_RIGHT,\n");
                    query += string.Format("sr.PART_EXAM_EYE_TENSION_LEFT,\n");
                }
               
                query += string.Format("pt.relative_name,\n");
                query += string.Format("pt.ethnic_name\n");

                query += string.Format("from his_patient pt\n");
                query += string.Format("join his_treatment trea on trea.patient_id=pt.id\n");
                query += string.Format("left join his_treatment_result tr on tr.id=trea.treatment_result_id\n");

                // xử lý thông tin phương pháp phẫu thuật thủ thuật
                if (filter.RELATIONSHIP_METHOD == true)
                {
                    query += string.Format(" join\n");
                    query += string.Format("(select\n");
                    query += string.Format("tdl_treatment_id,\n");
                    query += string.Format("RTRIM(XMLAGG(XMLELEMENT(E,manner,chr(10)).EXTRACT('//text()')).GetClobVal(),chr(10)) AS manner,\n");
                    query += string.Format("LISTAGG(\n");
                    query += string.Format("pttt_method_id,\n");
                    query += string.Format("','\n");
                    query += string.Format(") WITHIN GROUP(\n");
                    query += string.Format("ORDER BY\n");
                    query += string.Format("create_time\n");
                    query += string.Format(") AS pttt_method_ids\n");
                    query += string.Format("from his_sere_serv_pttt\n");
                    query += string.Format("group by \n");
                    query += string.Format("tdl_treatment_id)\n");
                    query += string.Format("ssp on ssp.tdl_treatment_id=trea.id\n");
                }
                //xử lý thông tin thị lực khi khám
                if (filter.RELATIONSHIP_EXAM_EYE == true)
                {
                    query += string.Format(" join\n");
                    query += string.Format("(select\n");
                    query += string.Format("treatment_id,\n");
                    query += string.Format("max(PART_EXAM_EYESIGHT_GLASS_RIGHT) PART_EXAM_EYESIGHT_GLASS_RIGHT,\n");
                    query += string.Format("max(PART_EXAM_EYESIGHT_GLASS_LEFT) PART_EXAM_EYESIGHT_GLASS_LEFT,\n");
                    query += string.Format("max(PART_EXAM_EYESIGHT_RIGHT) PART_EXAM_EYESIGHT_RIGHT,\n");
                    query += string.Format("max(PART_EXAM_EYESIGHT_LEFT) PART_EXAM_EYESIGHT_LEFT,\n");
                    query += string.Format("max(PART_EXAM_EYE_TENSION_RIGHT) PART_EXAM_EYE_TENSION_RIGHT,\n");
                    query += string.Format("max(PART_EXAM_EYE_TENSION_LEFT) PART_EXAM_EYE_TENSION_LEFT\n");
                    query += string.Format("from his_service_req\n");
                    query += string.Format("group by\n");
                    query += string.Format("treatment_id)\n");
                    query += string.Format("sr on sr.treatment_id=trea.id\n");
                }
                query += string.Format("where 1=1\n");
                query += string.Format("and trea.clinical_in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and trea.in_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                //query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);
                //if (filter.BRANCH_ID != null)
                //{
                //    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                //}
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00464RDO>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


        public List<INFO_EXAM_EYE> GetInfoExamEye(Mrs00464Filter filter)
        {
            List<INFO_EXAM_EYE> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach cac lan tai kham\n");
                query += string.Format("select\n");

                query += string.Format("trea.id treatment_id,\n");
                query += string.Format("sr.id service_req_id,\n");
                query += string.Format("sr.PART_EXAM_EYESIGHT_GLASS_RIGHT,\n");
                query += string.Format("sr.PART_EXAM_EYESIGHT_GLASS_LEFT,\n");
                query += string.Format("sr.PART_EXAM_EYESIGHT_RIGHT,\n");
                query += string.Format("sr.PART_EXAM_EYESIGHT_LEFT,\n");
                query += string.Format("sr.PART_EXAM_EYE_TENSION_RIGHT,\n");
                query += string.Format("sr.PART_EXAM_EYE_TENSION_LEFT\n");

                query += string.Format("from his_treatment trea \n");
                    query += string.Format("join his_service_req sr on sr.tdl_patient_id=trea.patient_id and sr.intruction_time>trea.out_time\n");
                    query += string.Format("where 1=1 \n");
                    query += string.Format("and sr.service_req_type_id=1 \n");
                    query += string.Format("and sr.is_delete=0 \n");
                    query += string.Format("and sr.is_no_execute is null \n");
                    query += string.Format(" and (sr.PART_EXAM_EYESIGHT_GLASS_RIGHT is not null or sr.PART_EXAM_EYESIGHT_GLASS_LEFT is not null\n");
                    query += string.Format(" or sr.PART_EXAM_EYESIGHT_RIGHT IS NOT NULL  OR sr.PART_EXAM_EYESIGHT_LEFT IS NOT NULL \n");
                    query += string.Format(" or sr.PART_EXAM_EYE_TENSION_RIGHT IS NOT NULL or sr.PART_EXAM_EYE_TENSION_LEFT IS NOT NULL) \n");
                query += string.Format("and trea.clinical_in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and trea.in_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
              
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<INFO_EXAM_EYE>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<DEPARTMENT_TRAN> GetDepartmentTran(Mrs00464Filter filter)
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
                query += string.Format("dpt.request_time,\n");
                query += string.Format("dpt.creator,\n");
                query += string.Format("dpt.icd_name,\n");
                query += string.Format("dpt.previous_id\n");
                query += string.Format("from his_department_tran dpt\n");
                query += string.Format("join his_treatment trea on trea.id=dpt.treatment_id\n");
                query += string.Format("where 1=1 \n");
                //query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);
                query += string.Format("and trea.clinical_in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and trea.in_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                //if (filter.BRANCH_ID != null)
                //{
                //    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                //}
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

        public List<PATIENT_TYPE_ALTER> GetPatientTypeAlter(Mrs00464Filter filter)
        {
            List<PATIENT_TYPE_ALTER> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach chuyen doi doi tuong\n");
                query += string.Format("select\n");
                query += string.Format("pta.treatment_id,\n");
                query += string.Format("pta.log_time,\n");
                query += string.Format("pta.create_time,\n");
                query += string.Format("pta.department_tran_id,\n");
                query += string.Format("pta.treatment_type_id,\n");
                query += string.Format("pta.right_route_code,\n");
                query += string.Format("pta.patient_type_id\n");
                query += string.Format("from his_patient_type_alter pta\n");
                query += string.Format("join his_treatment trea on trea.id=pta.treatment_id\n");
                query += string.Format("where 1=1 \n");
                //query += string.Format("and (case when trea.is_pause=1 then trea.out_time else {0}+1 end)>={0}\n", filter.TIME_FROM);
                query += string.Format("and trea.clinical_in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and trea.in_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                query += string.Format("and pta.treatment_type_id in ({0},{1})\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                if (filter.IS_TREAT_IN.HasValue)
                {
                    if (filter.IS_TREAT_IN.Value == true)
                    {
                        query += string.Format("and pta.treatment_type_id in ({0})\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                    else
                    {
                        query += string.Format("and pta.treatment_type_id in ({0})\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                    }
                }
                //if (filter.BRANCH_ID != null)
                //{
                //    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                //}
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
    }
}
