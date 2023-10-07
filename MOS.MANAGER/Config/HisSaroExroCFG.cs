using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using MOS.MANAGER.HisSaroExro;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTestIndex;
using MOS.MANAGER.HisTestIndexRange;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisSaroExroCFG
    {
        private static List<HIS_SARO_EXRO> activeData;
        public static List<HIS_SARO_EXRO> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisSaroExroGet().GetActive();
                }
                return activeData;
            }
            set
            {
                activeData = value;
            }
        }

        public static void Reload()
        {
            var data = new HisSaroExroGet().GetActive();
            activeData = data;
        }
    }
}
