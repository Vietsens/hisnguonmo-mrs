using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportStt.Get.ListEv
{
    class SarReportSttGetListEvBehaviorByFilterQuery : BeanObjectBase, ISarReportSttGetListEv
    {
        SarReportSttFilterQuery filterQuery;

        internal SarReportSttGetListEvBehaviorByFilterQuery(CommonParam param, SarReportSttFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SAR_REPORT_STT> ISarReportSttGetListEv.Run()
        {
            try
            {
                return DAOWorker.SarReportSttDAO.Get(filterQuery.Query(), param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
