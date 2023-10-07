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
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.DAO.Sql;
using System.Data;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00710
{
    internal class ManagerSql
    {

        internal List<Mrs00710RDOCountTreatment> GetCountTreatment(Mrs00710Filter filter)
        {
            long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
            List<Mrs00710RDOCountTreatment> result = new List<Mrs00710RDOCountTreatment>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("--tong hop ho so dieu tri theo thoi gian khoa vien phi\n");
            query += string.Format("select \n");
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and treatment_end_type_id={1} then 1 else 0 end) count_treatment_nt_cv,-- so benh nhan noi tru chuyen vien\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU,IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);
            query += string.Format("sum(case when trea.icd_code in ({0}) then 1 else 0 end) treatment_dethuong, -- so ho so dieu tri de thuong\n",
                !string.IsNullOrWhiteSpace(filter.NORMAL_BORN_ICD_CODE)?"'"+filter.NORMAL_BORN_ICD_CODE.Replace(",","','")+"'":"''");
            query += string.Format("sum(case when trea.icd_code in ({0}) then 1 else 0 end) treatment_dekho, -- so ho so dieu tri de kho\n",
                !string.IsNullOrWhiteSpace(filter.DIFFI_BORN_ICD_CODE) ? "'" + filter.DIFFI_BORN_ICD_CODE.Replace(",", "','") + "'" : "''");
            query += string.Format("sum(case when trea.icd_code in ({0}) then 1 else 0 end) treatment_mode, -- so ho so dieu tri mo de\n", 
                !string.IsNullOrWhiteSpace(filter.SURG_BORN_ICD_CODE) ? "'" + filter.SURG_BORN_ICD_CODE.Replace(",", "','") + "'" : "''");
            query += string.Format("sum(nvl(treatment_day_count,0))treatment_day_count, -- so ngay dieu tri theo bang ke\n");
            query += string.Format("sum(1) count_treatment,-- so benh nhan den kham\n");
            query += string.Format("sum(case when trea.tdl_patient_gender_id={0} then 1 else 0 end) count_treatment_female,--so benh nhan nu den kham\n",IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE);
            query += string.Format("sum(case when trea.tdl_patient_type_id = {0} then 1 else 0 end) count_treatment_bh, --so benh nhan bhyt den kham\n", patientTypeIdBhyt);
            query += string.Format("sum(case when ss.tdl_treatment_id is not null then 1 else 0 end) count_treatment_yhct,-- so benh nhan kham yhct\n");
            query += string.Format("sum(case when trea.tdl_patient_dob>in_time-150000000000 then 1 else 0 end) count_treatment_less15,--so benh nhan kham duoi 15 tuoi\n");
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} then 1 else 0 end) count_treatment_nt, --so benh nhan den kham dieu tri noi tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and trea.tdl_patient_gender_id={1} then 1 else 0 end) count_treatment_nt_female, -- so benh nhan nu den kham va dieu tri noi tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and trea.tdl_patient_type_id = {1} then 1 else 0 end) count_treatment_nt_bh, --so benh nhan bhyt den kham va dieu tri noi tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, patientTypeIdBhyt);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and ss.tdl_treatment_id is not null then 1 else 0 end) count_treatment_nt_yhct,-- so benh nhan kham yhct va dieu tri noi tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and trea.tdl_patient_dob>in_time-150000000000 then 1 else 0 end) count_treatment_nt_less15,--so benh nhan kham duoi 15 tuoi va dieu tri noi tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} then 1 else 0 end) count_treatment_ngt, --so benh nhan den kham dieu tri ngoai tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and trea.tdl_patient_gender_id={1} then 1 else 0 end) count_treatment_ngt_female, -- so benh nhan nu den kham va dieu tri ngoai tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and trea.tdl_patient_type_id = {1} then 1 else 0 end) count_treatment_ngt_bh, --so benh nhan bhyt den kham va dieu tri ngoai tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, patientTypeIdBhyt);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and ss.tdl_treatment_id is not null then 1 else 0 end) count_treatment_ngt_yhct,-- so benh nhan kham yhct va dieu tri ngoai tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
            query += string.Format("sum(case when trea.tdl_treatment_type_id={0} and trea.tdl_patient_dob>in_time-150000000000 then 1 else 0 end) count_treatment_ngt_less15--so benh nhan kham duoi 15 tuoi va dieu tri ngoai tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
            query += string.Format("from his_treatment trea\n");
            query += string.Format("left join \n");
            query += string.Format("(select \n");
            query += string.Format("tdl_treatment_id\n");
            query += string.Format("from his_sere_serv\n");
            query += string.Format("where 1=1 \n");
            query += string.Format("and is_delete=0\n");
            query += string.Format("and is_no_execute is null\n");
            query += string.Format("and tdl_service_type_id=1 \n");
            query += string.Format("and tdl_service_code in ({0}) \n",
                !string.IsNullOrWhiteSpace(filter.TRADI_SERVICE_CODE) ? "'" + filter.TRADI_SERVICE_CODE.Replace(",", "','") + "'" : "''");
            query += string.Format("group by tdl_treatment_id)\n");
            query += string.Format("ss on ss.tdl_treatment_id=trea.id\n");
            query += string.Format("where 1=1\n");
            if (filter.STATUS_TREATMENT == null)
            {
                query += string.Format("and trea.is_active = 0\n");
                query += string.Format("and trea.fee_lock_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            }
            if (filter.STATUS_TREATMENT == 0)
            {
                query += string.Format("and trea.is_pause is null\n");
                query += string.Format("and trea.in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            }
            if (filter.STATUS_TREATMENT == 1)
            {
                query += string.Format("and trea.is_pause = {0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                query += string.Format("and trea.out_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            }
            if (filter.STATUS_TREATMENT == 2)
            {
                query += string.Format("and trea.is_lock_hein ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                query += string.Format("and trea.fee_lock_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00710RDOCountTreatment>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00710");

            return result;
        }

        internal List<Mrs00710RDOCountService> GetCountService(Mrs00710Filter filter)
        {
            long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
            List<Mrs00710RDOCountService> result = new List<Mrs00710RDOCountService>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("--tong hop dich vu da xu ly theo thoi gian chi dinh\n");
            query += string.Format("select \n");
            query += string.Format("sum(case when ss.tdl_service_code in({0}) then 1 else 0 end) count_sv_dientim,--tong so dich vu dien tim chi dinh\n",
                !string.IsNullOrWhiteSpace(filter.EEG_SERVICE_CODE) ? "'" + filter.EEG_SERVICE_CODE.Replace(",", "','") + "'" : "''");
            query += string.Format("sum(case when ss.tdl_service_code in({0}) then 1 else 0 end) count_sv_diennao,--tong so dich vu dien nao chi dinh\n",
                !string.IsNullOrWhiteSpace(filter.ECG_SERVICE_CODE) ? "'" + filter.ECG_SERVICE_CODE.Replace(",", "','") + "'" : "''");
            query += string.Format("sum(case when ss.tdl_service_code in({0}) then 1 else 0 end) count_sv_ctscan,--tong so dich vu ct scan chi dinh\n",
                !string.IsNullOrWhiteSpace(filter.SCAN_SERVICE_CODE) ? "'" + filter.SCAN_SERVICE_CODE.Replace(",", "','") + "'" : "''");
            query += string.Format("ss.tdl_service_type_id,--tach theo loai dich vu\n");
            query += string.Format("sum(1) count_sv,--tong so dich vu cac loai\n");
            query += string.Format("sum(case when sr.treatment_type_id ={0} then 1 else 0 end) count_sv_nt,--tong so dich vu chi dinh dien noi tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            query += string.Format("sum(case when (sr.treatment_type_id <>{0} or sr.treatment_type_id is null) then 1 else 0 end) count_sv_ngt,--tong so dich vu chi dinh dien kham va ngoai tru\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            query += string.Format("sum(case when trea.tdl_patient_type_id ={0} then 1 else 0 end) count_sv_bh--tong so dich vu chi dinh cua benh nhan bhyt\n", patientTypeIdBhyt);
            query += string.Format("from his_service_req sr\n");
            query += string.Format("join his_sere_serv ss on ss.service_req_id=sr.id\n");
            query += string.Format("join his_treatment trea on sr.treatment_id=trea.id\n");
            query += string.Format("where sr.is_no_execute is null \n");
            query += string.Format("and sr.is_delete=0 \n");
            query += string.Format("and sr.service_req_stt_id>1 \n");
            query += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            if (filter.STATUS_TREATMENT == null)
            {
                query += string.Format("and trea.is_active = 0\n");
            }
            if (filter.STATUS_TREATMENT == 0)
            {
                query += string.Format("and trea.is_pause is null\n");
            }
            if (filter.STATUS_TREATMENT == 1)
            {
                query += string.Format("and trea.is_pause = {0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            if (filter.STATUS_TREATMENT == 2)
            {
                query += string.Format("and trea.is_lock_hein ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            query += string.Format("group by\n");
            query += string.Format("ss.tdl_service_type_id\n");

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00710RDOCountService>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00710");

            return result;
        }
    }
}
