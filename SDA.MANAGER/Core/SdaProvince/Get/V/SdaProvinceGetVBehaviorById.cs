using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaProvince.Get.V
{
    class SdaProvinceGetVBehaviorById : BeanObjectBase, ISdaProvinceGetV
    {
        long id;

        internal SdaProvinceGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SDA_PROVINCE ISdaProvinceGetV.Run()
        {
            try
            {
                return DAOWorker.SdaProvinceDAO.GetViewById(id, new SdaProvinceViewFilterQuery().Query());
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
