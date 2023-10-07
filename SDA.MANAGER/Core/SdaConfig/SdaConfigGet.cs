using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaConfig.Get.Ev;
using SDA.MANAGER.Core.SdaConfig.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaConfig
{
    partial class SdaConfigGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaConfigGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_CONFIG>))
                {
                    ISdaConfigGetListEv behavior = SdaConfigGetListEvBehaviorFactory.MakeISdaConfigGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_CONFIG))
                {
                    ISdaConfigGetEv behavior = SdaConfigGetEvBehaviorFactory.MakeISdaConfigGetEv(param, entity);
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
