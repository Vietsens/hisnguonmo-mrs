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
using MOS.DAO.Sql;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00559
{
    public partial class ManagerSql : BusinessBase
    {
        public const long CHIDINH = 1;
        public const long BATDAU = 2;
        public const long KETTHUC = 3;
        public List<CountWithPatientType> CountExamWait(Mrs00559Filter filter)
        {
            List<CountWithPatientType> result = null;
            try
            {
                string query = "";
                query += "SELECT ";
                query += "sr.execute_room_id as Id, ";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, ";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp ";

                query += "from his_service_req sr join his_treatment trea on sr.treatment_id = trea.id join v_his_room exro on sr.execute_room_id = exro.id join his_room rero on sr.request_room_id = rero.id ";

                query += "WHERE sr.is_no_execute is null and sr.is_delete =0 and sr.service_req_type_id =1 and rero.room_type_id = 3 and sr.service_req_stt_id =1 ";

                if (filter.INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME >= {0} ", filter.INTRUCTION_TIME_FROM);
                }
                if (filter.INTRUCTION_TIME_TO != null)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME < {0} ", filter.INTRUCTION_TIME_TO);
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                query += "group by ";
                query += "sr.execute_room_id ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public List<CountWithPatientType> CountExamFinish(Mrs00559Filter filter)
        {
            List<CountWithPatientType> result = null;
            try
            {
                string query = "";
                query += "SELECT ";
                query += "ro.Id, ";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, ";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp ";

                query += "from his_treatment trea join his_service_req sr on (sr.execute_room_id=trea.end_room_id and sr.treatment_id=trea.id and sr.service_req_stt_id=3 and sr.is_no_execute is null and sr.service_req_type_id =1 and sr.is_delete =0) join v_his_room ro on trea.end_room_id = ro.id ";

                query += "WHERE trea.is_pause=1 and trea.treatment_end_type_id in (1,2,3,4,5,6,7,8) ";
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND SR.FINISH_TIME >= {0} ", filter.INTRUCTION_TIME_FROM);
                }
                if (filter.INTRUCTION_TIME_TO != null)
                {
                    query += string.Format("AND SR.FINISH_TIME < {0} ", filter.INTRUCTION_TIME_TO);
                }

                query += "group by ";
                query += "ro.ID ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


        public List<ServiceReqDO> GetServiceReq(HisServiceReqFilterQuery serviceReqFilter,Mrs00559Filter filter)
        {
            List<ServiceReqDO> result = new List<ServiceReqDO>();
            string query = "";
            query += "SELECT ";
            query += "SR.ID, \n";
            query += "SR.REQUEST_ROOM_ID, \n";
            query += "SR.EXECUTE_ROOM_ID, \n";
            query += "SR.PREVIOUS_SERVICE_REQ_ID, \n";
            query += "SR.FINISH_TIME, \n";
            query += "SR.TREATMENT_ID, \n";
            query += "SR.IS_EMERGENCY, \n";
            query += "SR.TDL_PATIENT_DOB, \n";
            query += "SR.TDL_PATIENT_GENDER_ID, \n";
            query += "TREA.CLINICAL_IN_TIME, \n";
            query += "TREA.TDL_TREATMENT_TYPE_ID, \n";
            query += "TREA.TDL_PATIENT_TYPE_ID, \n";
            query += "TREA.IS_PAUSE, \n";
            query += "TREA.TREATMENT_END_TYPE_ID, \n";
            query += "TREA.END_ROOM_ID, \n";
            query += "TREA.OUT_TIME, \n";
            query += "TREA.HOSPITALIZE_DEPARTMENT_ID, \n";
            query += "TREA.BRANCH_ID, \n";

            query += "PA.ETHNIC_NAME \n";
            query += "FROM HIS_RS.HIS_SERVICE_REQ SR \n";

            query += "JOIN HIS_RS.HIS_PATIENT PA ON SR.TDL_PATIENT_ID = PA.ID \n";

            query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TREA.ID = SR.TREATMENT_ID \n";
            query += "AND SR.IS_DELETE =0 \n";
            query += "AND SR.IS_NO_EXECUTE IS NULL \n";
            query += string.Format("AND SR.SERVICE_REQ_TYPE_ID = {0} ", serviceReqFilter.SERVICE_REQ_TYPE_ID);
            if (filter.INPUT_DATA_ID_TIME_TYPE != null)
            {
                if (filter.INPUT_DATA_ID_TIME_TYPE == CHIDINH)
                {
                    if (serviceReqFilter.INTRUCTION_TIME_TO != null)
                    {
                        query += string.Format("AND SR.INTRUCTION_TIME < {0} ", serviceReqFilter.INTRUCTION_TIME_TO);
                    }
                    if (serviceReqFilter.INTRUCTION_TIME_FROM != null)
                    {
                        query += string.Format("AND SR.INTRUCTION_TIME >={0} ", serviceReqFilter.INTRUCTION_TIME_FROM);
                    }
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == BATDAU)
                {
                    if (serviceReqFilter.INTRUCTION_TIME_TO != null)
                    {
                        query += string.Format("AND SR.START_TIME < {0} ", serviceReqFilter.INTRUCTION_TIME_TO);
                    }
                    if (serviceReqFilter.INTRUCTION_TIME_FROM != null)
                    {
                        query += string.Format("AND SR.START_TIME >={0} ", serviceReqFilter.INTRUCTION_TIME_FROM);
                    }
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == KETTHUC)
                {
                    if (serviceReqFilter.INTRUCTION_TIME_TO != null)
                    {
                        query += string.Format("AND SR.FINISH_TIME < {0} ", serviceReqFilter.INTRUCTION_TIME_TO);
                    }
                    if (serviceReqFilter.INTRUCTION_TIME_FROM != null)
                    {
                        query += string.Format("AND SR.FINISH_TIME >={0} ", serviceReqFilter.INTRUCTION_TIME_FROM);
                    }
                }
            }
            else
            {
                if (serviceReqFilter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND SR.CREATE_TIME < {0} ", serviceReqFilter.CREATE_TIME_FROM);
                }
                if (serviceReqFilter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND SR.CREATE_TIME >={0} ", serviceReqFilter.CREATE_TIME_TO);
                }
                if (serviceReqFilter.INTRUCTION_TIME_TO != null)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME < {0} ", serviceReqFilter.INTRUCTION_TIME_TO);
                }
                if (serviceReqFilter.INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME >={0} ", serviceReqFilter.INTRUCTION_TIME_FROM);
                }
            }
            query += "ORDER BY SR.ID ASC ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<ServiceReqDO>(query);

            return result;
        }
    }

    public class CountWithPatientType
    {
        public long Id { get; set; }
        public long? CountBhyt { get; set; }
        public long? CountVp { get; set; }
    }

    public class ServiceReqDO
    {
        public long ID { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public long EXECUTE_ROOM_ID { get; set; }
        public long? PREVIOUS_SERVICE_REQ_ID { get; set; }
        public long? FINISH_TIME { get; set; }
        public long TREATMENT_ID { get; set; }
        public short? IS_EMERGENCY { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public short? IS_PAUSE { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public long? END_ROOM_ID { get; set; }
        public long? OUT_TIME { get; set; }
        public string ETHNIC_NAME { set; get; }
        public int? HOSPITALIZE_DEPARTMENT_ID { get; set; }
        public long? BRANCH_ID { get; set; }
    }
}
