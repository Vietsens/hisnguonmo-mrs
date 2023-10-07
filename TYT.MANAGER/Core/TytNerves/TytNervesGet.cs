using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytNerves.Get.Ev;
using TYT.MANAGER.Core.TytNerves.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytNerves
{
    partial class TytNervesGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytNervesGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_NERVES>))
                {
                    ITytNervesGetListEv behavior = TytNervesGetListEvBehaviorFactory.MakeITytNervesGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_NERVES))
                {
                    ITytNervesGetEv behavior = TytNervesGetEvBehaviorFactory.MakeITytNervesGetEv(param, entity);
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
