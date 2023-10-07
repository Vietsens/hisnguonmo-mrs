using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00823
{
    public class Mrs00823Processor : AbstractProcessor
    {
        public Mrs00823Filter filter = new Mrs00823Filter();
        public List<Mrs00823RDO> ListRdo = new List<Mrs00823RDO>();
        public List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        public List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
        public List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        public List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        public List<HIS_SERVICE> ListServicepParent = new List<HIS_SERVICE>();

        public Mrs00823Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00823Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00823Filter)this.reportFilter;
            bool result = false;
            try
            {
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.OUT_TIME_FROM = filter.TIME_FROM;
                treatmentFilter.OUT_TIME_TO = filter.TIME_TO;
                if (filter.ICD_CODEs != null)
                {
                    treatmentFilter.ICD_CODEs = filter.ICD_CODEs;
                }
                if (IsNotNullOrEmpty(filter.TREATMENT_CODE))
                {
                    treatmentFilter.TREATMENT_CODE__EXACT = filter.TREATMENT_CODE;
                }


                ListServiceRetyCat = new ManagerSql().GetServiceRetyCat();
                ListTreatment = new HisTreatmentManager().Get(treatmentFilter);
                var treatmentIds = ListTreatment.Select(x => x.ID).ToList();
                var skip = 0;
                while (treatmentIds.Count - skip > 0)
                {
                    var limit = treatmentIds.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                    ssFilter.SERVICE_IDs = ListServiceRetyCat.Select(x => x.SERVICE_ID).Distinct().ToList();
                    ssFilter.TREATMENT_IDs = limit;
                    var sereServs = new HisSereServManager().GetView(ssFilter);
                    ListSereServ.AddRange(sereServs);
                }
                skip = 0;
                var serviceIds = ListSereServ.Select(x => x.SERVICE_ID).Distinct().ToList();
                while (serviceIds.Count - skip > 0)
                {
                    var limit = serviceIds.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                    var services = new HisServiceManager().Get(serviceFilter);
                    ListService.AddRange(services);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                List<Mrs00823RDO> lstRdo = new List<Mrs00823RDO>();
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    
                    foreach (var item in ListSereServ)
                    {

                        Mrs00823RDO rdo = new Mrs00823RDO();
                        rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
                        rdo.SERVICE_CODE = item.TDL_SERVICE_CODE;
                        var prService = ListServiceRetyCat.Where(x => x.SERVICE_ID == item.SERVICE_ID).FirstOrDefault();
                        if (prService != null)
                        {
                            rdo.PARENT_SERVICE_CODE = prService.CATEGORY_CODE;
                            rdo.PARENT_SERVICE_NAME = prService.CATEGORY_NAME;

                        }
                        rdo.HEIN_PRICE = item.VIR_HEIN_PRICE??0;
                        if (item.PATIENT_TYPE_NAME.ToLower().Contains("viện phí"))
                        {
                            rdo.VP_PRICE = item.PRICE;
                        }
                        else
                        {
                            rdo.VP_PRICE = 0;
                        }
                        rdo.PATIENT_PRICE_BHYT = item.PATIENT_PRICE_BHYT??0;
                        //rdo.DIC_HEIN_PRICE = ListRdo.GroupBy(x => x.PARENT_SERVICE_CODE).ToDictionary(p => p.Key, q => q.Sum(x => x.HEIN_PRICE));
                        lstRdo.Add(rdo);
                    }
                }
                if (IsNotNullOrEmpty(lstRdo))
                {
                    Mrs00823RDO rdo = new Mrs00823RDO();
                    rdo.COUNT_TREATMENT = ListTreatment.Count();
                    rdo.TREATMENT_DAY_COUNT = ListTreatment.Sum(x => x.TREATMENT_DAY_COUNT ?? 0);
                    if (rdo.COUNT_TREATMENT != 0 || rdo.COUNT_TREATMENT != null)
                    {
                        rdo.TREATMENT_DAY_COUNT_TB =Math.Round((rdo.TREATMENT_DAY_COUNT / rdo.COUNT_TREATMENT),3);
                    }
                    rdo.SERVICE_COUNT = ListSereServ.Count();
                    rdo.DIC_HEIN_PRICE = lstRdo.GroupBy(x => x.PARENT_SERVICE_CODE).ToDictionary(p => p.Key, q => q.Sum(x => x.HEIN_PRICE));
                    rdo.DIC_PATIENT_PRICE_BHYT = lstRdo.GroupBy(x => x.PARENT_SERVICE_CODE).ToDictionary(p => p.Key, q => q.Sum(x => x.PATIENT_PRICE_BHYT));
                    rdo.DIC_VP_PRICE = lstRdo.GroupBy(x => x.PARENT_SERVICE_CODE).ToDictionary(p => p.Key, q => q.Sum(x => x.VP_PRICE));
                    ListRdo.Add(rdo);
                }
                
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
