using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytDeath.Get.V
{
    class TytDeathGetVBehaviorByCode : BeanObjectBase, ITytDeathGetV
    {
        string code;

        internal TytDeathGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_DEATH ITytDeathGetV.Run()
        {
            try
            {
                return DAOWorker.TytDeathDAO.GetViewByCode(code, new TytDeathViewFilterQuery().Query());
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
