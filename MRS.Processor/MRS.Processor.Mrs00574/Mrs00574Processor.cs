using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00574
{
    class Mrs00574Processor : AbstractProcessor
    {
        Mrs00574Filter castFilter = null;
        List<TYT.EFMODEL.DataModels.TYT_FETUS_ABORTION> ListTytFetusAbortion = new List<TYT.EFMODEL.DataModels.TYT_FETUS_ABORTION>();
        List<Mrs00574RDO> ListRdo = new List<Mrs00574RDO>();

        public Mrs00574Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00574Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                var param = new CommonParam();
                castFilter = (Mrs00574Filter)this.reportFilter;

                var tytFetusAbortionFilter = new TYT.MANAGER.Core.TytFetusAbortion.Get.TytFetusAbortionFilterQuery();
                tytFetusAbortionFilter.CREATE_TIME_FROM = castFilter.TIME_FROM;
                tytFetusAbortionFilter.CREATE_TIME_TO = castFilter.TIME_TO;

                ListTytFetusAbortion = new TYT.MANAGER.Manager.TytFetusAbortionManager(param).Get<List<TYT.EFMODEL.DataModels.TYT_FETUS_ABORTION>>(tytFetusAbortionFilter);

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
                if (IsNotNullOrEmpty(ListTytFetusAbortion))
                {
                    foreach (var item in ListTytFetusAbortion)
                    {
                        var rdo = new Mrs00574RDO(item);
                        if (IsNotNull(rdo))
                        {
                            ListRdo.Add(rdo);
                        }
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));

                if (!IsNotNull(ListRdo))
                {
                    ListRdo = new List<Mrs00574RDO>();
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
