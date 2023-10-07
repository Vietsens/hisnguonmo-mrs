using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaProvince.Get.ListEv
{
    class SdaProvinceGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaProvinceGetListEv
    {
        SdaProvinceFilterQuery filterQuery;

        internal SdaProvinceGetListEvBehaviorByFilterQuery(CommonParam param, SdaProvinceFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_PROVINCE> ISdaProvinceGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaProvinceDAO.Get(filterQuery.Query(), param);
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
