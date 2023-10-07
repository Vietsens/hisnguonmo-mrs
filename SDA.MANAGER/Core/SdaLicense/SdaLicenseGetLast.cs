using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaLicense.Get.GetLast;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaLicense
{
    partial class SdaLicenseGetLast : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal SdaLicenseGetLast(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(SDA_LICENSE))
                {
                    ISdaLicenseGetLast behavior = SdaLicenseGetLastBehaviorFactory.MakeISdaLicenseGetLast(param, entity);
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
