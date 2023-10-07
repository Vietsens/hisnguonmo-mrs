using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfect.Get.V
{
    class TytUninfectGetVBehaviorById : BeanObjectBase, ITytUninfectGetV
    {
        long id;

        internal TytUninfectGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_UNINFECT ITytUninfectGetV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectDAO.GetViewById(id, new TytUninfectViewFilterQuery().Query());
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
