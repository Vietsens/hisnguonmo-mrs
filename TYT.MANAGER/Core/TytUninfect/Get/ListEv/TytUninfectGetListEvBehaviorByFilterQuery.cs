using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfect.Get.ListEv
{
    class TytUninfectGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytUninfectGetListEv
    {
        TytUninfectFilterQuery filterQuery;

        internal TytUninfectGetListEvBehaviorByFilterQuery(CommonParam param, TytUninfectFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_UNINFECT> ITytUninfectGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytUninfectDAO.Get(filterQuery.Query(), param);
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
