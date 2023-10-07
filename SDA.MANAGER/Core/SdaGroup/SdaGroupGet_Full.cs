using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaGroup.Get.Ev;
using SDA.MANAGER.Core.SdaGroup.Get.ListEv;
using SDA.MANAGER.Core.SdaGroup.Get.ListV;
using SDA.MANAGER.Core.SdaGroup.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaGroup
{
    partial class SdaGroupGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaGroupGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_GROUP>))
                {
                    ISdaGroupGetListEv behavior = SdaGroupGetListEvBehaviorFactory.MakeISdaGroupGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_GROUP))
                {
                    ISdaGroupGetEv behavior = SdaGroupGetEvBehaviorFactory.MakeISdaGroupGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_SDA_GROUP>))
                {
                    ISdaGroupGetListV behavior = SdaGroupGetListVBehaviorFactory.MakeISdaGroupGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_SDA_GROUP))
                {
                    ISdaGroupGetV behavior = SdaGroupGetVBehaviorFactory.MakeISdaGroupGetV(param, entity);
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
