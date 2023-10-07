using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport
{
    partial class SarReportBO : BusinessObjectBase
    {
        internal bool UpdateStt(object data)
        {
            bool result = false;
            try
            {
                IDelegacy delegacy = new SarReportUpdateStt(param, data);
                result = delegacy.Execute();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
