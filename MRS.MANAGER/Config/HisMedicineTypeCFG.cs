using System;

namespace MRS.MANAGER.Config
{
    public class HisMedicineTypeCFG
    {
        private const string MRS_HIS_MEDICINE_TYPE_NATIONAL__VN = "MRS.HIS_MEDICINE_TYPE.NATIONAL.VN";

        private static string MedicineTypeNatioanlVN;
        public static string MEDICINE_TYPE_NATIOANL_VN
        {
            get
            {
                if (string.IsNullOrEmpty(MedicineTypeNatioanlVN))
                {
                    var config = Loader.dictionaryConfig[MRS_HIS_MEDICINE_TYPE_NATIONAL__VN];
                    if (config == null) throw new ArgumentNullException(MRS_HIS_MEDICINE_TYPE_NATIONAL__VN);
                    var value = string.IsNullOrEmpty(config.VALUE) ? (string.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                    if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(MRS_HIS_MEDICINE_TYPE_NATIONAL__VN);
                    MedicineTypeNatioanlVN = value;
                }

                return MedicineTypeNatioanlVN;
            }
            set
            {
                MedicineTypeNatioanlVN = value;
            }
        }

        public static void Refresh()
        {
            try
            {
                MedicineTypeNatioanlVN = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
