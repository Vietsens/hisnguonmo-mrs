using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroup.Get.Ev
{
    class SdaGroupGetEvBehaviorById : BeanObjectBase, ISdaGroupGetEv
    {
        long id;

        internal SdaGroupGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_GROUP ISdaGroupGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaGroupDAO.GetById(id, new SdaGroupFilterQuery().Query());
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
