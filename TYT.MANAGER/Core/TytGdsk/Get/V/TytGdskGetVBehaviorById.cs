using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytGdsk.Get.V
{
    class TytGdskGetVBehaviorById : BeanObjectBase, ITytGdskGetV
    {
        long id;

        internal TytGdskGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_GDSK ITytGdskGetV.Run()
        {
            try
            {
                return DAOWorker.TytGdskDAO.GetViewById(id, new TytGdskViewFilterQuery().Query());
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
