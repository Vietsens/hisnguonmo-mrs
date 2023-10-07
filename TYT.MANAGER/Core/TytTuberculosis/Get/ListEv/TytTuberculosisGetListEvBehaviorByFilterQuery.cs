using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytTuberculosis.Get.ListEv
{
    class TytTuberculosisGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytTuberculosisGetListEv
    {
        TytTuberculosisFilterQuery filterQuery;

        internal TytTuberculosisGetListEvBehaviorByFilterQuery(CommonParam param, TytTuberculosisFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_TUBERCULOSIS> ITytTuberculosisGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytTuberculosisDAO.Get(filterQuery.Query(), param);
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
