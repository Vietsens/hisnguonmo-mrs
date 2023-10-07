using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusBorn.Get.V
{
    class TytFetusBornGetVBehaviorById : BeanObjectBase, ITytFetusBornGetV
    {
        long id;

        internal TytFetusBornGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_FETUS_BORN ITytFetusBornGetV.Run()
        {
            try
            {
                return DAOWorker.TytFetusBornDAO.GetViewById(id, new TytFetusBornViewFilterQuery().Query());
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
