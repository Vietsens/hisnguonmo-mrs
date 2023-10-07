using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaEventLog.Get.Ev;
using SDA.MANAGER.Core.SdaEventLog.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaEventLog
{
    partial class SdaEventLogGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaEventLogGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_EVENT_LOG>))
                {
                    ISdaEventLogGetListEv behavior = SdaEventLogGetListEvBehaviorFactory.MakeISdaEventLogGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_EVENT_LOG))
                {
                    ISdaEventLogGetEv behavior = SdaEventLogGetEvBehaviorFactory.MakeISdaEventLogGetEv(param, entity);
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
