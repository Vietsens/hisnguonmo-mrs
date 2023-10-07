using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaEthnic.Get.Ev
{
    class SdaEthnicGetEvBehaviorById : BeanObjectBase, ISdaEthnicGetEv
    {
        long id;

        internal SdaEthnicGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_ETHNIC ISdaEthnicGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaEthnicDAO.GetById(id, new SdaEthnicFilterQuery().Query());
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
