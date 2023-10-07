using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytDeath.Get.ListEv
{
    class TytDeathGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytDeathGetListEv
    {
        TytDeathFilterQuery filterQuery;

        internal TytDeathGetListEvBehaviorByFilterQuery(CommonParam param, TytDeathFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_DEATH> ITytDeathGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytDeathDAO.Get(filterQuery.Query(), param);
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
