using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRoom;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisServiceRoomCFG
    {
        private static List<V_HIS_SERVICE_ROOM> activeDataView;
        public static List<V_HIS_SERVICE_ROOM> ACTIVE_DATA_VIEW
        {
            get
            {
                if (activeDataView == null)
                {
                    activeDataView = new HisServiceRoomGet().GetActiveView();
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
            var data = new HisServiceRoomGet().GetActiveView();
            activeDataView = data;
        }
    }
}
