using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytHiv.Get.Ev;
using TYT.MANAGER.Core.TytHiv.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytHiv
{
    partial class TytHivGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytHivGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_HIV>))
                {
                    ITytHivGetListEv behavior = TytHivGetListEvBehaviorFactory.MakeITytHivGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_HIV))
                {
                    ITytHivGetEv behavior = TytHivGetEvBehaviorFactory.MakeITytHivGetEv(param, entity);
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
