using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytTuberculosis.Get.Ev;
using TYT.MANAGER.Core.TytTuberculosis.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytTuberculosis
{
    partial class TytTuberculosisGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytTuberculosisGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_TUBERCULOSIS>))
                {
                    ITytTuberculosisGetListEv behavior = TytTuberculosisGetListEvBehaviorFactory.MakeITytTuberculosisGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_TUBERCULOSIS))
                {
                    ITytTuberculosisGetEv behavior = TytTuberculosisGetEvBehaviorFactory.MakeITytTuberculosisGetEv(param, entity);
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
