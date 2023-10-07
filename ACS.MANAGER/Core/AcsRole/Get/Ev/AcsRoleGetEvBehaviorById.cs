using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Base;
using Inventec.Core;
using System;

namespace ACS.MANAGER.Core.AcsRole.Get.Ev
{
    class AcsRoleGetEvBehaviorById : BeanObjectBase, IAcsRoleGetEv
    {
        long id;

        internal AcsRoleGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        ACS_ROLE IAcsRoleGetEv.Run()
        {
            try
            {
                return DAOWorker.AcsRoleDAO.GetById(id, new AcsRoleFilterQuery().Query());
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
