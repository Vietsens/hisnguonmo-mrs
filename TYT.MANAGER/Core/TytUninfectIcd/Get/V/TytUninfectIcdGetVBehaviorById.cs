using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcd.Get.V
{
    class TytUninfectIcdGetVBehaviorById : BeanObjectBase, ITytUninfectIcdGetV
    {
        long id;

        internal TytUninfectIcdGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_UNINFECT_ICD ITytUninfectIcdGetV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdDAO.GetViewById(id, new TytUninfectIcdViewFilterQuery().Query());
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
