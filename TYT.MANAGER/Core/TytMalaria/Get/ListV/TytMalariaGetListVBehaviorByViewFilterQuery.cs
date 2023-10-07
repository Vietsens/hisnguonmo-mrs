using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytMalaria.Get.ListV
{
    class TytMalariaGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytMalariaGetListV
    {
        TytMalariaViewFilterQuery filterQuery;

        internal TytMalariaGetListVBehaviorByViewFilterQuery(CommonParam param, TytMalariaViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_MALARIA> ITytMalariaGetListV.Run()
        {
            try
            {
                return DAOWorker.TytMalariaDAO.GetView(filterQuery.Query(), param);
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
