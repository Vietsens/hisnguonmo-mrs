using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytTuberculosis.Get.ListV
{
    class TytTuberculosisGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytTuberculosisGetListV
    {
        TytTuberculosisViewFilterQuery filterQuery;

        internal TytTuberculosisGetListVBehaviorByViewFilterQuery(CommonParam param, TytTuberculosisViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_TUBERCULOSIS> ITytTuberculosisGetListV.Run()
        {
            try
            {
                return DAOWorker.TytTuberculosisDAO.GetView(filterQuery.Query(), param);
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
