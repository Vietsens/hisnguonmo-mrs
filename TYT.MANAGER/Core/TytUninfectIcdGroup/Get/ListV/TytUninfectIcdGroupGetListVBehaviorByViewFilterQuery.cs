using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup.Get.ListV
{
    class TytUninfectIcdGroupGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytUninfectIcdGroupGetListV
    {
        TytUninfectIcdGroupViewFilterQuery filterQuery;

        internal TytUninfectIcdGroupGetListVBehaviorByViewFilterQuery(CommonParam param, TytUninfectIcdGroupViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_UNINFECT_ICD_GROUP> ITytUninfectIcdGroupGetListV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdGroupDAO.GetView(filterQuery.Query(), param);
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
