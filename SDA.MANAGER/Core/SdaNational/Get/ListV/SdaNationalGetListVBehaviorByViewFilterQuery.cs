using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaNational.Get.ListV
{
    class SdaNationalGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISdaNationalGetListV
    {
        SdaNationalViewFilterQuery filterQuery;

        internal SdaNationalGetListVBehaviorByViewFilterQuery(CommonParam param, SdaNationalViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SDA_NATIONAL> ISdaNationalGetListV.Run()
        {
            try
            {
                return DAOWorker.SdaNationalDAO.GetView(filterQuery.Query(), param);
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
