using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00578;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Repository;

namespace MRS.Processor.Mrs00578
{
    public class Mrs00578Processor : AbstractProcessor
    {
        private List<Mrs00578RDO> ListRdo = new List<Mrs00578RDO>();
        Mrs00578Filter filter = null;
        string thisReportTypeCode = "";

        public Mrs00578Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00578Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00578Filter)this.reportFilter;
            try
            {
                if (filter.IS_TRANSACTION_TIME_OR_INTRUCTION_TIME != false)
                {
                    ListRdo = new Mrs00578RDOManager().GetMrs00578RDOByTransactionTime(filter.PARENT_SERVICE_CODE, filter.TRANSACTION_TIME_FROM, filter.TRANSACTION_TIME_TO, filter.REQUEST_DEPARTMENT_ID);
                }
                else
                {
                    ListRdo = new Mrs00578RDOManager().GetMrs00578RDOByIntructionTime(filter.PARENT_SERVICE_CODE, filter.TRANSACTION_TIME_FROM, filter.TRANSACTION_TIME_TO, filter.REQUEST_DEPARTMENT_ID);
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
                if (ListRdo != null && ListRdo.Count > 0)
                {
                    foreach (var item in ListRdo)
                    {
                        item.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TRANSACTION_TIME ?? 0);
                        item.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.DOB ?? 0);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00578RDO>();
                result = false;
            }
            return result;
        }





        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TRANSACTION_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TRANSACTION_TIME_FROM));
            dicSingleTag.Add("TRANSACTION_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TRANSACTION_TIME_TO));

            objectTag.AddObjectData(store, "Report", ListRdo ?? new List<Mrs00578RDO>());
            objectTag.AddObjectData(store, "Intruction", ListRdo ?? new List<Mrs00578RDO>());

        }

    }

}
