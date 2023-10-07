using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaProvince.Get.Ev;
using SDA.MANAGER.Core.SdaProvince.Get.ListEv;
using SDA.MANAGER.Core.SdaProvince.Get.ListV;
using SDA.MANAGER.Core.SdaProvince.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaProvince
{
    partial class SdaProvinceGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaProvinceGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_PROVINCE>))
                {
                    ISdaProvinceGetListEv behavior = SdaProvinceGetListEvBehaviorFactory.MakeISdaProvinceGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_PROVINCE))
                {
                    ISdaProvinceGetEv behavior = SdaProvinceGetEvBehaviorFactory.MakeISdaProvinceGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_SDA_PROVINCE>))
                {
                    ISdaProvinceGetListV behavior = SdaProvinceGetListVBehaviorFactory.MakeISdaProvinceGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_SDA_PROVINCE))
                {
                    ISdaProvinceGetV behavior = SdaProvinceGetVBehaviorFactory.MakeISdaProvinceGetV(param, entity);
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
