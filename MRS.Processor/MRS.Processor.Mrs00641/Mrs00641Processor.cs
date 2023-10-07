using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Logging;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisService;


namespace MRS.Processor.Mrs00641
{

    class Mrs00641Processor : AbstractProcessor
    {
        Mrs00641Filter castFilter = null;
        List<DepartmentCountRDO> listAmountInRdo = new List<DepartmentCountRDO>();
        List<DepartmentCountRDO> listAmountOutRdo = new List<DepartmentCountRDO>();
        Dictionary<string, decimal> DIC_OUT_SERVICE_TYPE = new Dictionary<string, decimal>();
        Dictionary<string, decimal> DIC_IN_SERVICE_TYPE_DEPARTMENT = new Dictionary<string, decimal>();
        Dictionary<string, decimal> DIC_OUT_CATEGORY = new Dictionary<string, decimal>();
        Dictionary<string, decimal> DIC_IN_CATEGORY_DEPARTMENT = new Dictionary<string, decimal>();
        decimal OUT_BHYT_AMOUNT = 0;
        Dictionary<string, decimal> DIC_IN_BHYT_AMOUNT_DEPARTMENT = new Dictionary<string, decimal>();
        List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        public Mrs00641Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00641Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00641Filter)this.reportFilter;
                HisServiceRetyCatViewFilterQuery srFilter = new HisServiceRetyCatViewFilterQuery();
                srFilter.REPORT_TYPE_CODE__EXACT = this.ReportTypeCode;
                ListServiceRetyCat = new HisServiceRetyCatManager().GetView(srFilter);

                listAmountInRdo = new ManagerSql().GetAmountInDO(this.castFilter);
                listAmountOutRdo = new ManagerSql().GetAmountOutDO(this.castFilter);
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

                if (listAmountOutRdo != null && listAmountOutRdo.Count > 0)
                {

                        DIC_OUT_SERVICE_TYPE = listAmountOutRdo.Where(q=>q.PATIENT_TYPE_ID!=HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).GroupBy(o => ServiceTypeCode(o.SERVICE_TYPE_ID)).ToDictionary(p => p.Key, p => p.Sum(s => s.AMOUNT));
                        DIC_OUT_CATEGORY = listAmountOutRdo.Where(q => q.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).GroupBy(o => CategoryCode(o.SERVICE_ID, ListServiceRetyCat)).ToDictionary(p => p.Key, p => p.Sum(s => s.AMOUNT));
                        OUT_BHYT_AMOUNT = listAmountOutRdo.Where(q => q.PATIENT_TYPE_ID== HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(s => s.AMOUNT);
                }

                if (listAmountInRdo != null && listAmountInRdo.Count > 0)
                {

                    DIC_IN_SERVICE_TYPE_DEPARTMENT = listAmountInRdo.Where(q => q.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).GroupBy(o => ServiceTypeCode(o.SERVICE_TYPE_ID,o.DEPARTMENT_ID)).ToDictionary(p => p.Key, p => p.Sum(s => s.AMOUNT));
                    DIC_IN_CATEGORY_DEPARTMENT = listAmountInRdo.Where(q => q.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).GroupBy(o => CategoryCode(o.SERVICE_ID,o.DEPARTMENT_ID, ListServiceRetyCat)).ToDictionary(p => p.Key, p => p.Sum(s => s.AMOUNT));
                    DIC_IN_BHYT_AMOUNT_DEPARTMENT = listAmountInRdo.Where(q => q.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).GroupBy(o => DepartmentCode(o.DEPARTMENT_ID)).ToDictionary(p => p.Key, p => p.Sum(s => s.AMOUNT));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private string ServiceTypeCode(long serviceTypeId)
        {
            try
            {
                return ((HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == serviceTypeId) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string CategoryCode(long serviceId,List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return ((listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string ServiceTypeCode(long serviceTypeId, long departmentId)
        {
            try
            {
                return ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                    + "_" + ((HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == serviceTypeId) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string CategoryCode(long serviceId, long departmentId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                    + "_" + ((listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string DepartmentCode(long departmentId)
        {
            string result = "";
            try
            {
                result = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

            dicSingleTag.Add("DIC_OUT_SERVICE_TYPE", DIC_OUT_SERVICE_TYPE);

            dicSingleTag.Add("DIC_IN_SERVICE_TYPE_DEPARTMENT", DIC_IN_SERVICE_TYPE_DEPARTMENT);

            dicSingleTag.Add("DIC_OUT_CATEGORY", DIC_OUT_CATEGORY);

            dicSingleTag.Add("DIC_IN_CATEGORY_DEPARTMENT", DIC_IN_CATEGORY_DEPARTMENT);

            dicSingleTag.Add("OUT_BHYT_AMOUNT", OUT_BHYT_AMOUNT);

            dicSingleTag.Add("DIC_IN_BHYT_AMOUNT_DEPARTMENT", DIC_IN_BHYT_AMOUNT_DEPARTMENT);

            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }
    }
}
