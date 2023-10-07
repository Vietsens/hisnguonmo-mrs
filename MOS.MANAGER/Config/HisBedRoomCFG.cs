using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBedRoom;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisBedRoomCFG
    {
        private static List<V_HIS_BED_ROOM> data;
        public static List<V_HIS_BED_ROOM> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisBedRoomGet().GetView(new HisBedRoomViewFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        public static void Reload()
        {
            var tmp = new HisBedRoomGet().GetView(new HisBedRoomViewFilterQuery());
            data = tmp;
        }
    }
}
