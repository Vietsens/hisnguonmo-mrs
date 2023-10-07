using System;

namespace TYT.MANAGER.Core.TytDeath
{
    partial class TytDeathBO : BusinessObjectBase
    {
        internal TytDeathBO()
            : base()
        {

        }

        internal T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                IDelegacyT delegacy = new TytDeathGet(param, data);
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
