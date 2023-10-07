using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisExecuteRoomCFG
    {
        private static List<V_HIS_EXECUTE_ROOM> activeData;
        public static List<V_HIS_EXECUTE_ROOM> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisExecuteRoomGet().GetViewActive();
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
            var data = new HisExecuteRoomGet().GetViewActive();
            activeData = data;
        }
    }
}
