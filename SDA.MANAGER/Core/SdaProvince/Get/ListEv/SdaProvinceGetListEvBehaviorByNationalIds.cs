using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaProvince.Get.ListEv
{
    class SdaProvinceGetListEvBehaviorByNationalIds : BeanObjectBase, ISdaProvinceGetListEv
    {
        List<long> nationalIds;

        internal SdaProvinceGetListEvBehaviorByNationalIds(CommonParam param, List<long> _nationalIds)
            : base(param)
        {
            nationalIds = _nationalIds;
        }

        List<SDA_PROVINCE> ISdaProvinceGetListEv.Run()
        {
            try
            {
                SdaProvinceFilterQuery filter = new SdaProvinceFilterQuery();
                filter.NATIONAL_IDs = nationalIds;
                return DAOWorker.SdaProvinceDAO.Get(filter.Query(), param);
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
