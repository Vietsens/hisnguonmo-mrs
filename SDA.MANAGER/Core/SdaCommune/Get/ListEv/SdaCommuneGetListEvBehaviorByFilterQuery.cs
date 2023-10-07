using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaCommune.Get.ListEv
{
    class SdaCommuneGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaCommuneGetListEv
    {
        SdaCommuneFilterQuery filterQuery;

        internal SdaCommuneGetListEvBehaviorByFilterQuery(CommonParam param, SdaCommuneFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_COMMUNE> ISdaCommuneGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaCommuneDAO.Get(filterQuery.Query(), param);
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
