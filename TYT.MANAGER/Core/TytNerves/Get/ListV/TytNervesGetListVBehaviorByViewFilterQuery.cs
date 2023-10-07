using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytNerves.Get.ListV
{
    class TytNervesGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytNervesGetListV
    {
        TytNervesViewFilterQuery filterQuery;

        internal TytNervesGetListVBehaviorByViewFilterQuery(CommonParam param, TytNervesViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_NERVES> ITytNervesGetListV.Run()
        {
            try
            {
                return DAOWorker.TytNervesDAO.GetView(filterQuery.Query(), param);
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
