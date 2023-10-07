using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytHiv.Get.ListV
{
    class TytHivGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytHivGetListV
    {
        TytHivViewFilterQuery filterQuery;

        internal TytHivGetListVBehaviorByViewFilterQuery(CommonParam param, TytHivViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_HIV> ITytHivGetListV.Run()
        {
            try
            {
                return DAOWorker.TytHivDAO.GetView(filterQuery.Query(), param);
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
