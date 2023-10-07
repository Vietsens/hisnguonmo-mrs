using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfectIcd.Get.ListV
{
    class TytUninfectIcdGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytUninfectIcdGetListV
    {
        TytUninfectIcdViewFilterQuery filterQuery;

        internal TytUninfectIcdGetListVBehaviorByViewFilterQuery(CommonParam param, TytUninfectIcdViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_UNINFECT_ICD> ITytUninfectIcdGetListV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdDAO.GetView(filterQuery.Query(), param);
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
