using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytNerves.Get.V
{
    class TytNervesGetVBehaviorById : BeanObjectBase, ITytNervesGetV
    {
        long id;

        internal TytNervesGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_NERVES ITytNervesGetV.Run()
        {
            try
            {
                return DAOWorker.TytNervesDAO.GetViewById(id, new TytNervesViewFilterQuery().Query());
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
