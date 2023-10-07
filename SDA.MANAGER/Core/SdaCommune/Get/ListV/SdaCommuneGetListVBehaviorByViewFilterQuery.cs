using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaCommune.Get.ListV
{
    class SdaCommuneGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISdaCommuneGetListV
    {
        SdaCommuneViewFilterQuery filterQuery;

        internal SdaCommuneGetListVBehaviorByViewFilterQuery(CommonParam param, SdaCommuneViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SDA_COMMUNE> ISdaCommuneGetListV.Run()
        {
            try
            {
                return DAOWorker.SdaCommuneDAO.GetView(filterQuery.Query(), param);
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
