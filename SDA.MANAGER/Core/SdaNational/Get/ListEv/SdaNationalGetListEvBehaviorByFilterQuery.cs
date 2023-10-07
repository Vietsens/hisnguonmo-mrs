using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaNational.Get.ListEv
{
    class SdaNationalGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaNationalGetListEv
    {
        SdaNationalFilterQuery filterQuery;

        internal SdaNationalGetListEvBehaviorByFilterQuery(CommonParam param, SdaNationalFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_NATIONAL> ISdaNationalGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaNationalDAO.Get(filterQuery.Query(), param);
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
