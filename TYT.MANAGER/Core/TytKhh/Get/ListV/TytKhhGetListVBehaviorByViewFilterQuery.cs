using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytKhh.Get.ListV
{
    class TytKhhGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytKhhGetListV
    {
        TytKhhViewFilterQuery filterQuery;

        internal TytKhhGetListVBehaviorByViewFilterQuery(CommonParam param, TytKhhViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_KHH> ITytKhhGetListV.Run()
        {
            try
            {
                return DAOWorker.TytKhhDAO.GetView(filterQuery.Query(), param);
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
