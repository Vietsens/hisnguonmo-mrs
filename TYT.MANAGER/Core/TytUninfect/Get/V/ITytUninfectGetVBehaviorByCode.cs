using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfect.Get.V
{
    class TytUninfectGetVBehaviorByCode : BeanObjectBase, ITytUninfectGetV
    {
        string code;

        internal TytUninfectGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_UNINFECT ITytUninfectGetV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectDAO.GetViewByCode(code, new TytUninfectViewFilterQuery().Query());
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
