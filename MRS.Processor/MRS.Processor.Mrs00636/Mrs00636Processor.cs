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


namespace MRS.Processor.Mrs00636
{

    class Mrs00636Processor : AbstractProcessor
    {
        Mrs00636Filter castFilter = null;
        List<DepartmentCountRDO> listCountExamRdo = new List<DepartmentCountRDO>();
        List<DepartmentCountRDO> listCountTreatRdo = new List<DepartmentCountRDO>();
        List<DepartmentCountRDO> listCountCategoryRdo = new List<DepartmentCountRDO>();
        List<DepartmentCountRDO> listToTalPriceRdo = new List<DepartmentCountRDO>();
        //List<Mrs00636RDO> listRdo = new List<Mrs00636RDO>();
        decimal? COUNT_EXAM = null;
        decimal? COUNT_TREATIN = null;
        decimal? DATE_TREATIN = null;
        decimal? COUNT_TREATOUT = null;
        Dictionary<string, decimal> DIC_AMOUNT_CATEGORY = new Dictionary<string, decimal>();

        Dictionary<string, decimal> DIC_COUNT_EXAM = new Dictionary<string, decimal>();
        Dictionary<string, decimal> DIC_COUNT_TREATIN = new Dictionary<string, decimal>();
        Dictionary<string, decimal> DIC_DATE_TREATIN = new Dictionary<string, decimal>();
        Dictionary<string, decimal> DIC_COUNT_TREATOUT = new Dictionary<string, decimal>();
        Dictionary<string, decimal> DIC_AMOUNT_CATEGORY_DEPARTMENT = new Dictionary<string, decimal>();

        decimal? TOTAL_PRICE = null;
        Dictionary<string, decimal> DIC_TOTAL_PRICE = new Dictionary<string, decimal>();

        public Mrs00636Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00636Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00636Filter)this.reportFilter;

                listCountExamRdo = new ManagerSql().GetCountExamDO(this.castFilter);
                listCountTreatRdo = new ManagerSql().GetCountTreatDO(this.castFilter);
                listCountCategoryRdo = new ManagerSql().GetCountCategoryDO(this.castFilter);
                listToTalPriceRdo = new ManagerSql().GetTotalPriceDO(this.castFilter);
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

               
                if (listCountExamRdo != null && listCountExamRdo.Count > 0)
                {
                    COUNT_EXAM = listCountExamRdo.Sum(s => s.COUNT);
                    DIC_COUNT_EXAM = listCountExamRdo.GroupBy(o => DepartmentCode(o.DEPARTMENT_ID)).ToDictionary(p => p.Key, p => p.Sum(s => s.COUNT));
                }

                if (listCountTreatRdo != null && listCountTreatRdo.Count > 0)
                {
                    var listCountTreatInRdo = listCountTreatRdo.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    COUNT_TREATIN = listCountTreatInRdo.Sum(s => s.COUNT);
                    DIC_COUNT_TREATIN = listCountTreatInRdo.GroupBy(o => DepartmentCode(o.DEPARTMENT_ID)).ToDictionary(p => p.Key, p => p.Sum(s => s.COUNT));
                    DATE_TREATIN = listCountTreatInRdo.Sum(s => s.TOTAL_DATE);
                    DIC_DATE_TREATIN = listCountTreatInRdo.GroupBy(o => DepartmentCode(o.DEPARTMENT_ID)).ToDictionary(p => p.Key, p => p.Sum(s => s.TOTAL_DATE));
                    var listCountTreatOutRdo = listCountTreatRdo.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                    COUNT_TREATOUT = listCountTreatOutRdo.Sum(s => s.COUNT);
                    DIC_COUNT_TREATOUT = listCountTreatOutRdo.GroupBy(o => DepartmentCode(o.DEPARTMENT_ID)).ToDictionary(p => p.Key, p => p.Sum(s => s.COUNT));
                }

                if (listCountCategoryRdo != null && listCountCategoryRdo.Count > 0)
                {
                    DIC_AMOUNT_CATEGORY = listCountCategoryRdo.GroupBy(o => CategoryCode(o.CATEGORY_CODE)).ToDictionary(p => p.Key, p => p.Sum(s => s.COUNT));
                    DIC_AMOUNT_CATEGORY_DEPARTMENT = listCountCategoryRdo.GroupBy(o => DepartmentCategoryCode(o.DEPARTMENT_ID, o.CATEGORY_CODE)).ToDictionary(p => p.Key, p => p.Sum(s => s.COUNT));
                }

                if (listToTalPriceRdo != null && listToTalPriceRdo.Count > 0)
                {
                    TOTAL_PRICE = listToTalPriceRdo.Sum(s => s.COUNT);
                    DIC_TOTAL_PRICE = listToTalPriceRdo.GroupBy(o => DepartmentCode(o.DEPARTMENT_ID)).ToDictionary(p => p.Key, p => p.Sum(s => s.COUNT));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private string DepartmentCategoryCode(long departmentId, string CategoryCode)
        {
            string result = "";
            try
            {
                result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                     + "_" + (CategoryCode ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string CategoryCode(string CategoryCode)
        {
            string result = "";
            try
            {
                result = CategoryCode ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string DepartmentCode(long departmentId)
        {
            string result = "";
            try
            {
                result = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "";
                Inventec.Common.Logging.LogSystem.Info(result);
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

            dicSingleTag.Add("COUNT_EXAM", COUNT_EXAM);

            dicSingleTag.Add("COUNT_TREATIN", COUNT_TREATIN);

            dicSingleTag.Add("DATE_TREATIN", DATE_TREATIN);

            dicSingleTag.Add("COUNT_TREATOUT", COUNT_TREATOUT);

            dicSingleTag.Add("DIC_AMOUNT_CATEGORY", DIC_AMOUNT_CATEGORY);

            dicSingleTag.Add("DIC_COUNT_EXAM", DIC_COUNT_EXAM);

            dicSingleTag.Add("DIC_COUNT_TREATIN", DIC_COUNT_TREATIN);

            dicSingleTag.Add("DIC_DATE_TREATIN", DIC_DATE_TREATIN);

            dicSingleTag.Add("DIC_COUNT_TREATOUT", DIC_COUNT_TREATOUT);

            dicSingleTag.Add("DIC_AMOUNT_CATEGORY_DEPARTMENT", DIC_AMOUNT_CATEGORY_DEPARTMENT);

            dicSingleTag.Add("TOTAL_PRICE", TOTAL_PRICE);

            dicSingleTag.Add("DIC_TOTAL_PRICE", DIC_TOTAL_PRICE);

            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }
    }
}
