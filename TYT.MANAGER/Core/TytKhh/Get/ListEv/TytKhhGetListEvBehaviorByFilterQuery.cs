using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytKhh.Get.ListEv
{
    class TytKhhGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytKhhGetListEv
    {
        TytKhhFilterQuery filterQuery;

        internal TytKhhGetListEvBehaviorByFilterQuery(CommonParam param, TytKhhFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_KHH> ITytKhhGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytKhhDAO.Get(filterQuery.Query(), param);
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
