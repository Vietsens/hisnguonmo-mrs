using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytMalaria.Get.Ev;
using TYT.MANAGER.Core.TytMalaria.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytMalaria
{
    partial class TytMalariaGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytMalariaGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_MALARIA>))
                {
                    ITytMalariaGetListEv behavior = TytMalariaGetListEvBehaviorFactory.MakeITytMalariaGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_MALARIA))
                {
                    ITytMalariaGetEv behavior = TytMalariaGetEvBehaviorFactory.MakeITytMalariaGetEv(param, entity);
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
