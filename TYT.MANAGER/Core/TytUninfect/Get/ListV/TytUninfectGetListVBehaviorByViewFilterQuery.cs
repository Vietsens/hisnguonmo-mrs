using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfect.Get.ListV
{
    class TytUninfectGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytUninfectGetListV
    {
        TytUninfectViewFilterQuery filterQuery;

        internal TytUninfectGetListVBehaviorByViewFilterQuery(CommonParam param, TytUninfectViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_UNINFECT> ITytUninfectGetListV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectDAO.GetView(filterQuery.Query(), param);
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
