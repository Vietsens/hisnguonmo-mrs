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

namespace MRS.Processor.Mrs00181
{
    public partial class ManagerSql
    {

        public List<ReqTypeUsed> GetReqTypeUsed(Mrs00181Filter filter, long serviceIdDT, long serviceIdXQ)
        {
            List<ReqTypeUsed> result = new List<ReqTypeUsed>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "--iss39332\n";
            query += " --danh sach kham va can lam sang benh nhan da dung\n";
            query += "select \n";
            query += "trea.id treatment_id,\n";
            query += "trea.in_time,\n";
            //if (filter.IS_SPLIT_HASMEDI == true)
            {
                query += "min(case when sr.service_req_type_id = 1 then sr.start_time else null end) start_exam_time,\n";
                query += "max(case when sr.service_req_type_id = 1 and sr.execute_room_id=nvl(trea.in_room_id,trea.end_room_id) then sr.finish_time else null end) finish_exam_time,\n";
                query += "max(ex.finish_time) finish_time,\n";
            }
            query += "nvl(trea.clinical_in_time,trea.out_time) out_time,\n";
            query += "listagg(sr.service_req_type_id,'_') within group(order by sr.service_req_type_id) list_type\n";
            query += "from his_treatment trea \n";
            query += "join his_service_req sr on trea.id=sr.treatment_id\n";
            //if (filter.IS_SPLIT_HASMEDI == true)
            {
                query += string.Format("left join lateral\n");
                query += string.Format("(\n");
                query += string.Format("select\n");
                query += string.Format("ex.tdl_treatment_id,\n");
                query += string.Format("max(ex.finish_time) finish_time\n");
                query += string.Format("from his_exp_mest ex \n");
                query += string.Format("where 1=1\n");
                query += string.Format("and ex.tdl_treatment_id = trea.id\n");
                query += string.Format("and ex.exp_mest_type_id ={0} and ex.exp_mest_stt_id ={1}\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE);
                query += string.Format("group by ex.tdl_treatment_id\n");

                query += string.Format(") ex\n");
                query += string.Format("on ex.tdl_treatment_id = trea.id\n");
            }
            if (filter.FILTER_01 == true)
            {
                query += string.Format("left join\n");
                query += string.Format("(\n");
                query += string.Format("select ss.tdl_treatment_id\n");
                query += string.Format("from his_sere_serv ss \n");
                query += string.Format("join his_service sv on sv.id=ss.service_id\n");
                query += string.Format("join his_treatment trea on trea.id=ss.tdl_treatment_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and (sv.service_type_id ={0} or sv.service_type_id ={1} and sv.id <>{2} or sv.service_type_id ={3} and sv.parent_id <>{4})\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN, serviceIdDT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA, serviceIdXQ);
                query += string.Format("and trea.in_date between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("and trea.end_department_id = {0}\n", filter.DEPARTMENT_ID);
                }

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and trea.end_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += string.Format("and trea.is_pause =1\n");
                query += string.Format("and nvl(trea.clinical_in_time,trea.out_time)<trea.in_date+1000000\n");
                query += string.Format(") tmid on tmid.tdl_treatment_id=trea.id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and tmid.tdl_treatment_id is null\n");
            }
            else
            {
                query += "where 1=1\n";
            }
            query += "and sr.service_req_type_id in (1,2,3,5,8,9)\n";
            query += "and sr.is_no_execute is null\n";
            query += "and sr.is_delete =0\n";

            query += string.Format("and trea.in_date between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);

            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("and trea.end_department_id = {0}\n", filter.DEPARTMENT_ID);
            }

            if (filter.DEPARTMENT_IDs != null)
            {
                query += string.Format("and trea.end_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
            }
            if (filter.ROOM_ID != null)
            {
                query += string.Format("and trea.end_room_id = {0}\n", filter.ROOM_ID);
            }

            if (filter.ROOM_IDs != null)
            {
                query += string.Format("and  in ({0})\n", string.Join(",", filter.ROOM_IDs));
            }

            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("and trea.end_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
            }
            //if (filter.EXAM_ROOM_IDs != null)
            //{
            //    query += string.Format("and sr.EXECUTE_ROOM_ID in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
            //}
            query += "and trea.is_pause =1\n";
            if (filter.IS_NGOAI_TRU == true && filter.FILTER_01 != true)
            {
                query += string.Format("and trea.tdl_treatment_type_id in ({0},{1})", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
            }
            query += "and nvl(trea.clinical_in_time,trea.out_time)<trea.in_date+1000000\n";

            query += "group by \n";
            query += "trea.id,\n";
            query += "trea.in_time,\n";
            query += "nvl(trea.clinical_in_time,trea.out_time)\n";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<ReqTypeUsed>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00181");
            return result;
        }


        internal List<ReqTypeUsed> GetReqTypeVsUsed(Mrs00181Filter filter)
        {
            List<ReqTypeUsed> result = new List<ReqTypeUsed>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += " --danh sach XN vi sinh benh nhan da dung\n";
            query += "select \n";
            query += "trea.id treatment_id,\n";
            query += "trea.in_time,\n";
            //if (filter.IS_SPLIT_HASMEDI == true)
            //{
            //    query += "min(case when sr.service_req_type_id = 1 then sr.start_time else null end) start_exam_time,\n";
            //    query += "max(case when sr.service_req_type_id = 1 and sr.execute_room_id=nvl(trea.in_room_id,trea.end_room_id) then sr.finish_time else null end) finish_exam_time,\n";
            //    query += "max(ex.finish_time) finish_time,\n";
            //}
            //query += "nvl(trea.clinical_in_time,trea.out_time) out_time,\n";
            query += "listagg(sr.service_req_type_id,'_') within group(order by sr.service_req_type_id) list_type\n";
            query += "from his_treatment trea \n";
            query += "join his_service_req sr on trea.id=sr.treatment_id\n";
            query += "join his_sere_serv ss on ss.service_req_id=sr.id \n";
            query += "join his_service sv on sv.id=ss.service_id\n";
            query += "join his_service pr on pr.id=sv.parent_id\n";
            //if (filter.IS_SPLIT_HASMEDI == true)
            {
                //query += string.Format("left join lateral\n");
                //query += string.Format("(\n");
                //query += string.Format("select\n");
                //query += string.Format("ex.tdl_treatment_id,\n");
                //query += string.Format("max(ex.finish_time) finish_time\n");
                //query += string.Format("from his_exp_mest ex \n");
                //query += string.Format("where 1=1\n");
                //query += string.Format("and ex.tdl_treatment_id = trea.id\n");
                //query += string.Format("and ex.exp_mest_type_id ={0} and ex.exp_mest_stt_id ={1}\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE);
                //query += string.Format("group by ex.tdl_treatment_id\n");

                //query += string.Format(") ex\n");
                //query += string.Format("on ex.tdl_treatment_id = trea.id\n");
            }
            //if (filter.FILTER_01 == true)
            //{
            //    query += string.Format("left join\n");
            //    query += string.Format("(\n");
            //    query += string.Format("select ss.tdl_treatment_id\n");
            //    query += string.Format("from his_sere_serv ss \n");
            //    query += string.Format("join his_service sv on sv.id=ss.service_id\n");
            //    query += string.Format("join his_treatment trea on trea.id=ss.tdl_treatment_id\n");
            //    query += string.Format("where 1=1\n");
            //    query += string.Format("and (sv.service_type_id ={0} or sv.service_type_id ={1} and sv.id <>{2} or sv.service_type_id ={3} and sv.parent_id <>{4})\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN, serviceIdDT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA, serviceIdXQ);
            //    query += string.Format("and trea.in_date between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            //    if (filter.DEPARTMENT_ID != null)
            //    {
            //        query += string.Format("and trea.end_department_id = {0}\n", filter.DEPARTMENT_ID);
            //    }

            //    if (filter.DEPARTMENT_IDs != null)
            //    {
            //        query += string.Format("and trea.end_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
            //    }
            //    query += string.Format("and trea.is_pause =1\n");
            //    query += string.Format("and nvl(trea.clinical_in_time,trea.out_time)<trea.in_date+1000000\n");
            //    query += string.Format(") tmid on tmid.tdl_treatment_id=trea.id\n");
            //    query += string.Format("where 1=1\n");
            //    query += string.Format("and tmid.tdl_treatment_id is null\n");
            //}
            //else
            {
                query += "where 1=1\n";
            }
            query += "and sr.service_req_type_id in (2)\n";
            if (filter.PARENT_SV_CODE__VSs != null && filter.PARENT_SV_CODE__VSs.Length>0)
            {
                query += string.Format("and pr.service_code in ('{0}')\n", filter.PARENT_SV_CODE__VSs.Replace(",","','"));
            }
            query += "and sr.is_no_execute is null\n";
            query += "and sr.is_delete =0\n";

            query += string.Format("and trea.in_date between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);

            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("and trea.end_department_id = {0}\n", filter.DEPARTMENT_ID);
            }

            if (filter.DEPARTMENT_IDs != null)
            {
                query += string.Format("and trea.end_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
            }
            if (filter.ROOM_ID != null)
            {
                query += string.Format("and trea.end_room_id = {0}\n", filter.ROOM_ID);
            }

            if (filter.ROOM_IDs != null)
            {
                query += string.Format("and  in ({0})\n", string.Join(",", filter.ROOM_IDs));
            }

            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("and trea.end_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
            }
            //if (filter.EXAM_ROOM_IDs != null)
            //{
            //    query += string.Format("and sr.EXECUTE_ROOM_ID in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
            //}
            query += "and trea.is_pause =1\n";
            if (filter.IS_NGOAI_TRU == true)
            {
                query += string.Format("and trea.tdl_treatment_type_id in ({0},{1})", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
            }
            query += "and nvl(trea.clinical_in_time,trea.out_time)<trea.in_date+1000000\n";

            query += "group by \n";
            query += "trea.id,\n";
            query += "trea.in_time,\n";
            query += "nvl(trea.clinical_in_time,trea.out_time)\n";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<ReqTypeUsed>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00181");
            return result;
        }
    }
}
