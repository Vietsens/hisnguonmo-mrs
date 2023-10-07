using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00575
{
    class Mrs00575Processor : AbstractProcessor
    {
        Mrs00575Filter castFilter = null;
        List<TYT.EFMODEL.DataModels.TYT_KHH> ListTyuKhh = new List<TYT.EFMODEL.DataModels.TYT_KHH>();

        public Mrs00575Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00575Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                var param = new CommonParam();
                castFilter = (Mrs00575Filter)this.reportFilter;

                var tytKhhFilter = new TYT.MANAGER.Core.TytKhh.Get.TytKhhFilterQuery();
                tytKhhFilter.CREATE_TIME_FROM = castFilter.TIME_FROM;
                tytKhhFilter.CREATE_TIME_TO = castFilter.TIME_TO;

                ListTyuKhh = new TYT.MANAGER.Manager.TytKhhManager(param).Get<List<TYT.EFMODEL.DataModels.TYT_KHH>>(tytKhhFilter);
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {

            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));

                if (!IsNotNull(ListTyuKhh))
                {
                    ListTyuKhh = new List<TYT.EFMODEL.DataModels.TYT_KHH>();
                }

                objectTag.AddObjectData(store, "Report", ListTyuKhh);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
