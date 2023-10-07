using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisRoom;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisRoomCFG
    {
        private static List<V_HIS_ROOM> data;
        public static List<V_HIS_ROOM> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisRoomGet().GetView(new HisRoomViewFilterQuery());
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
            var tmp = new HisRoomGet().GetView(new HisRoomViewFilterQuery());
            data = tmp;
        }
    }
}
