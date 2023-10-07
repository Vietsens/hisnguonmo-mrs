using System;

namespace TYT.MANAGER.Core.TytFetusExam
{
    partial class TytFetusExamBO : BusinessObjectBase
    {
        internal TytFetusExamBO()
            : base()
        {

        }

        internal T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                IDelegacyT delegacy = new TytFetusExamGet(param, data);
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
