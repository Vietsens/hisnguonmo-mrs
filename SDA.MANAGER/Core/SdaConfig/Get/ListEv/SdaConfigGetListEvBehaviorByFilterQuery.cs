using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaConfig.Get.ListEv
{
    class SdaConfigGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaConfigGetListEv
    {
        SdaConfigFilterQuery filterQuery;

        internal SdaConfigGetListEvBehaviorByFilterQuery(CommonParam param, SdaConfigFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_CONFIG> ISdaConfigGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigDAO.Get(filterQuery.Query(), param);
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
