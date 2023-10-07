using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytHiv.Get.ListEv
{
    class TytHivGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytHivGetListEv
    {
        TytHivFilterQuery filterQuery;

        internal TytHivGetListEvBehaviorByFilterQuery(CommonParam param, TytHivFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_HIV> ITytHivGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytHivDAO.Get(filterQuery.Query(), param);
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
