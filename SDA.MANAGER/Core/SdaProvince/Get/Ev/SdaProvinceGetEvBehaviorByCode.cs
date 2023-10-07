using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaProvince.Get.Ev
{
    class SdaProvinceGetEvBehaviorByCode : BeanObjectBase, ISdaProvinceGetEv
    {
        string code;

        internal SdaProvinceGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_PROVINCE ISdaProvinceGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaProvinceDAO.GetByCode(code, new SdaProvinceFilterQuery().Query());
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
