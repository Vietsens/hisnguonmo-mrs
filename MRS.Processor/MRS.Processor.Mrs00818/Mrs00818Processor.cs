using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Sql;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBaby;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00818
{
    public class Mrs00818Processor:AbstractProcessor
    {
        public MRS00818Filter filter;
        public List<Mrs00818RDO> listRdo = new List<Mrs00818RDO>();
        public List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        public List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>();
        public List<V_HIS_SERE_SERV> listSereServ = new List<V_HIS_SERE_SERV>();
        public List<HIS_BABY> listBaby = new List<HIS_BABY>();
        public Mrs00818Processor(CommonParam param, string reportTypeCode):base(param,reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(MRS00818Filter);
        }

        protected override bool GetData()
        {
            filter = (MRS00818Filter)this.reportFilter;
            bool result = false;
            try
            {
                result = true;
                listRdo = GetRdo();
               
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<Mrs00818RDO> GetRdo()
        {
            string query = @"
select 
count(case when trea.tdl_treatment_type_id=1 then 1 end) treatment_kh,
count(case when trea.tdl_treatment_type_id=3 then 1 end) treatment_noitru,
count(case when trea.tdl_treatment_type_id=2 then 1 end) treatment_ngoaitru,
count(case when trea.tdl_treatment_type_id=1 and trea.tdl_patient_gender_id=1 then 1 end) TREATMENT_KH_FEMALE,
count(case when trea.tdl_treatment_type_id=1 and trea.tdl_patient_dob>trea.in_time-150000000000 then 1 end) TREATMENT_KH_15,
count(case when trea.tdl_treatment_type_id=1 and trea.tdl_patient_type_id =1 then 1 end) TREATMENT_KH_BHYT,
count(case when trea.tdl_treatment_type_id=1 and trea.tdl_patient_type_id <>1 then 1 end) TREATMENT_KH_VP,
count(case when trea.tdl_treatment_type_id=3 and trea.tdl_patient_gender_id=1 then 1 end) TREATMENT_NOITRU_FEMALE,
count(case when trea.tdl_treatment_type_id=3 and trea.tdl_patient_dob>trea.in_time-150000000000 then 1 end) TREATMENT_NOITRU_15,
count(case when trea.tdl_treatment_type_id=3 and trea.tdl_patient_type_id =1 then 1 end) TREATMENT_NOITRU_BHYT,
count(case when trea.tdl_treatment_type_id=3 and trea.tdl_patient_type_id <>1 then 1 end) TREATMENT_NOITRU_VP,
count(case when trea.tdl_treatment_type_id=2 and trea.tdl_patient_type_id =1 then 1 end) TREATMENT_NGOAITRU_BHYT,
count(case when trea.tdl_treatment_type_id=2 and trea.tdl_patient_type_id <>1 then 1 end) TREATMENT_NGOAITRU_VP,
sum(trea.treatment_day_count) TREATMET_NOITRU_DAY_COUNT,
sum(ss.COUNT_XN) COUNT_XN,
sum(ss.COUNT_SA) COUNT_SA,
sum(ss.COUNT_XQ) COUNT_XQ,
sum(ss.COUNT_DT) COUNT_DT,
sum(ss.COUNT_DN) COUNT_DN,
sum(ss.COUNT_NS) COUNT_NS,
sum(ss.COUNT_TT) COUNT_TT,
sum(bb.count_de_kho) count_de_kho,
sum(bb.count_de) count_de,
sum(bb.count_de_mo) count_de_mo,
sum(bb.count_de_thuong) count_de_thuong,
1
from his_treatment trea
left join lateral 
(select 
sum(case when ss.tdl_service_type_id=2 then 1 end) COUNT_XN,
sum(case when ss.tdl_service_type_id=10 then 1 end) COUNT_SA,
sum(case when lower(ss.tdl_service_name) like '%xquang%' then 1 end) COUNT_XQ,
sum(case when lower(ss.tdl_service_name) like '%điện tim%' then 1 end) COUNT_DT,
sum(case when lower(ss.tdl_service_name) like '%điện não%' then 1 end) COUNT_DN,
sum(case when ss.tdl_service_type_id=9 then 1 end) COUNT_NS,
sum(case when ss.tdl_service_type_id=4 then 1 end) COUNT_TT,
1
from his_sere_serv ss 
where 1=1
and ss.tdl_treatment_id=trea.id
and ss.is_delete=0
and ss.is_no_execute is null
) ss on 1=1
left join lateral 
(select
count(1) count_de,
count(case when bb.born_type_id=3 then 1 end) count_de_kho,
count(case when bb.born_type_id=2 then 1 end) count_de_mo,
count(case when bb.born_type_id=1 then 1 end) count_de_thuong,
1
from his_baby bb
where bb.treatment_id=trea.id
) bb on 1=1
where 1=1
and trea.in_time between {0} and {1}";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            return new SqlDAO().GetSql<Mrs00818RDO>(string.Format(query,filter.TIME_FROM,filter.TIME_TO));
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM",Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO",Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
