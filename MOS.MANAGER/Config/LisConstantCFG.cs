using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class LisConstantCFG
    {
        private const string MOS_LIS_FORBID_NOT_ENOUGH_FEE_CFG = "MOS.LIS_FORBID_NOT_ENOUGH_FEE";

        private static bool? lisForbidNotEnoughFee;
        public static bool MOS_LIS_FORBID_NOT_ENOUGH_FEE
        {
            get
            {
                if (lisForbidNotEnoughFee == null)
                {
                    lisForbidNotEnoughFee = ConfigUtil.GetIntConfig(MOS_LIS_FORBID_NOT_ENOUGH_FEE_CFG) == 1;
                }
                return lisForbidNotEnoughFee.Value;
            }
            set
            {
                lisForbidNotEnoughFee = value;
            }
        }

        public static void Reload()
        {
            lisForbidNotEnoughFee = ConfigUtil.GetIntConfig(MOS_LIS_FORBID_NOT_ENOUGH_FEE_CFG) == 1;
        }
    }
}
