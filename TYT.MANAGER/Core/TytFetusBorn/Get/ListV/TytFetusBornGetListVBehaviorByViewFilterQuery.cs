using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusBorn.Get.ListV
{
    class TytFetusBornGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytFetusBornGetListV
    {
        TytFetusBornViewFilterQuery filterQuery;

        internal TytFetusBornGetListVBehaviorByViewFilterQuery(CommonParam param, TytFetusBornViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_FETUS_BORN> ITytFetusBornGetListV.Run()
        {
            try
            {
                return DAOWorker.TytFetusBornDAO.GetView(filterQuery.Query(), param);
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
