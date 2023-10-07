using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytDeath.Get.ListV
{
    class TytDeathGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytDeathGetListV
    {
        TytDeathViewFilterQuery filterQuery;

        internal TytDeathGetListVBehaviorByViewFilterQuery(CommonParam param, TytDeathViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_DEATH> ITytDeathGetListV.Run()
        {
            try
            {
                return DAOWorker.TytDeathDAO.GetView(filterQuery.Query(), param);
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
