using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytKhh.Get.V
{
    class TytKhhGetVBehaviorById : BeanObjectBase, ITytKhhGetV
    {
        long id;

        internal TytKhhGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_KHH ITytKhhGetV.Run()
        {
            try
            {
                return DAOWorker.TytKhhDAO.GetViewById(id, new TytKhhViewFilterQuery().Query());
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
