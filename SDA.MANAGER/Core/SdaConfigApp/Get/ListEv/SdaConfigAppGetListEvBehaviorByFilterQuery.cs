using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaConfigApp.Get.ListEv
{
    class SdaConfigAppGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaConfigAppGetListEv
    {
        SdaConfigAppFilterQuery filterQuery;

        internal SdaConfigAppGetListEvBehaviorByFilterQuery(CommonParam param, SdaConfigAppFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_CONFIG_APP> ISdaConfigAppGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigAppDAO.Get(filterQuery.Query(), param);
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
