using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.Check
{
    class SarUserReportTypeCheckVerifyExistsCode
    {
        internal static bool Verify(CommonParam param, string code, long? id)
        {
            bool result = true;
            try
            {
                if (SAR.MANAGER.Base.DAOWorker.SarUserReportTypeDAO.ExistsCode(code, id))
                {
                    SAR.MANAGER.Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Common__MaDaTonTaiTrenHeThong);
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
