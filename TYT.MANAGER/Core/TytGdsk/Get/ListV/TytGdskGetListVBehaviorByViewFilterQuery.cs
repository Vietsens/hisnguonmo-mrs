using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytGdsk.Get.ListV
{
    class TytGdskGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytGdskGetListV
    {
        TytGdskViewFilterQuery filterQuery;

        internal TytGdskGetListVBehaviorByViewFilterQuery(CommonParam param, TytGdskViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_GDSK> ITytGdskGetListV.Run()
        {
            try
            {
                return DAOWorker.TytGdskDAO.GetView(filterQuery.Query(), param);
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
