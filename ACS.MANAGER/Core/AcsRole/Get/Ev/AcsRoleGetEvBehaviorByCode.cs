using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Base;
using Inventec.Core;
using System;

namespace ACS.MANAGER.Core.AcsRole.Get.Ev
{
    class AcsRoleGetEvBehaviorByCode : BeanObjectBase, IAcsRoleGetEv
    {
        string code;

        internal AcsRoleGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        ACS_ROLE IAcsRoleGetEv.Run()
        {
            try
            {
                return DAOWorker.AcsRoleDAO.GetByCode(code, new AcsRoleFilterQuery().Query());
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
