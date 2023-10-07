using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisMestRoomCFG
    {
        private static List<HIS_MEST_ROOM> activeData;
        public static List<HIS_MEST_ROOM> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisMestRoomGet().GetActive();
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
            var data = new HisMestRoomGet().GetActive();
            activeData = data;
        }
    }
}
