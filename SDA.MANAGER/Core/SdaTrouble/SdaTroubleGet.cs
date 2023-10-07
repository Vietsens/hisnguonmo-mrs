using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaTrouble.Get.Ev;
using SDA.MANAGER.Core.SdaTrouble.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaTrouble
{
    partial class SdaTroubleGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaTroubleGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_TROUBLE>))
                {
                    ISdaTroubleGetListEv behavior = SdaTroubleGetListEvBehaviorFactory.MakeISdaTroubleGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_TROUBLE))
                {
                    ISdaTroubleGetEv behavior = SdaTroubleGetEvBehaviorFactory.MakeISdaTroubleGetEv(param, entity);
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
