using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Base;
using Inventec.Core;
using System;

namespace ACS.MANAGER.Core.AcsUser.Get.Ev
{
    class AcsUserGetEvBehaviorById : BeanObjectBase, IAcsUserGetEv
    {
        long id;

        internal AcsUserGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        ACS_USER IAcsUserGetEv.Run()
        {
            try
            {
                return DAOWorker.AcsUserDAO.GetById(id, new AcsUserFilterQuery().Query());
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
