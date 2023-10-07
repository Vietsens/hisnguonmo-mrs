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

namespace MRS.Processor.Mrs00678
{
    public partial class ManagerSql : BusinessBase
    {

        public List<HIS_SERE_SERV> GetSereServ(Mrs00678Filter filter)
        {
            List<HIS_SERE_SERV> result = null;
            try
            {

                string query = "--du lieu dich vu\n";
                query += "SELECT \n";
                query += "distinct ss.* \n";

                query += string.Format("from his_rs.his_sere_serv ss\n");

                query += "join his_rs.HIS_SERVICE_REQ SR on sr.treatment_id=ss.tdl_treatment_id and sr.execute_room_id=ss.tdl_request_room_id\n";
                query += "join his_rs.his_treatment trea on trea.id=sr.treatment_id\n";

                query += string.Format("where ss.is_no_execute is null and ss.is_delete =0\n");
                FilterServiceReq(filter, ref query);

                FilterTreatment(filter, ref query);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<SALE_MEDICINE> GetSaleExpMestMedicine(Mrs00678Filter filter)
        {
            List<SALE_MEDICINE> result = null;
            try
            {

                string query = "--du lieu mua thuoc\n";
                query += "SELECT \n";
                query += "distinct srex.treatment_id, \n";
                query += "srex.request_room_id, \n";
                query += "exmm.medicine_type_name, \n";
                query += "exmm.amount \n";

                query += string.Format("from his_rs.v_his_exp_mest_medicine exmm\n");

                query += string.Format("join his_rs.his_exp_mest ex on ex.id = exmm.exp_mest_id\n");

                query += string.Format("join his_rs.HIS_SERVICE_REQ srex on srex.id = ex.prescription_id\n");

                query += "join his_rs.HIS_SERVICE_REQ SR on sr.execute_room_id=srex.request_room_id and sr.treatment_id=srex.treatment_id\n";
                query += "join his_rs.his_treatment trea on trea.id=sr.treatment_id\n";

                query += string.Format("where exmm.exp_mest_type_id=8 and exmm.is_delete =0 and exmm.is_export=1\n");
                FilterServiceReq(filter, ref query);

                FilterTreatment(filter, ref query);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<SALE_MEDICINE>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<D_HIS_SERVICE_REQ> Get(Mrs00678Filter filter)
        {
            List<D_HIS_SERVICE_REQ> result = null;
            try
            {

                string query = "--danh sách y lệnh khám\n";
                query += "SELECT \n";
                query += "nvl(ss.service_id||'',sr.tdl_service_ids) tdl_service_ids,\n";
                query += "sr.* \n";

                query += "from his_rs.HIS_SERVICE_REQ SR\n";
                query += "join his_rs.his_treatment trea on trea.id=sr.treatment_id\n";
                query += "left join his_rs.his_sere_serv ss on sr.id=ss.service_req_id and sr.tdl_service_ids is null\n";
                query += "where 1=1\n";
                FilterServiceReq(filter, ref query);

                FilterTreatment(filter, ref query);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<D_HIS_SERVICE_REQ>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<V_HIS_TREATMENT_4> GetTreatment(Mrs00678Filter filter)
        {
            List<V_HIS_TREATMENT_4> result = null;
            try
            {

                string query = "--danh sách hồ sơ điều trị\n";
                query += "SELECT \n";
                query += "distinct trea.* \n";

                query += "from his_rs.HIS_SERVICE_REQ SR\n";
                query += "join his_rs.v_his_treatment_4 trea on trea.id=sr.treatment_id\n";
                query += "where 1=1\n";
                FilterServiceReq(filter, ref query);

                FilterTreatment(filter, ref query);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TREATMENT_4>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private void FilterTreatment(Mrs00678Filter filter, ref string query)
        {
            //điều kiện lọc của hồ sơ điều trị
            if (filter.TDL_PATIENT_TYPE_IDs != null)
            {
                query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
            {
                query += string.Format("AND trea.IN_TIME between {0} and {1}\n", filter.FINISH_TIME_FROM, filter.FINISH_TIME_TO);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
            {
                query += string.Format("AND trea.IS_PAUSE=1 AND trea.OUT_TIME between {0} and {1}\n", filter.FINISH_TIME_FROM, filter.FINISH_TIME_TO);
            }
            //điều kiện lọc là người già
            if (filter.IS_ELDER == true)
            {
                query += string.Format("and trea.tdl_patient_dob < trea.in_time - 600000000000 \n");
            }
            //điều kiện lọc là nội trú
            if (filter.IS_TREATIN == true)
            {
                query += string.Format("and trea.tdl_treatment_type_id in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            }

            if (filter.BRANCH_ID != null)
            {
                query += string.Format("AND trea.BRANCH_ID = {0} \n", filter.BRANCH_ID);
            }
        }

        private void FilterServiceReq(Mrs00678Filter filter, ref string query)
        {
            // điều kiện lọc của y lệnh khám

            query += "and sr.service_req_type_id=1\n";
            //query += "and sr.tdl_service_ids is not null\n";
            query += "and sr.is_delete = 0\n";
            query += "and sr.is_no_execute is null \n";
            if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
            {
                query += string.Format("AND sr.INTRUCTION_TIME between {0} and {1}\n", filter.FINISH_TIME_FROM, filter.FINISH_TIME_TO);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
            {
                query += string.Format("AND sr.START_TIME between {0} and {1}\n", filter.FINISH_TIME_FROM, filter.FINISH_TIME_TO);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
            {
                query += string.Format("AND sr.FINISH_TIME between {0} and {1}\n", filter.FINISH_TIME_FROM, filter.FINISH_TIME_TO);
            }
            if (filter.INPUT_DATA_ID_TIME_TYPE == null || filter.INPUT_DATA_ID_TIME_TYPE == 0)
            {
                if (filter.FINISH_TIME_FROM != null)
                {
                    query += string.Format("AND SR.FINISH_TIME >= {0} \n", filter.FINISH_TIME_FROM);
                }
                if (filter.FINISH_TIME_TO != null)
                {
                    query += string.Format("AND SR.FINISH_TIME < {0} \n", filter.FINISH_TIME_TO);

                }
            }
            //if (treatmentIds != null && treatmentIds.Count > 0)
            //{
            //    query += string.Format("AND SR.TREATMENT_ID IN ({0}) \n", string.Join(",", treatmentIds));

            //}
            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("AND SR.EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));

            }
            if (filter.EXAM_ROOM_ID != null)
            {
                query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXAM_ROOM_ID);

            }

            if (filter.REQUEST_ROOM_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID IN ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));

            }
            if (filter.REQUEST_ROOM_ID != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID = {0} \n", filter.REQUEST_ROOM_ID);
            }

           
        }
    }

    public class SALE_MEDICINE
    {
        public long TREATMENT_ID { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public decimal AMOUNT { get; set; }
    }
}
