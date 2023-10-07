using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaConfigAppUser.Get.ListV
{
    class SdaConfigAppUserGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISdaConfigAppUserGetListV
    {
        SdaConfigAppUserViewFilterQuery filterQuery;

        internal SdaConfigAppUserGetListVBehaviorByViewFilterQuery(CommonParam param, SdaConfigAppUserViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SDA_CONFIG_APP_USER> ISdaConfigAppUserGetListV.Run()
        {
            try
            {
                return DAOWorker.SdaConfigAppUserDAO.GetView(filterQuery.Query(), param);
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
