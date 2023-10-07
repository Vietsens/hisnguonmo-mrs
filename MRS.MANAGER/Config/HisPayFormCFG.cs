using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPayForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisPayFormCFG
    {
        //private const string HIS_PAY_FORM_CASH = "DBCODE.HIS_RS.HIS_PAY_FORM.PAY_FORM_CODE.CASH";
        //private const string HIS_PAY_FORM_TRANSFER = "DBCODE.HIS_RS.HIS_PAY_FORM.PAY_FORM_CODE.TRANSFER";
        //private const string HIS_PAY_FORM_CASH_TRANSFER = "DBCODE.HIS_RS.HIS_PAY_FORM.PAY_FORM_CODE.CASH_TRANSFER";

        //private static long HisPayFormIdCash;
        //public static long HisPayFormIdCashStr
        //{
        //    get
        //    {
        //        if (HisPayFormIdCash == null || HisPayFormIdCash == 0)
        //        {
        //            HisPayFormIdCash = GetId(HIS_PAY_FORM_CASH);
        //        }
        //        return HisPayFormIdCash;
        //    }
        //    set
        //    {
        //        HisPayFormIdCash = value;
        //    }
        //}

        //private static long HisPayFormIdCTransfer;
        //public static long HisPayFormIdTransferStr
        //{
        //    get
        //    {
        //        if (HisPayFormIdCTransfer == null || HisPayFormIdCTransfer == 0)
        //        {
        //            HisPayFormIdCTransfer = GetId(HIS_PAY_FORM_TRANSFER);
        //        }
        //        return HisPayFormIdCTransfer;
        //    }
        //    set
        //    {
        //        HisPayFormIdCTransfer = value;
        //    }
        //}

        //private static long HisPayFormIdCashTransfer;
        //public static long HisPayFormIdCashTransferStr
        //{
        //    get
        //    {
        //        if (HisPayFormIdCashTransfer == null || HisPayFormIdCashTransfer == 0)
        //        {
        //            HisPayFormIdCashTransfer = GetId(HIS_PAY_FORM_CASH_TRANSFER);
        //        }
        //        return HisPayFormIdCashTransfer;
        //    }
        //    set
        //    {
        //        HisPayFormIdCashTransfer = value;
        //    }
        //}

        //private static long GetId(string code)
        //{
        //    long result = 0;
        //    try
        //    {
        //        SDA.EFMODEL.DataModels.SDA_CONFIG config = Loader.dictionaryConfig[code];
        //        if (config == null) throw new ArgumentNullException(code);
        //        string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
        //        if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
        //        HisPayFormFilter filter = new HisPayFormFilter();
        //        //filter.ROOM_TYPE_CODE = value;//TODO
        //        var data = new MRS.MANAGER.His.HisPayFormGet().Get(filter).FirstOrDefault(o => o.PAY_FORM_CODE == value);
        //        if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
        //        result = data.ID;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        result = 0;
        //    }
        //    return result;
        //}

        private static List<HIS_PAY_FORM> lstPayForm;
        public static List<HIS_PAY_FORM> ListPayForm
        {
            get
            {
                if (lstPayForm == null || lstPayForm.Count <= 0)
                {
                    lstPayForm = GetList();
                }
                return lstPayForm;
            }
            set
            {
                lstPayForm = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PAY_FORM> GetList()
        {
            List<MOS.EFMODEL.DataModels.HIS_PAY_FORM> result = null;
            try
            {
                HisPayFormFilterQuery filter = new HisPayFormFilterQuery();
                result = new HisPayFormManager().Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                lstPayForm = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
