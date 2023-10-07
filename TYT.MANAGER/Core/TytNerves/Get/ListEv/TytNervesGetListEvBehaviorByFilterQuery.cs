using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytNerves.Get.ListEv
{
    class TytNervesGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytNervesGetListEv
    {
        TytNervesFilterQuery filterQuery;

        internal TytNervesGetListEvBehaviorByFilterQuery(CommonParam param, TytNervesFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_NERVES> ITytNervesGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytNervesDAO.Get(filterQuery.Query(), param);
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
