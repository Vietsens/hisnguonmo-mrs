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
using MOS.MANAGER.HisVitaminA;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisBaby;

namespace MRS.Processor.Mrs00611
{
    class Mrs00611Processor : AbstractProcessor
    {
        Mrs00611Filter castFilter = null;
        List<Mrs00611RDO> listRdo = new List<Mrs00611RDO>();
        List<Mrs00611RDO> listPrint = new List<Mrs00611RDO>();
        HIS_BRANCH CurrentBranch;
        string thisReportTypeCode = "";
        public Mrs00611Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00611Filter);
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                if (this.CurrentBranch != null)
                {
                    dicSingleTag.Add("BRANCH_NAME", this.CurrentBranch.BRANCH_NAME);
                    dicSingleTag.Add("BRANCH_CODE", this.CurrentBranch.BRANCH_CODE);
                }
                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listPrint);
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00611Filter)this.reportFilter;
                this.CurrentBranch = new Mrs00611RDOManager().GetBranch(this.castFilter);
                this.listRdo = new Mrs00611RDOManager().GetRdo(this.castFilter);
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
                CommonParam paramGet = new CommonParam();
                listPrint = new List<Mrs00611RDO>();

                var groupListRdo = listRdo.GroupBy(o => o.PATIENT_ID);
                foreach (var ListChild in groupListRdo)
                {
                    if (ListChild != null && ListChild.Count() > 0)
                    {
                        var ListChildItem = ListChild.OrderBy(o => o.EXECUTE_TIME).ToList();
                        foreach (var item in ListChildItem)
                        {
                            Mrs00611RDO mrs00611RDO = new Mrs00611RDO();
                            AutoMapper.Mapper.CreateMap<Mrs00611RDO, Mrs00611RDO>();
                            mrs00611RDO = AutoMapper.Mapper.Map<Mrs00611RDO>(item);
                            if (item.EXECUTE_TIME.HasValue)
                            {
                                mrs00611RDO.DESCRIPTION = ((Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.EXECUTE_TIME ?? 0) ?? DateTime.MinValue) - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.TDL_PATIENT_DOB) ?? DateTime.MinValue)).TotalDays * 0.0328549112;
                            }
                            else
                            {
                                mrs00611RDO.DESCRIPTION = null;
                            }
                            listPrint.Add(mrs00611RDO);
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


    }
}
