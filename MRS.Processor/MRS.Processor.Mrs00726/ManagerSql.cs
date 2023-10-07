using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00726
{
    internal class ManagerSql
    {

        internal List<Mrs00726RDO> GetRdo(Mrs00726Filter filter)
        {

            List<Mrs00726RDO> list = new List<Mrs00726RDO>();
            CommonParam val = new CommonParam();
            try
            {
                string text = "--danh sach y lenh\n";
                text += "select \n";
                text += "dp.department_code,\n";
                text += "dp.department_name,\n";
                text += "ro.room_code,\n";
                text += "ro.room_name,\n";
                text += "count(sr.ID) as SO_LUOT_KHAM,\n";
                text += "count(case when sr.service_req_stt_id = 3 then trea.id else null end) as SO_LUOT_DA_KHAM,\n";
                text += "count(case when sr.is_main_exam = 1 and trea.tdl_patient_type_id = 1 then sr.ID else null end) as KHAM_CHINH_BHYT,\n";
                text += "count(case when (sr.is_main_exam <> 1 or sr.is_main_exam is null) and trea.tdl_patient_type_id = 1 then sr.ID else null end) as KHAM_PHU_BHYT,\n";
                text += "count(case when sr.is_main_exam = 1 and trea.tdl_patient_type_id = 124 then sr.ID else null end) as KHAM_CHINH_DV,\n";
                text += "count(case when (sr.is_main_exam <> 1 or sr.is_main_exam is null) and trea.tdl_patient_type_id = 124 then sr.ID else null end) as KHAM_PHU_DV,\n";
                text += "count(case when trea.clinical_in_time is not null then trea.id else null end) as NHAP_VIEN,\n";
                text += "count(case when trea.tdl_patient_province_code = 01 then trea.id else null end) as BN_DIA_PHUONG,\n";
                text += "count(case when trea.tdl_patient_province_code <> 01 then trea.id else null end) as BN_CAC_TINH,\n";
                text += "count(case when trea.tdl_patient_type_id = 1 then trea.id else null end) as DANGKY_BHYT,\n";
                text += "count(case when trea.tdl_patient_type_id = 124 then trea.id else null end) as DANGKY_DV\n";
                text += "from his_service_req sr\n";
                text += "join his_treatment trea on sr.treatment_id = trea.id\n";
                text += "join v_his_room ro on sr.request_room_id = ro.id\n";
                text += "join his_department dp on sr.request_department_id = dp.id\n";
                text += "where 1=1\n";
                text += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                text += string.Format("and sr.service_req_type_id = 1\n");
                text += "and ro.is_exam = 1\n";
                text += "group by dp.department_code, dp.department_name, ro.room_code, ro.room_name";


                LogSystem.Info("SQL: " + text);
                list = new SqlDAO().GetSql<Mrs00726RDO>(text);
                LogSystem.Info("Result: " + list);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                list = null;
            }
            return list;
        }
    }
}
