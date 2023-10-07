using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaLicense.Get.Ev
{
    class SdaLicenseGetEvBehaviorByCode : BeanObjectBase, ISdaLicenseGetEv
    {
        string code;

        internal SdaLicenseGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_LICENSE ISdaLicenseGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaLicenseDAO.GetByCode(code, new SdaLicenseFilterQuery().Query());
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
