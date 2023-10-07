using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Logging;
using MOS.MANAGER.HisReportTypeCat;

namespace MRS.MANAGER.Config
{
    public class HisServiceRetyCatCFG
    {
        private const string CATEGORY_CODE_MRS00143_XQ = "MRS.HIS_REPORT_TYPE_CAT.CATEGORY_CODE.MRS00143_XQ";//Nhóm chụp X-Quang MRS00143
        private const string CATEGORY_CODE_MRS00143_CT = "MRS.HIS_REPORT_TYPE_CAT.CATEGORY_CODE.MRS00143_CT";//Nhóm chụp CT MRS00143
        private const string CATEGORY_CODE_MRS00143_DT = "MRS.HIS_REPORT_TYPE_CAT.CATEGORY_CODE.MRS00143_DT";//Nhóm điện tim MRS00143
        private const string CATEGORY_CODE_MRS00143_YHCT = "MRS.HIS_REPORT_TYPE_CAT.CATEGORY_CODE.MRS00143_YHCT";//Nhóm khám y học cổ truyền

        private static long categoryIdMrs00143YHCT;
        public static long CATEGORY_ID_MRS00143_YHCT
        {
            get
            {
                if (categoryIdMrs00143YHCT == 0)
                {
                    categoryIdMrs00143YHCT = GetId(CATEGORY_CODE_MRS00143_YHCT);
                }
                return categoryIdMrs00143YHCT;
            }
            set
            {
                categoryIdMrs00143YHCT = value;
            }
        }

        private static long categoryIdMrs00143XQ;
        public static long CATEGORY_ID_MRS00143_XQ
        {
            get
            {
                if (categoryIdMrs00143XQ == 0)
                {
                    categoryIdMrs00143XQ = GetId(CATEGORY_CODE_MRS00143_XQ);
                }
                return categoryIdMrs00143XQ;
            }
            set
            {
                categoryIdMrs00143XQ = value;
            }
        }

        private static long categoryIdMrs00143CT;
        public static long CATEGORY_ID_MRS00143_CT
        {
            get
            {
                if (categoryIdMrs00143CT == 0)
                {
                    categoryIdMrs00143CT = GetId(CATEGORY_CODE_MRS00143_CT);
                }
                return categoryIdMrs00143CT;
            }
            set
            {
                categoryIdMrs00143CT = value;
            }
        }

        private static long categoryIdMrs00143DT;
        public static long CATEGORY_ID_MRS00143_DT
        {
            get
            {
                if (categoryIdMrs00143DT == 0)
                {
                    categoryIdMrs00143DT = GetId(CATEGORY_CODE_MRS00143_DT);
                }
                return categoryIdMrs00143DT;
            }
            set
            {
                categoryIdMrs00143DT = value;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                var filter = new HisReportTypeCatFilterQuery();
                filter.CATEGORY_CODE__EXACT = value;
                var data = new HisReportTypeCatManager().Get(filter).First();
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
                categoryIdMrs00143YHCT = 0;
                categoryIdMrs00143XQ = 0;
                categoryIdMrs00143CT = 0;
                categoryIdMrs00143DT = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
