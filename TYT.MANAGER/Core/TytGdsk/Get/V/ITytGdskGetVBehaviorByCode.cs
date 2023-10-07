using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytGdsk.Get.V
{
    class TytGdskGetVBehaviorByCode : BeanObjectBase, ITytGdskGetV
    {
        string code;

        internal TytGdskGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_GDSK ITytGdskGetV.Run()
        {
            try
            {
                return DAOWorker.TytGdskDAO.GetViewByCode(code, new TytGdskViewFilterQuery().Query());
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
