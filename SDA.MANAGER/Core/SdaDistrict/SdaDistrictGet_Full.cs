using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaDistrict.Get.Ev;
using SDA.MANAGER.Core.SdaDistrict.Get.ListEv;
using SDA.MANAGER.Core.SdaDistrict.Get.ListV;
using SDA.MANAGER.Core.SdaDistrict.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaDistrict
{
    partial class SdaDistrictGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaDistrictGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_DISTRICT>))
                {
                    ISdaDistrictGetListEv behavior = SdaDistrictGetListEvBehaviorFactory.MakeISdaDistrictGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_DISTRICT))
                {
                    ISdaDistrictGetEv behavior = SdaDistrictGetEvBehaviorFactory.MakeISdaDistrictGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_SDA_DISTRICT>))
                {
                    ISdaDistrictGetListV behavior = SdaDistrictGetListVBehaviorFactory.MakeISdaDistrictGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_SDA_DISTRICT))
                {
                    ISdaDistrictGetV behavior = SdaDistrictGetVBehaviorFactory.MakeISdaDistrictGetV(param, entity);
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
