using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroup.Get.V
{
    class SdaGroupGetVBehaviorByCode : BeanObjectBase, ISdaGroupGetV
    {
        string code;

        internal SdaGroupGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_SDA_GROUP ISdaGroupGetV.Run()
        {
            try
            {
                return DAOWorker.SdaGroupDAO.GetViewByCode(code, new SdaGroupViewFilterQuery().Query());
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
