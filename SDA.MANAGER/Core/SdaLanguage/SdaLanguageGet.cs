using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaLanguage.Get.Ev;
using SDA.MANAGER.Core.SdaLanguage.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaLanguage
{
    partial class SdaLanguageGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaLanguageGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_LANGUAGE>))
                {
                    ISdaLanguageGetListEv behavior = SdaLanguageGetListEvBehaviorFactory.MakeISdaLanguageGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_LANGUAGE))
                {
                    ISdaLanguageGetEv behavior = SdaLanguageGetEvBehaviorFactory.MakeISdaLanguageGetEv(param, entity);
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
