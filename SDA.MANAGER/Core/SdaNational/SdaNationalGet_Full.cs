using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaNational.Get.Ev;
using SDA.MANAGER.Core.SdaNational.Get.ListEv;
using SDA.MANAGER.Core.SdaNational.Get.ListV;
using SDA.MANAGER.Core.SdaNational.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaNational
{
    partial class SdaNationalGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaNationalGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_NATIONAL>))
                {
                    ISdaNationalGetListEv behavior = SdaNationalGetListEvBehaviorFactory.MakeISdaNationalGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_NATIONAL))
                {
                    ISdaNationalGetEv behavior = SdaNationalGetEvBehaviorFactory.MakeISdaNationalGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_SDA_NATIONAL>))
                {
                    ISdaNationalGetListV behavior = SdaNationalGetListVBehaviorFactory.MakeISdaNationalGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_SDA_NATIONAL))
                {
                    ISdaNationalGetV behavior = SdaNationalGetVBehaviorFactory.MakeISdaNationalGetV(param, entity);
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
