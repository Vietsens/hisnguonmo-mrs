using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytUninfectIcdGroup.Get.Ev;
using TYT.MANAGER.Core.TytUninfectIcdGroup.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup
{
    partial class TytUninfectIcdGroupGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytUninfectIcdGroupGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_UNINFECT_ICD_GROUP>))
                {
                    ITytUninfectIcdGroupGetListEv behavior = TytUninfectIcdGroupGetListEvBehaviorFactory.MakeITytUninfectIcdGroupGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_UNINFECT_ICD_GROUP))
                {
                    ITytUninfectIcdGroupGetEv behavior = TytUninfectIcdGroupGetEvBehaviorFactory.MakeITytUninfectIcdGroupGetEv(param, entity);
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
