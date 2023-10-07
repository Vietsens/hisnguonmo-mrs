using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytDeath.Get.Ev;
using TYT.MANAGER.Core.TytDeath.Get.ListEv;
using TYT.MANAGER.Core.TytDeath.Get.ListV;
using TYT.MANAGER.Core.TytDeath.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytDeath
{
    partial class TytDeathGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytDeathGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_DEATH>))
                {
                    ITytDeathGetListEv behavior = TytDeathGetListEvBehaviorFactory.MakeITytDeathGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_DEATH))
                {
                    ITytDeathGetEv behavior = TytDeathGetEvBehaviorFactory.MakeITytDeathGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_TYT_DEATH>))
                {
                    ITytDeathGetListV behavior = TytDeathGetListVBehaviorFactory.MakeITytDeathGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_TYT_DEATH))
                {
                    ITytDeathGetV behavior = TytDeathGetVBehaviorFactory.MakeITytDeathGetV(param, entity);
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
