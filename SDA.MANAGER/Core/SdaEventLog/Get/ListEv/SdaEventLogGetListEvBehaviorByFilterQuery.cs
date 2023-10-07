using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaEventLog.Get.ListEv
{
    class SdaEventLogGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaEventLogGetListEv
    {
        SdaEventLogFilterQuery filterQuery;

        internal SdaEventLogGetListEvBehaviorByFilterQuery(CommonParam param, SdaEventLogFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_EVENT_LOG> ISdaEventLogGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaEventLogDAO.Get(filterQuery.Query(), param);
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
