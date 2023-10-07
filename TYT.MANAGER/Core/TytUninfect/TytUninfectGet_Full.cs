using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytUninfect.Get.Ev;
using TYT.MANAGER.Core.TytUninfect.Get.ListEv;
using TYT.MANAGER.Core.TytUninfect.Get.ListV;
using TYT.MANAGER.Core.TytUninfect.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfect
{
    partial class TytUninfectGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytUninfectGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_UNINFECT>))
                {
                    ITytUninfectGetListEv behavior = TytUninfectGetListEvBehaviorFactory.MakeITytUninfectGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_UNINFECT))
                {
                    ITytUninfectGetEv behavior = TytUninfectGetEvBehaviorFactory.MakeITytUninfectGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_TYT_UNINFECT>))
                {
                    ITytUninfectGetListV behavior = TytUninfectGetListVBehaviorFactory.MakeITytUninfectGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_TYT_UNINFECT))
                {
                    ITytUninfectGetV behavior = TytUninfectGetVBehaviorFactory.MakeITytUninfectGetV(param, entity);
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
