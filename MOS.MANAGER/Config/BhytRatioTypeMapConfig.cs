using MOS.LibraryHein.Bhyt.HeinRatio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.Config
{
    class BhytRatioTypeMapConfig
    {
        //cau hinh de anh xa giua nhom dich vu BHYT voi nhom dich vu khi tinh ti le BHYT
        private static Dictionary<long, string> HEIN_SERVICE_TYPE__RATIO_TYPE = new Dictionary<long, string>()
        {
            { HisHeinServiceTypeCFG.CDHA, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.DVKT_TTL, HeinRatioTypeCode.HIGH },
            { HisHeinServiceTypeCFG.GI_NGT, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.GI_NT, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.KH, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.MAU, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.PTTT, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.T_DTUT, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.T_NDM, HeinRatioTypeCode.OTHER },
            { HisHeinServiceTypeCFG.T_TDM, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.T_TTL, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.TDCN, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.VC, HeinRatioTypeCode.TRANSPORT },
            { HisHeinServiceTypeCFG.VT_NDM, HeinRatioTypeCode.OTHER },
            { HisHeinServiceTypeCFG.VT_TDM, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.VT_TT, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.VT_TTL, HeinRatioTypeCode.NORMAL },
            { HisHeinServiceTypeCFG.XN, HeinRatioTypeCode.NORMAL }
        };

        public static string GetRatioType(long? heinServiceTypeId)
        {
            if (heinServiceTypeId.HasValue && HEIN_SERVICE_TYPE__RATIO_TYPE != null && HEIN_SERVICE_TYPE__RATIO_TYPE.ContainsKey(heinServiceTypeId.Value))
            {
                return HEIN_SERVICE_TYPE__RATIO_TYPE[heinServiceTypeId.Value];
            }
            return HeinRatioTypeCode.OTHER;//neu ko duoc gan thi mac dinh tra ve loai la dich vu "khac" ==> ko duoc BHYT chi tra
        }
    }
}
