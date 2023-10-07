using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytFetusBorn.Get.Ev;
using TYT.MANAGER.Core.TytFetusBorn.Get.ListEv;
using TYT.MANAGER.Core.TytFetusBorn.Get.ListV;
using TYT.MANAGER.Core.TytFetusBorn.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusBorn
{
    partial class TytFetusBornGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytFetusBornGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_FETUS_BORN>))
                {
                    ITytFetusBornGetListEv behavior = TytFetusBornGetListEvBehaviorFactory.MakeITytFetusBornGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_FETUS_BORN))
                {
                    ITytFetusBornGetEv behavior = TytFetusBornGetEvBehaviorFactory.MakeITytFetusBornGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_TYT_FETUS_BORN>))
                {
                    ITytFetusBornGetListV behavior = TytFetusBornGetListVBehaviorFactory.MakeITytFetusBornGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_TYT_FETUS_BORN))
                {
                    ITytFetusBornGetV behavior = TytFetusBornGetVBehaviorFactory.MakeITytFetusBornGetV(param, entity);
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
