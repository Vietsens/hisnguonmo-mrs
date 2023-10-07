using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using MOS.MANAGER.HisTranPatiForm;

namespace MRS.MANAGER.Config
{
    public class HisTranPatiFormCFG
    {
        private const string HIS_TRAN_PATI_FORM_CODE__UP_DOWN = "MRS.HIS_TRAN_PATI_FORM.HIS_TRAN_PATI_FORM_CODE__UP_DOWN";
        private const string HIS_TRAN_PATI_FORM_CODE__DOWN_UP_NEXT = "MRS.HIS_TRAN_PATI_FORM.HIS_TRAN_PATI_FORM_CODE__DOWN_UP_NEXT";//Chuyển người bệnh từ tuyến dưới lên tuyến trên liền kề
        private const string HIS_TRAN_PATI_FORM_CODE__EQUAL = "MRS.HIS_TRAN_PATI_FORM.HIS_TRAN_PATI_FORM_CODE__EQUAL";
        private const string HIS_TRAN_PATI_FORM_CODE__DOWN_UP_NON_NEXT = "MRS.HIS_TRAN_PATI_FORM.HIS_TRAN_PATI_FORM_CODE__DOWN_UP_NON_NEXT";//Chuyển người bệnh từ tuyến dưới lên tuyến trên không qua tuyến liền kề

        private static long tranPatiFormIdUpDown;
        public static long HIS_TRAN_PATI_FORM_ID__UP_DOWN
        {
            get
            {
                if (tranPatiFormIdUpDown == 0)
                {
                    tranPatiFormIdUpDown = GetId(HIS_TRAN_PATI_FORM_CODE__UP_DOWN);
                }
                return tranPatiFormIdUpDown;
            }
            set
            {
                tranPatiFormIdUpDown = value;
            }
        }

        private static long tranPatiFormIdDownUpNext;
        public static long HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT
        {
            get
            {
                if (tranPatiFormIdDownUpNext == 0)
                {
                    tranPatiFormIdDownUpNext = GetId(HIS_TRAN_PATI_FORM_CODE__DOWN_UP_NEXT);
                }
                return tranPatiFormIdDownUpNext;
            }
            set
            {
                tranPatiFormIdDownUpNext = value;
            }
        }

        private static long tranPatiFormIdDownUpNonNext;
        public static long HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT
        {
            get
            {
                if (tranPatiFormIdDownUpNonNext == 0)
                {
                    tranPatiFormIdDownUpNonNext = GetId(HIS_TRAN_PATI_FORM_CODE__DOWN_UP_NON_NEXT);
                }
                return tranPatiFormIdDownUpNonNext;
            }
            set
            {
                tranPatiFormIdDownUpNonNext = value;
            }
        }

        private static long tranPatiFormIdEqual;
        public static long HIS_TRAN_PATI_FORM_ID__EQUAL
        {
            get
            {
                if (tranPatiFormIdEqual == 0)
                {
                    tranPatiFormIdEqual = GetId(HIS_TRAN_PATI_FORM_CODE__EQUAL);
                }
                return tranPatiFormIdEqual;
            }
            set
            {
                tranPatiFormIdEqual = value;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                HisTranPatiFormFilterQuery filter = new HisTranPatiFormFilterQuery();
                //filter.ROOM_TYPE_CODE = value;//TODO
                var data = new HisTranPatiFormManager().Get(filter).FirstOrDefault(o => o.TRAN_PATI_FORM_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                tranPatiFormIdUpDown = 0;
                tranPatiFormIdDownUpNext = 0;
                tranPatiFormIdDownUpNonNext = 0;
                tranPatiFormIdEqual = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
