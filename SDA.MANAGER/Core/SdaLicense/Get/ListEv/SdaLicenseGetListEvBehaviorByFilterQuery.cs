using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaLicense.Get.ListEv
{
    class SdaLicenseGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaLicenseGetListEv
    {
        SdaLicenseFilterQuery filterQuery;

        internal SdaLicenseGetListEvBehaviorByFilterQuery(CommonParam param, SdaLicenseFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_LICENSE> ISdaLicenseGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaLicenseDAO.Get(filterQuery.Query(), param);
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
