using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytFetusAbortion.Get.Ev;
using TYT.MANAGER.Core.TytFetusAbortion.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusAbortion
{
    partial class TytFetusAbortionGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytFetusAbortionGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_FETUS_ABORTION>))
                {
                    ITytFetusAbortionGetListEv behavior = TytFetusAbortionGetListEvBehaviorFactory.MakeITytFetusAbortionGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_FETUS_ABORTION))
                {
                    ITytFetusAbortionGetEv behavior = TytFetusAbortionGetEvBehaviorFactory.MakeITytFetusAbortionGetEv(param, entity);
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
