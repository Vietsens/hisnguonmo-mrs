using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MRS.MANAGER.Config;
using System.Reflection;
using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get;
using HTC.MANAGER.Manager;
using HTC.MANAGER.Core.PeriodBO.HtcPeriod.Get;

namespace MRS.Processor.Mrs00491
{
    public class Mrs00491Processor : AbstractProcessor
    {
        List<Mrs00491RDO> ListRdo = new List<Mrs00491RDO>();
        List<HTC_REVENUE> ListHtcRevenue = new List<HTC_REVENUE>();
        List<HTC_PERIOD> ListHtcPeriod = new List<HTC_PERIOD>();
        Dictionary<long, Mrs00491RDO> dicRdo = new Dictionary<long, Mrs00491RDO>();
        Mrs00491Filter castFilter = null;
        List<string> listServiceTypeCode = null;

        public Mrs00491Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00491Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00491Filter)this.reportFilter);

                HtcPeriodFilterQuery HtcPeriodfilter = new HtcPeriodFilterQuery();
                ListHtcPeriod = new HtcPeriodManager(paramGet).Get<List<HTC_PERIOD>>(HtcPeriodfilter);
                HtcRevenueFilterQuery HtcRevenuefilter = new HtcRevenueFilterQuery();
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<HtcRevenueFilterQuery>();
                foreach (var item in pi)
                {
                    item.SetValue(HtcRevenuefilter, (item.GetValue(castFilter)));
                }
                if (castFilter.DEPARTMENT_ID != null)
                {
                    HtcRevenuefilter.EXECUTE_DEPARTMENT_CODE_EXACT = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                }
                ListHtcRevenue = new HtcRevenueManager(paramGet).Get<List<HTC_REVENUE>>(HtcRevenuefilter);
                if (IsNotNullOrEmpty(ListHtcRevenue))
                {
                     listServiceTypeCode = ListHtcRevenue.Where(n => n.SERVICE_TYPE_CODE != null).OrderBy(m => m.SERVICE_TYPE_CODE).Select(o => o.SERVICE_TYPE_CODE).Distinct().ToList();
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu HTC_REVENUE, Mrs00491");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListHtcRevenue))
                {
                    var groupByPatientCode = ListHtcRevenue.Where(n => n.PATIENT_CODE != null).GroupBy(o => o.PATIENT_CODE).ToList();
                    foreach (var item in groupByPatientCode)
                    {
                        List<HTC_REVENUE> listSub = item.ToList<HTC_REVENUE>();
                        Mrs00491RDO rdo = new Mrs00491RDO(listSub.First());
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.IN_TIME ?? 0);
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.OUT_TIME ?? 0);
                        for (int i =0;i< listServiceTypeCode.Count;i++)
                        {
                            if (i < 40)
                            {
                                PropertyInfo pi = typeof(Mrs00491RDO).GetProperty(string.Format("AMOUNT_{0}", i + 1));
                                pi.SetValue(rdo, (decimal)listSub.Where(o => o.SERVICE_TYPE_CODE == listServiceTypeCode[i]).Sum(s => s.VIR_TOTAL_PRICE??0));
                            }
                        }
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {

                dicSingleTag.Add("DEPARTMENT_NAME", string.Join(" - ", ListHtcRevenue.Where(n => n.EXECUTE_DEPARTMENT_NAME != null).OrderBy(m => m.EXECUTE_DEPARTMENT_NAME).Select(o => o.EXECUTE_DEPARTMENT_NAME).Distinct().ToList()));
                if (ListHtcPeriod != null)
                {
                    dicSingleTag.Add("PERIOD_NAME", string.Join(" - ", ListHtcPeriod.Where(n => ListHtcRevenue.Exists(q => q.PERIOD_ID == n.ID)).OrderBy(m => m.PERIOD_NAME).Select(o => o.PERIOD_NAME).Distinct().ToList()));
                    dicSingleTag.Add("PERIOD_CODE", string.Join(" - ", ListHtcPeriod.Where(n => ListHtcRevenue.Exists(q => q.PERIOD_ID == n.ID)).OrderBy(m => m.PERIOD_CODE).Select(o => o.PERIOD_CODE).Distinct().ToList()));
                }

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.REVENUE_TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.REVENUE_TIME_TO ?? 0));

                for (int i = 0; i < listServiceTypeCode.Count; i++)
                {
                    dicSingleTag.Add(string.Format("SERVICE_TYPE_NAME_{0}", i + 1), (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o=>o.SERVICE_TYPE_CODE==listServiceTypeCode[i])??new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME);
                }
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
