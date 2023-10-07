using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaLicense.Get.Ev;
using SDA.MANAGER.Core.SdaLicense.Get.ListEv;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaLicense
{
    partial class SdaLicenseGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaLicenseGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<SDA_LICENSE>))
                {
                    ISdaLicenseGetListEv behavior = SdaLicenseGetListEvBehaviorFactory.MakeISdaLicenseGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(SDA_LICENSE))
                {
                    ISdaLicenseGetEv behavior = SdaLicenseGetEvBehaviorFactory.MakeISdaLicenseGetEv(param, entity);
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
