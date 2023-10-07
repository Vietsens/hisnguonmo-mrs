using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytNerves.Get.Ev
{
    class TytNervesGetEvBehaviorById : BeanObjectBase, ITytNervesGetEv
    {
        long id;

        internal TytNervesGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_NERVES ITytNervesGetEv.Run()
        {
            try
            {
                return DAOWorker.TytNervesDAO.GetById(id, new TytNervesFilterQuery().Query());
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
