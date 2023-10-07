using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaConfigAppUser.Get.Ev;
using SDA.MANAGER.Core.SdaConfigAppUser.Get.ListEv;
using SDA.MANAGER.Core.SdaConfigAppUser.Get.ListV;
using SDA.MANAGER.Core.SdaConfigAppUser.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaConfigAppUser
{
    partial class SdaConfigAppUserGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaConfigAppUserGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_CONFIG_APP_USER>))
                {
                    ISdaConfigAppUserGetListEv behavior = SdaConfigAppUserGetListEvBehaviorFactory.MakeISdaConfigAppUserGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_CONFIG_APP_USER))
                {
                    ISdaConfigAppUserGetEv behavior = SdaConfigAppUserGetEvBehaviorFactory.MakeISdaConfigAppUserGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_SDA_CONFIG_APP_USER>))
                {
                    ISdaConfigAppUserGetListV behavior = SdaConfigAppUserGetListVBehaviorFactory.MakeISdaConfigAppUserGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_SDA_CONFIG_APP_USER))
                {
                    ISdaConfigAppUserGetV behavior = SdaConfigAppUserGetVBehaviorFactory.MakeISdaConfigAppUserGetV(param, entity);
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
