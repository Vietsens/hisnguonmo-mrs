using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get.Ev;
using HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.RevenueBO
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
                if (typeof(T) == typeof(List<HTC_REVENUE>))
                {
                    IHtcRevenueGetListEv behavior = HtcRevenueGetListEvBehaviorFactory.MakeIHtcRevenueGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(HTC_REVENUE))
                {
                    IHtcRevenueGetEv behavior = HtcRevenueGetEvBehaviorFactory.MakeIHtcRevenueGetEv(param, entity);
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
