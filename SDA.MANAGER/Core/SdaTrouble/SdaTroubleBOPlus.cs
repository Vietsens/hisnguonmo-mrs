using System;

namespace SDA.MANAGER.Core.SdaTrouble
{
    partial class SdaTroubleBO : BusinessObjectBase
    {
        internal bool CreateByMessage(object data)
        {
            bool result = false;
            try
            {
                IDelegacy delegacy = new SdaTroubleCreateByMessage(param, data);
                result = delegacy.Execute();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        internal bool Scan()
        {
            bool result = false;
            try
            {
                IDelegacy delegacy = new SdaTroubleScan(param);
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
