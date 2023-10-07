using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytUninfectIcd.Get.Ev;
using TYT.MANAGER.Core.TytUninfectIcd.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfectIcd
{
    partial class TytUninfectIcdGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytUninfectIcdGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_UNINFECT_ICD>))
                {
                    ITytUninfectIcdGetListEv behavior = TytUninfectIcdGetListEvBehaviorFactory.MakeITytUninfectIcdGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_UNINFECT_ICD))
                {
                    ITytUninfectIcdGetEv behavior = TytUninfectIcdGetEvBehaviorFactory.MakeITytUninfectIcdGetEv(param, entity);
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
