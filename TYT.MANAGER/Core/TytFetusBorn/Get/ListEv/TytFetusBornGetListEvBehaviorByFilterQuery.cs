using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusBorn.Get.ListEv
{
    class TytFetusBornGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytFetusBornGetListEv
    {
        TytFetusBornFilterQuery filterQuery;

        internal TytFetusBornGetListEvBehaviorByFilterQuery(CommonParam param, TytFetusBornFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_FETUS_BORN> ITytFetusBornGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytFetusBornDAO.Get(filterQuery.Query(), param);
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
