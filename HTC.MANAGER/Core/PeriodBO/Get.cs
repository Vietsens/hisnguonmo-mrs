using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Core.PeriodBO.HtcPeriod.Get.Ev;
using HTC.MANAGER.Core.PeriodBO.HtcPeriod.Get.ListEv;
using HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.Ev;
using HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.ListEv;
using HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.ListV;
using HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.PeriodBO
{
    partial class Get : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal Get(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<HTC_PERIOD>))
                {
                    IHtcPeriodGetListEv behavior = HtcPeriodGetListEvBehaviorFactory.MakeIHtcPeriodGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(HTC_PERIOD))
                {
                    IHtcPeriodGetEv behavior = HtcPeriodGetEvBehaviorFactory.MakeIHtcPeriodGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<HTC_PERIOD_DEPARTMENT>))
                {
                    IHtcPeriodDepartmentGetListEv behavior = HtcPeriodDepartmentGetListEvBehaviorFactory.MakeIHtcPeriodDepartmentGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(HTC_PERIOD_DEPARTMENT))
                {
                    IHtcPeriodDepartmentGetEv behavior = HtcPeriodDepartmentGetEvBehaviorFactory.MakeIHtcPeriodDepartmentGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_HTC_PERIOD_DEPARTMENT>))
                {
                    IHtcPeriodDepartmentGetListV behavior = HtcPeriodDepartmentGetListVBehaviorFactory.MakeIHtcPeriodDepartmentGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_HTC_PERIOD_DEPARTMENT))
                {
                    IHtcPeriodDepartmentGetV behavior = HtcPeriodDepartmentGetVBehaviorFactory.MakeIHtcPeriodDepartmentGetV(param, entity);
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
