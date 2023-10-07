using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytMalaria.Get.V
{
    class TytMalariaGetVBehaviorById : BeanObjectBase, ITytMalariaGetV
    {
        long id;

        internal TytMalariaGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_MALARIA ITytMalariaGetV.Run()
        {
            try
            {
                return DAOWorker.TytMalariaDAO.GetViewById(id, new TytMalariaViewFilterQuery().Query());
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
