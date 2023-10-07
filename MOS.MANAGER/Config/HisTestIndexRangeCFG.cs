using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTestIndexRange;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisTestIndexRangeCFG
    {
        private static List<HIS_TEST_INDEX_RANGE> activeData;
        public static List<HIS_TEST_INDEX_RANGE> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisTestIndexRangeGet().GetActive();
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
            var data = new HisTestIndexRangeGet().GetActive();
            activeData = data;
        }
    }
}
