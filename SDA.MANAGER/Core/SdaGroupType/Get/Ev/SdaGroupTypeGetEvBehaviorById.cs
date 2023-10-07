using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroupType.Get.Ev
{
    class SdaGroupTypeGetEvBehaviorById : BeanObjectBase, ISdaGroupTypeGetEv
    {
        long id;

        internal SdaGroupTypeGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_GROUP_TYPE ISdaGroupTypeGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaGroupTypeDAO.GetById(id, new SdaGroupTypeFilterQuery().Query());
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
