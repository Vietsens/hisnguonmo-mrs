using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.Check
{
    class SarReportCheckVerifyExistsCode
    {
        internal static bool Verify(CommonParam param, string code, long? id)
        {
            bool result = true;
            try
            {
                if (SAR.MANAGER.Base.DAOWorker.SarReportDAO.ExistsCode(code, id))
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
                param.HasException = true;
            }
            return result;
        }
    }
}
