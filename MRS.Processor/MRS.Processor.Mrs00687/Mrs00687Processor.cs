using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
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
using MOS.MANAGER.HisTreatment;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using System.Threading;

namespace MRS.Processor.Mrs00687
{
    public class Mrs00687Processor : AbstractProcessor
    {
        Mrs00687Filter filter = new Mrs00687Filter();
        CommonParam paramGet = new CommonParam();
      
        public Mrs00687Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00687Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = (Mrs00687Filter)this.reportFilter;
                //get dữ liệu:
             

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

               
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

            }
            return result;
        }

      

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());

            objectTag.AddObjectData(store, "Report", new ManagerSql().Get(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15)));

        }

      
    }
}