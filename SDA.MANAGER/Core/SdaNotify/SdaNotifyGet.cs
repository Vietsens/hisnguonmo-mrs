using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaNotify.Get.Ev;
using SDA.MANAGER.Core.SdaNotify.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaNotify
{
    partial class SdaNotifyGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaNotifyGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_NOTIFY>))
                {
                    ISdaNotifyGetListEv behavior = SdaNotifyGetListEvBehaviorFactory.MakeISdaNotifyGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_NOTIFY))
                {
                    ISdaNotifyGetEv behavior = SdaNotifyGetEvBehaviorFactory.MakeISdaNotifyGetEv(param, entity);
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
