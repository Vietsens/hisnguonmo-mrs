using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytKhh.Get.V
{
    class TytKhhGetVBehaviorByCode : BeanObjectBase, ITytKhhGetV
    {
        string code;

        internal TytKhhGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_KHH ITytKhhGetV.Run()
        {
            try
            {
                return DAOWorker.TytKhhDAO.GetViewByCode(code, new TytKhhViewFilterQuery().Query());
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
