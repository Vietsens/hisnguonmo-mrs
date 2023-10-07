using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytHiv.Get.V
{
    class TytHivGetVBehaviorById : BeanObjectBase, ITytHivGetV
    {
        long id;

        internal TytHivGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_HIV ITytHivGetV.Run()
        {
            try
            {
                return DAOWorker.TytHivDAO.GetViewById(id, new TytHivViewFilterQuery().Query());
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
