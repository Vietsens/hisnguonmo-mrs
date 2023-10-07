using System;

namespace TYT.MANAGER.Core.TytTuberculosis
{
    partial class TytTuberculosisBO : BusinessObjectBase
    {
        internal TytTuberculosisBO()
            : base()
        {

        }

        internal T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                IDelegacyT delegacy = new TytTuberculosisGet(param, data);
                result = delegacy.Execute<T>();
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
