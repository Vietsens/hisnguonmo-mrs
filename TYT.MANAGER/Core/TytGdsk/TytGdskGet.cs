using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytGdsk.Get.Ev;
using TYT.MANAGER.Core.TytGdsk.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytGdsk
{
    partial class TytGdskGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytGdskGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_GDSK>))
                {
                    ITytGdskGetListEv behavior = TytGdskGetListEvBehaviorFactory.MakeITytGdskGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_GDSK))
                {
                    ITytGdskGetEv behavior = TytGdskGetEvBehaviorFactory.MakeITytGdskGetEv(param, entity);
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
