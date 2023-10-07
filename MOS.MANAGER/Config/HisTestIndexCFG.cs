using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTestIndex;
using MOS.MANAGER.HisTestIndexRange;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisTestIndexCFG
    {
        private static List<V_HIS_TEST_INDEX> activeDataView;
        public static List<V_HIS_TEST_INDEX> ACTIVE_DATA_VIEW
        {
            get
            {
                if (activeDataView == null)
                {
                    activeDataView = new HisTestIndexGet().GetActiveView();
                }
                return activeDataView;
            }
            set
            {
                activeDataView = value;
            }
        }

        public static void Reload()
        {
            var data = new HisTestIndexGet().GetActiveView();
            activeDataView = data;
        }
    }
}
