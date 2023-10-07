using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using SAR.EFMODEL.DataModels;
using SAR.Filter;
using MOS.LibraryHein;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace MRS.Processor.Mrs00677
{

    class Mrs00677Processor : AbstractProcessor
    {
        Mrs00677Filter castFilter = new Mrs00677Filter();

        protected string BRANCH_NAME = "";
        List<Mrs00677RDO> Total = new List<Mrs00677RDO>();
        List<Mrs00677RDO> program = new List<Mrs00677RDO>();
        List<Mrs00677RDO> Emergency = new List<Mrs00677RDO>();
        List<Mrs00677RDO> Required = new List<Mrs00677RDO>();
        List<SURGERY> listSurgery = new List<SURGERY>();
        long PtttGroupId1;
        long PtttGroupId2;
        long PtttGroupId3;
        long PtttGroupIdDb;

        long PtttPriority_Program;
        long PtttPriority_Emergency;
        long PtttPriority_Required;
        public Mrs00677Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00677Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {

                 PtttGroupId1 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1;
                 PtttGroupId2 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2;
                 PtttGroupId3 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3;
                 PtttGroupIdDb = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4;

                 PtttPriority_Program = HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__P;
                 PtttPriority_Emergency = HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__CC;
                 PtttPriority_Required = HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__YC;
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00677Filter)this.reportFilter;
                listSurgery = new ManagerSql().GetSuregry(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15))??new List<SURGERY>();
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
                if (listSurgery.Count > 0)
                {
                    var groupByDepartment = listSurgery.GroupBy(o => o.DEPARTMENT_NAME).ToList();
                    foreach (var item in groupByDepartment)
                    {
                        var surgerySub = item.ToList<SURGERY>();
                        Mrs00677RDO rdoSurgery = processRdo(surgerySub);
                        Total.Add(rdoSurgery);
                        Mrs00677RDO rdoProgram = processRdo(surgerySub.Where(o => o.PTTT_PRIORITY_ID == PtttPriority_Program).ToList());
                        if (IsNotNullOrEmpty(rdoProgram.DEPARTMENT_NAME))
                        {
                            program.Add(rdoProgram);
                        }
                        Mrs00677RDO rdoEmergency = processRdo(surgerySub.Where(o => o.PTTT_PRIORITY_ID == PtttPriority_Emergency).ToList());
                        if (IsNotNullOrEmpty(rdoEmergency.DEPARTMENT_NAME))
                        {
                            Emergency.Add(rdoEmergency);
                        }

                        Mrs00677RDO rdoRequired = processRdo(surgerySub.Where(o => o.PTTT_PRIORITY_ID == PtttPriority_Required).ToList());
                        if (IsNotNullOrEmpty(rdoRequired.DEPARTMENT_NAME))
                        {
                            Required.Add(rdoRequired);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private Mrs00677RDO processRdo(List<SURGERY> surgerySub)
        {
            Mrs00677RDO result = new Mrs00677RDO();
            try
            {
                result.DEPARTMENT_NAME = (surgerySub.FirstOrDefault()??new SURGERY()).DEPARTMENT_NAME;
                result.COUNT_PTTT_GROUP_ID_1 = surgerySub.Where(o => o.PTTT_GROUP_ID == PtttGroupId1).ToList().Count;
                result.COUNT_PTTT_GROUP_ID_2 = surgerySub.Where(o => o.PTTT_GROUP_ID == PtttGroupId2).ToList().Count;
                result.COUNT_PTTT_GROUP_ID_3 = surgerySub.Where(o => o.PTTT_GROUP_ID == PtttGroupId3).ToList().Count;
                result.COUNT_PTTT_GROUP_ID_DB = surgerySub.Where(o => o.PTTT_GROUP_ID == PtttGroupIdDb).ToList().Count;
                result.COUNT_PTTT_GROUP_ID_SUM = surgerySub.ToList().Count;
                result.COUNT_IN_TIME = surgerySub.Where(o => o.IS_IN_TIME == 1).ToList().Count;
                result.COUNT_OUT_TIME = surgerySub.Where(o => o.IS_OUT_TIME == 1).ToList().Count;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new Mrs00677RDO() ;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                objectTag.AddObjectData(store, "Total", Total);

                objectTag.AddObjectData(store, "Program", program);

                objectTag.AddObjectData(store, "Emergency", Emergency);

                objectTag.AddObjectData(store, "Required", Required);

                objectTag.AddObjectData(store, "MisuDsa", new ManagerSql().Get(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 16)));
                objectTag.AddObjectData(store, "Date", new ManagerSql().Get(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 17)));
                objectTag.AddRelationship(store, "Date", "MisuDsa", "START_DATE", "START_DATE");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
