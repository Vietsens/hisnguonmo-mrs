using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytMalaria.Get.V
{
    class TytMalariaGetVBehaviorByCode : BeanObjectBase, ITytMalariaGetV
    {
        string code;

        internal TytMalariaGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_MALARIA ITytMalariaGetV.Run()
        {
            try
            {
                return DAOWorker.TytMalariaDAO.GetViewByCode(code, new TytMalariaViewFilterQuery().Query());
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
