using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaEthnic.Get.Ev;
using SDA.MANAGER.Core.SdaEthnic.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaEthnic
{
    partial class SdaEthnicGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaEthnicGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_ETHNIC>))
                {
                    ISdaEthnicGetListEv behavior = SdaEthnicGetListEvBehaviorFactory.MakeISdaEthnicGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_ETHNIC))
                {
                    ISdaEthnicGetEv behavior = SdaEthnicGetEvBehaviorFactory.MakeISdaEthnicGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = default(T);
            }
            return result;
        }
    }
}
