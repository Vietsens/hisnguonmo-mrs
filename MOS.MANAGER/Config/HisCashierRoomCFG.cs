using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisCashierRoomCFG
    {
        private static List<V_HIS_CASHIER_ROOM> activeData;
        public static List<V_HIS_CASHIER_ROOM> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisCashierRoomGet().GetViewActive();
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
            var data = new HisCashierRoomGet().GetViewActive();
            activeData = data;
        }
    }
}
