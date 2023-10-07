using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroup.Get.V
{
    class SdaGroupGetVBehaviorById : BeanObjectBase, ISdaGroupGetV
    {
        long id;

        internal SdaGroupGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SDA_GROUP ISdaGroupGetV.Run()
        {
            try
            {
                return DAOWorker.SdaGroupDAO.GetViewById(id, new SdaGroupViewFilterQuery().Query());
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
