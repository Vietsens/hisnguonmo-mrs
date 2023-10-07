using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Base;
using Inventec.Core;
using System;

namespace ACS.MANAGER.Core.AcsUser.Get.Ev
{
    class AcsUserGetEvBehaviorByCode : BeanObjectBase, IAcsUserGetEv
    {
        string code;

        internal AcsUserGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        ACS_USER IAcsUserGetEv.Run()
        {
            try
            {
                return DAOWorker.AcsUserDAO.GetByCode(code, new AcsUserFilterQuery().Query());
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
