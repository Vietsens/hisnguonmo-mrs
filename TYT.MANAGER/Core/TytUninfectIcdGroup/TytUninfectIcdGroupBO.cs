using System;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup
{
    partial class TytUninfectIcdGroupBO : BusinessObjectBase
    {
        internal TytUninfectIcdGroupBO()
            : base()
        {

        }

        internal T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                IDelegacyT delegacy = new TytUninfectIcdGroupGet(param, data);
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
