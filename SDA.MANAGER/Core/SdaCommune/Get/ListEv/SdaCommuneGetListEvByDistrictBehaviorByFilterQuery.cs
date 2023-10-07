using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaCommune.Get.ListEv
{
    class SdaCommuneGetListEvByDistrictBehaviorByFilterQuery : BeanObjectBase, ISdaCommuneGetListEv
    {
        List<long> disTrictIds;

        internal SdaCommuneGetListEvByDistrictBehaviorByFilterQuery(CommonParam param, List<long> districtIds)
            : base(param)
        {
            disTrictIds = districtIds;
        }

        List<SDA_COMMUNE> ISdaCommuneGetListEv.Run()
        {
            try
            {
                SdaCommuneFilterQuery filter = new SdaCommuneFilterQuery();
                filter.DISTRICT_IDs = disTrictIds;
                return DAOWorker.SdaCommuneDAO.Get(filter.Query(), param);
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
