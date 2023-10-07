using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using MOS.MANAGER.HisServicePaty;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisServicePatyCFG
    {
        private static List<V_HIS_SERVICE_PATY> activeData;
        public static List<V_HIS_SERVICE_PATY> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisServicePatyGet().GetViewActive();
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
            var data = new HisServicePatyGet().GetViewActive();
            activeData = data;
        }
    }
}
