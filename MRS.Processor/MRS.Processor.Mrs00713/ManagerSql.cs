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
using SDA.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00713
{
    internal class ManagerSql
    {

        internal List<Mrs00713RDO> GetTreatment(Mrs00713Filter filter)
        {
            List<Mrs00713RDO> result = new List<Mrs00713RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("--iss46720\n");
            query += string.Format("--thong ke thoi gian cho\n");
            query += string.Format("select\n");
            query += string.Format("trea.create_time,\n");
            query += string.Format("trea.in_time,\n");
            query += string.Format("trea.out_time,\n");
            query += string.Format("trea.fee_lock_time,\n");
            query += string.Format("trea.tdl_patient_code,\n");
            query += string.Format("trea.treatment_code,\n");
            query += string.Format("trea.tdl_patient_name,\n");
            query += string.Format("trea.tdl_hein_card_number,\n");
            query += string.Format("trea.tdl_first_exam_room_id,\n");
            query += string.Format("ptt.patient_type_name,\n");
            query += string.Format("(case when sr.has_priority>0 then 1 else null end) is_priority,\n");
            //kham
            query += string.Format("sr.exam_intruction_time,\n");
            query += string.Format("sr.exam_start_time,\n");
            query += string.Format("sr.exam_finish_time,\n");

            //xn
            query += string.Format("sr.test_intruction_time,\n");
            query += string.Format("sr.test_finish_time,\n");
            query += string.Format("sst.test_result_time,\n");
            query += string.Format("sst.test_start_time,\n");

            //cdha
            query += string.Format("sr.cdha_intruction_time,\n");
            query += string.Format("sr.cdha_start_time,\n");
            query += string.Format("sr.cdha_finish_time,\n");

            //tdcn
            query += string.Format("sr.tdcn_intruction_time,\n");
            query += string.Format("sr.tdcn_start_time,\n");
            query += string.Format("sr.tdcn_finish_time,\n");

            //cls
            query += string.Format("sr.cls_finish_time,\n");

            //exp
            query += string.Format("ex.exp_finish_time\n");
            query += string.Format("from his_treatment trea \n");
            query += string.Format("left join lateral\n");
            query += string.Format("(\n");
            query += string.Format("select\n");
            //uu tien
            query += string.Format("sum(case when sr.priority_type_id is not null then 1 else 0 end) has_priority,\n");
            //kham
            query += string.Format("min(case when sr.service_req_type_id<>1 then 99999999999999 when sr.intruction_time is null then 99999999999999 else sr.intruction_time end) exam_intruction_time,\n");
            query += string.Format("min(case when sr.service_req_type_id<>1 then 99999999999999 when sr.start_time is null then 99999999999999 else sr.start_time end) exam_start_time,\n");
            query += string.Format("max(case when sr.service_req_type_id<>1 then 0 when sr.finish_time is null then 99999999999999 else sr.finish_time end) exam_finish_time,\n");

            //xn
            query += string.Format("min(case when sr.service_req_type_id<>2 then 99999999999999 when sr.intruction_time is null then 99999999999999 else sr.intruction_time end) test_intruction_time,\n");
            //query += string.Format("min(case when sr.service_req_type_id<>2 then 99999999999999 when sr.start_time is null then 99999999999999 else sr.start_time end) test_start_time,\n");
            query += string.Format("max(case when sr.service_req_type_id<>2 then 0 when sr.finish_time is null then 99999999999999 else sr.finish_time end) test_finish_time,\n");

            //cdha
            query += string.Format("min(case when sr.service_req_type_id<>3 then 99999999999999 when sr.intruction_time is null then 99999999999999 else sr.intruction_time end) cdha_intruction_time,\n");
            query += string.Format("min(case when sr.service_req_type_id<>3 then 99999999999999 when sr.start_time is null then 99999999999999 else sr.start_time end) cdha_start_time,\n");
            query += string.Format("max(case when sr.service_req_type_id<>3 then 0 when sr.finish_time is null then 99999999999999 else sr.finish_time end) cdha_finish_time,\n");

            //tdcn
            query += string.Format("min(case when sr.service_req_type_id<>5 then 99999999999999 when sr.intruction_time is null then 99999999999999 else sr.intruction_time end) tdcn_intruction_time,\n");
            query += string.Format("min(case when sr.service_req_type_id<>5 then 99999999999999 when sr.start_time is null then 99999999999999 else sr.start_time end) tdcn_start_time,\n");
            query += string.Format("max(case when sr.service_req_type_id<>5 then 0 when sr.finish_time is null then 99999999999999 else sr.finish_time end) tdcn_finish_time,\n");

            //cls
            query += string.Format("max(case when sr.service_req_type_id not in (2,3,5,8,9) then 0 when sr.finish_time is null then 99999999999999 else sr.finish_time end) cls_finish_time,\n");
            query += string.Format("1 hasvalue\n");

            query += string.Format("from his_service_req sr\n");
            query += string.Format("where 1=1\n");
            query += string.Format("and sr.is_delete=0\n");
            query += string.Format("and sr.is_no_execute is null\n");
            query += string.Format("and sr.service_req_stt_id =3\n");
            query += string.Format("and sr.service_req_type_id in (1,2,3,5,8,9)\n");
            if (filter.REQUEST_DEPARTMENT_IDs != null)
            {
                query += string.Format("and sr.request_department_id in ({0})\n", string.Join(",",filter.REQUEST_DEPARTMENT_IDs));
            }
            query += string.Format("and trea.id=sr.treatment_id\n");
            query += string.Format(") sr\n");
            query += string.Format("on 1=1\n");
            query += string.Format("left join lateral\n");
            query += string.Format("(\n");
            query += string.Format("select\n");
            query += string.Format("max(sst.modify_time) test_result_time,\n");
            query += string.Format("min(sst.create_time) test_start_time,\n");
            query += string.Format("1 hasvalue\n");
            query += string.Format("from his_sere_serv_tein sst\n");
            query += string.Format("where 1=1 \n");
            query += string.Format("and sst.value is not null\n");
            query += string.Format("and trea.id=sst.tdl_treatment_id\n");
            if (filter.REQUEST_DEPARTMENT_IDs != null)
            {
                query += string.Format("and sst.tdl_service_req_id in (select id from his_service_req where request_department_id in ({0}))\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
            }
            query += string.Format(") sst\n");
            query += string.Format("on 1=1\n");
            query += string.Format("left join lateral\n");
            query += string.Format("(\n");
            query += string.Format("select\n");
            query += string.Format("max(ex.finish_time) exp_finish_time,\n");
            query += string.Format("1 hasvalue\n");
            query += string.Format("from his_exp_mest ex\n");
            query += string.Format("where 1=1 \n");
            query += string.Format("and ex.exp_mest_stt_id=5\n");
            query += string.Format("and ex.is_delete=0\n");
            query += string.Format("and trea.id=ex.tdl_treatment_id\n");
            if (filter.REQUEST_DEPARTMENT_IDs != null)
            {
                query += string.Format("and ex.req_department_id in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
            }
            query += string.Format(") ex\n");
            query += string.Format("on 1=1\n");
            query += string.Format("join his_patient_type ptt on trea.tdl_patient_type_id=ptt.id\n");
            query += string.Format("where 1=1\n");
            query += string.Format("and trea.in_time between {0} and {1}\n",filter.TIME_FROM,filter.TIME_TO);

           
            if (filter.IS_NGOAITRU == true)
            {
                query += string.Format("and trea.tdl_treatment_type_id=2\n");
            }
            if (filter.IS_NOITRU == true)
            {
                query += string.Format("and trea.tdl_treatment_type_id=3\n");
            }
            //diện điều trị
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            if (filter.REQUEST_DEPARTMENT_IDs != null)
            {
                query += string.Format("and (sr.hasvalue=1 or sst.hasvalue =1 or ex.hasvalue =1)\n");
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00713RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00713");

            return result;
        }
    }
}
