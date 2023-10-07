using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaGroup.Get.ListEv
{
    class SdaGroupGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaGroupGetListEv
    {
        SdaGroupFilterQuery filterQuery;

        internal SdaGroupGetListEvBehaviorByFilterQuery(CommonParam param, SdaGroupFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_GROUP> ISdaGroupGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaGroupDAO.Get(filterQuery.Query(), param);
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
