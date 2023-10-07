using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaConfigApp.Get.Ev;
using SDA.MANAGER.Core.SdaConfigApp.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaConfigApp
{
    partial class SdaConfigAppGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaConfigAppGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_CONFIG_APP>))
                {
                    ISdaConfigAppGetListEv behavior = SdaConfigAppGetListEvBehaviorFactory.MakeISdaConfigAppGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_CONFIG_APP))
                {
                    ISdaConfigAppGetEv behavior = SdaConfigAppGetEvBehaviorFactory.MakeISdaConfigAppGetEv(param, entity);
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
