using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytMalaria.Get.ListEv
{
    class TytMalariaGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytMalariaGetListEv
    {
        TytMalariaFilterQuery filterQuery;

        internal TytMalariaGetListEvBehaviorByFilterQuery(CommonParam param, TytMalariaFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_MALARIA> ITytMalariaGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytMalariaDAO.Get(filterQuery.Query(), param);
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
