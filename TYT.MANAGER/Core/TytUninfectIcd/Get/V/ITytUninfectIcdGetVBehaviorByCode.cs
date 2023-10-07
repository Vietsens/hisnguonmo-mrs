using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcd.Get.V
{
    class TytUninfectIcdGetVBehaviorByCode : BeanObjectBase, ITytUninfectIcdGetV
    {
        string code;

        internal TytUninfectIcdGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_UNINFECT_ICD ITytUninfectIcdGetV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdDAO.GetViewByCode(code, new TytUninfectIcdViewFilterQuery().Query());
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
