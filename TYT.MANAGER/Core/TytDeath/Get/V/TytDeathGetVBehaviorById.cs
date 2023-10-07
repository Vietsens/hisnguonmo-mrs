using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytDeath.Get.V
{
    class TytDeathGetVBehaviorById : BeanObjectBase, ITytDeathGetV
    {
        long id;

        internal TytDeathGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_DEATH ITytDeathGetV.Run()
        {
            try
            {
                return DAOWorker.TytDeathDAO.GetViewById(id, new TytDeathViewFilterQuery().Query());
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
