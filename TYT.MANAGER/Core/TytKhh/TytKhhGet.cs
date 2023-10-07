using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytKhh.Get.Ev;
using TYT.MANAGER.Core.TytKhh.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytKhh
{
    partial class TytKhhGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytKhhGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_KHH>))
                {
                    ITytKhhGetListEv behavior = TytKhhGetListEvBehaviorFactory.MakeITytKhhGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_KHH))
                {
                    ITytKhhGetEv behavior = TytKhhGetEvBehaviorFactory.MakeITytKhhGetEv(param, entity);
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
