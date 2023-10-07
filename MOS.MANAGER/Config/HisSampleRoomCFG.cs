using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSampleRoom;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisSampleRoomCFG
    {
        private static List<V_HIS_SAMPLE_ROOM> activeData;
        public static List<V_HIS_SAMPLE_ROOM> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisSampleRoomGet().GetViewActive();
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
            var data = new HisSampleRoomGet().GetViewActive();
            activeData = data;
        }
    }
}
