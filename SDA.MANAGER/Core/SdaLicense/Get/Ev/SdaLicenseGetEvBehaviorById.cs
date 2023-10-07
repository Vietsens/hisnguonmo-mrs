using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaLicense.Get.Ev
{
    class SdaLicenseGetEvBehaviorById : BeanObjectBase, ISdaLicenseGetEv
    {
        long id;

        internal SdaLicenseGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_LICENSE ISdaLicenseGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaLicenseDAO.GetById(id, new SdaLicenseFilterQuery().Query());
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
