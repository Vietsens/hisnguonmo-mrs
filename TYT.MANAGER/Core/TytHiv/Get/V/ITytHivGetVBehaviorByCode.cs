using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytHiv.Get.V
{
    class TytHivGetVBehaviorByCode : BeanObjectBase, ITytHivGetV
    {
        string code;

        internal TytHivGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_HIV ITytHivGetV.Run()
        {
            try
            {
                return DAOWorker.TytHivDAO.GetViewByCode(code, new TytHivViewFilterQuery().Query());
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
