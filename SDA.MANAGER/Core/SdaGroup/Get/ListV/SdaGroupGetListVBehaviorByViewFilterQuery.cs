using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaGroup.Get.ListV
{
    class SdaGroupGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISdaGroupGetListV
    {
        SdaGroupViewFilterQuery filterQuery;

        internal SdaGroupGetListVBehaviorByViewFilterQuery(CommonParam param, SdaGroupViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SDA_GROUP> ISdaGroupGetListV.Run()
        {
            try
            {
                return DAOWorker.SdaGroupDAO.GetView(filterQuery.Query(), param);
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
