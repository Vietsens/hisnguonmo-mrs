/*select trea.TREATMENT_CODE,trea.TDL_PATIENT_NAME,trea.TDL_PATIENT_DOB,sum(case when ss.service_id=5881 then ss.amount else 0 end) as DV1,sum(case when ss.service_id=1743 then ss.amount else 0 end) as DV2,sum(case when ss.service_id=748 then ss.amount else 0 end) as DV3,sum(case when ss.service_id=747 then ss.amount else 0 end) as DV4,sum(case when ss.service_id=595 then ss.amount else 0 end) as DV5,sum(case when ss.service_id=3975 then ss.amount else 0 end) as DV6 from his_treatment trea 
join his_sere_serv ss on trea.id = ss.tdl_treatment_id where 
ss.TDL_service_code in (select regexp_substr((select default_value from his_config where key ='MRS.KIDNEY_DIALYSIS_SERVICE_CODES' FETCH FIRST ROWS ONLY),'[^,]+', 1, level) from dual
   connect by regexp_substr((select default_value from his_config where key ='MRS.KIDNEY_DIALYSIS_SERVICE_CODES' FETCH FIRST ROWS ONLY), '[^,]+', 1, level) is not null) group by TREA.treatment_code,trea.TDL_PATIENT_NAME,trea.TDL_PATIENT_dob;*/
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00510;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using System.Reflection;
using MOS.MANAGER.HisService;

namespace MRS.Processor.Mrs00510
{
    public class Mrs00510Processor : AbstractProcessor
    {
        private List<Mrs00510RDO> ListRdo = new List<Mrs00510RDO>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<V_HIS_SERE_SERV> listSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERVICE> listService = new List<V_HIS_SERVICE>();
        Mrs00510Filter filter = null;

        string thisReportTypeCode = "";
        public Mrs00510Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00510Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00510Filter)this.reportFilter;
            try
            {
                /*
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.IN_TIME_FROM = this.filter.TIME_FROM;
                treatmentFilter.IN_TIME_TO = this.filter.TIME_TO;
                listTreatment = new HisTreatmentManager().Get(treatmentFilter);

                if (IsNotNullOrEmpty(listTreatment))
                {
                    var treatmentIds = listTreatment.Select(o => o.ID).ToList();
                    var skip = 0;
                    HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
                    serviceFilter.SERVICE_CODEs = HisServiceCFG.getList_SERVICE_CODE__KND;
                    listService = new HisServiceManager().GetView(serviceFilter) ?? new List<V_HIS_SERVICE>();

                    while (treatmentIds.Count - skip > 0)
                    {
                        
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery()
                        {
                            TREATMENT_IDs = limit,
                            HAS_EXECUTE = true,
                            SERVICE_IDs = listService.Select(o=>o.ID).ToList(),
                            REQUEST_DEPARTMENT_ID = this.filter.DEPARTMENT_ID
                        };
                        listSereServ.AddRange(new HisSereServManager().GetView(sereServFilter) ?? new List<V_HIS_SERE_SERV>());

                    }

                }*/
                HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
                serviceFilter.SERVICE_CODEs = HisServiceCFG.getList_SERVICE_CODE__KND;
                listService = new HisServiceManager().GetView(serviceFilter) ?? new List<V_HIS_SERVICE>();

                HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery()
                {
                    INTRUCTION_TIME_FROM = this.filter.TIME_FROM,
                    INTRUCTION_TIME_TO = this.filter.TIME_TO,
                    HAS_EXECUTE = true,
                    SERVICE_IDs = listService.Select(o => o.ID).ToList(),
                    REQUEST_DEPARTMENT_ID = this.filter.DEPARTMENT_ID
                };
                listSereServ = new HisSereServManager().GetView(sereServFilter) ?? new List<V_HIS_SERE_SERV>();
                if (IsNotNullOrEmpty(listSereServ))
                {
                    var treatmentIds = listSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).ToList();
                    var skip = 0;


                    while (treatmentIds.Count - skip > 0)
                    {

                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery()
                        {
                            IDs = limit
                        };
                        listTreatment.AddRange(new HisTreatmentManager().Get(treatmentFilter) ?? new List<HIS_TREATMENT>());

                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                foreach (var item in listTreatment)
                {
                    var sereServ = listSereServ.Where(o => o.TDL_TREATMENT_ID == item.ID).ToList();
                    if (!IsNotNullOrEmpty(sereServ)) continue;
                    Mrs00510RDO rdo = new Mrs00510RDO(item,sereServ);
                    ListRdo.Add(rdo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00510RDO>();
                result = false;
            }
            return result;
        }

      
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==(this.filter.DEPARTMENT_ID??0))??new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            objectTag.AddObjectData(store, "Report", ListRdo);
            int i=0;
            foreach (var item in HisServiceCFG.getList_SERVICE_CODE__KND)
            {
                i++;
                dicSingleTag.Add(String.Format("NAME_SERVICE_{0}", i), (listService.FirstOrDefault(o => o.SERVICE_CODE == item) ?? new V_HIS_SERVICE()).SERVICE_NAME);
                dicSingleTag.Add(String.Format("CODE_SERVICE_{0}", i), (listService.FirstOrDefault(o => o.SERVICE_CODE == item) ?? new V_HIS_SERVICE()).SERVICE_CODE);
            }
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
        }

    }

}
