using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00577
{
    class Mrs00577Processor : AbstractProcessor
    {
        Mrs00577Filter castFilter = null;
        List<TYT.EFMODEL.DataModels.TYT_FETUS_EXAM> ListTytFetusExam = new List<TYT.EFMODEL.DataModels.TYT_FETUS_EXAM>();

        public Mrs00577Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00577Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                var param = new CommonParam();
                castFilter = (Mrs00577Filter)this.reportFilter;

                var tytFetusExamFilter = new TYT.MANAGER.Core.TytFetusExam.Get.TytFetusExamFilterQuery();
                tytFetusExamFilter.CREATE_TIME_FROM = castFilter.TIME_FROM;
                tytFetusExamFilter.CREATE_TIME_TO = castFilter.TIME_TO;

                ListTytFetusExam = new TYT.MANAGER.Manager.TytFetusExamManager(param).Get<List<TYT.EFMODEL.DataModels.TYT_FETUS_EXAM>>(tytFetusExamFilter);
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

                if (!IsNotNull(ListTytFetusExam))
                {
                    ListTytFetusExam = new List<TYT.EFMODEL.DataModels.TYT_FETUS_EXAM>();
                }

                objectTag.AddObjectData(store, "Report", ListTytFetusExam);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
