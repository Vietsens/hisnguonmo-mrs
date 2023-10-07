

namespace MOS.MANAGER.Config
{
    class HisMobaImpMestCFG
    {
        private const string NOT_ALLOW_FOR_CABINET_CFG = "MOS.HIS_MOBA_IMP_MEST.NOT_ALLOW_FOR_CABINET";

        private static bool? notAllowForCabinet;
        public static bool NOT_ALLOW_FOR_CABINET
        {
            get
            {
                if (!notAllowForCabinet.HasValue)
                {
                    notAllowForCabinet = ConfigUtil.GetIntConfig(NOT_ALLOW_FOR_CABINET_CFG) == 1;
                }
                return notAllowForCabinet.Value;
            }
            set
            {
                notAllowForCabinet = value;
            }
        }

        public static void Reload()
        {
            notAllowForCabinet = ConfigUtil.GetIntConfig(NOT_ALLOW_FOR_CABINET_CFG) == 1;
        }
    }
}
