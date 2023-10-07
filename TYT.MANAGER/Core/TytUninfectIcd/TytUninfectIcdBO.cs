using System;

namespace TYT.MANAGER.Core.TytUninfectIcd
{
    partial class TytUninfectIcdBO : BusinessObjectBase
    {
        internal TytUninfectIcdBO()
            : base()
        {

        }

        internal T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                IDelegacyT delegacy = new TytUninfectIcdGet(param, data);
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
