using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using System;

namespace MOS.MANAGER.Config
{
    class HisPayFormCFG
    {
        //PAY_FORM_CODE cua loai "tien mat"
        private const string PAY_FORM_CODE__CASH = "MOS.PAY_FORM_CODE__CASH";

        private static long payFormIdCash;
        public static long PAY_FORM_ID__CASH
        {
            get
            {
                if (payFormIdCash == 0)
                {
                    payFormIdCash = GetId(PAY_FORM_CODE__CASH);
                }
                return payFormIdCash;
            }
            set
            {
                payFormIdCash = value;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                var data = new HisPayForm.HisPayFormGet().GetByCode(value);
                if (data == null) throw new ArgumentNullException(code);
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        public static void Reload()
        {
            var idCash = GetId(PAY_FORM_CODE__CASH);
            payFormIdCash = idCash;
        }
    }
}
