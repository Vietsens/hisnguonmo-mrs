using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusBorn.Get.V
{
    class TytFetusBornGetVBehaviorByCode : BeanObjectBase, ITytFetusBornGetV
    {
        string code;

        internal TytFetusBornGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_FETUS_BORN ITytFetusBornGetV.Run()
        {
            try
            {
                return DAOWorker.TytFetusBornDAO.GetViewByCode(code, new TytFetusBornViewFilterQuery().Query());
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
