using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaConfigAppUser.Get.ListEv
{
    class SdaConfigAppUserGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaConfigAppUserGetListEv
    {
        SdaConfigAppUserFilterQuery filterQuery;

        internal SdaConfigAppUserGetListEvBehaviorByFilterQuery(CommonParam param, SdaConfigAppUserFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_CONFIG_APP_USER> ISdaConfigAppUserGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigAppUserDAO.Get(filterQuery.Query(), param);
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
