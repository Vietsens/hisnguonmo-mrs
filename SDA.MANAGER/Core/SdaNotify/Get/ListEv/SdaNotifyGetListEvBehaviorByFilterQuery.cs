using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaNotify.Get.ListEv
{
    class SdaNotifyGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaNotifyGetListEv
    {
        SdaNotifyFilterQuery filterQuery;

        internal SdaNotifyGetListEvBehaviorByFilterQuery(CommonParam param, SdaNotifyFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_NOTIFY> ISdaNotifyGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaNotifyDAO.Get(filterQuery.Query(), param);
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
