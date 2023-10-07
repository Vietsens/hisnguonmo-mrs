using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytKhh.Get.Ev
{
    class TytKhhGetEvBehaviorById : BeanObjectBase, ITytKhhGetEv
    {
        long id;

        internal TytKhhGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_KHH ITytKhhGetEv.Run()
        {
            try
            {
                return DAOWorker.TytKhhDAO.GetById(id, new TytKhhFilterQuery().Query());
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
