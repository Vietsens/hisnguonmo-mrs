using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytTuberculosis.Get.Ev
{
    class TytTuberculosisGetEvBehaviorById : BeanObjectBase, ITytTuberculosisGetEv
    {
        long id;

        internal TytTuberculosisGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_TUBERCULOSIS ITytTuberculosisGetEv.Run()
        {
            try
            {
                return DAOWorker.TytTuberculosisDAO.GetById(id, new TytTuberculosisFilterQuery().Query());
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
