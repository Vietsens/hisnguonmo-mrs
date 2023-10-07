using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaProvince.Get.V
{
    class SdaProvinceGetVBehaviorByCode : BeanObjectBase, ISdaProvinceGetV
    {
        string code;

        internal SdaProvinceGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_SDA_PROVINCE ISdaProvinceGetV.Run()
        {
            try
            {
                return DAOWorker.SdaProvinceDAO.GetViewByCode(code, new SdaProvinceViewFilterQuery().Query());
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
