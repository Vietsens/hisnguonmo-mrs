using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaTranslate.Get.Ev;
using SDA.MANAGER.Core.SdaTranslate.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaTranslate
{
    partial class SdaTranslateGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaTranslateGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_TRANSLATE>))
                {
                    ISdaTranslateGetListEv behavior = SdaTranslateGetListEvBehaviorFactory.MakeISdaTranslateGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_TRANSLATE))
                {
                    ISdaTranslateGetEv behavior = SdaTranslateGetEvBehaviorFactory.MakeISdaTranslateGetEv(param, entity);
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
