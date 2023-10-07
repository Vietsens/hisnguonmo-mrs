using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class ExeCFG
    {
        private const string WARNING_UNPAID_AMOUNT__EXECUTE_ROOM_CFG = "MOS.WARNING_UNPAID_AMOUNT.EXECUTE_ROOM";
        private const string WARNING_UNPAID_AMOUNT__BED_ROOM_CFG = "MOS.WARNING_UNPAID_AMOUNT.BED_ROOM";

        private static decimal? warningUnpaidAmountExecuteRoom;
        public static decimal? WARNING_UNPAID_AMOUNT__EXECUTE_ROOM
        {
            get
            {
                if (!warningUnpaidAmountExecuteRoom.HasValue)
                {
                    warningUnpaidAmountExecuteRoom = ConfigUtil.GetDecimalConfig(WARNING_UNPAID_AMOUNT__EXECUTE_ROOM_CFG) ?? -1;
                }
                return warningUnpaidAmountExecuteRoom;
            }
            set
            {
                warningUnpaidAmountExecuteRoom = value;
            }
        }

        private static decimal? warningUnpaidAmountBedRoom;
        public static decimal? WARNING_UNPAID_AMOUNT__BED_ROOM
        {
            get
            {
                if (!warningUnpaidAmountBedRoom.HasValue)
                {
                    warningUnpaidAmountBedRoom = ConfigUtil.GetDecimalConfig(WARNING_UNPAID_AMOUNT__BED_ROOM_CFG) ?? -1;
                }
                return warningUnpaidAmountBedRoom;
            }
            set
            {
                warningUnpaidAmountBedRoom = value;
            }
        }

        public static void Reload()
        {
            warningUnpaidAmountExecuteRoom = ConfigUtil.GetDecimalConfig(WARNING_UNPAID_AMOUNT__EXECUTE_ROOM_CFG) ?? -1;
            warningUnpaidAmountBedRoom = ConfigUtil.GetDecimalConfig(WARNING_UNPAID_AMOUNT__BED_ROOM_CFG) ?? -1;
        }
    }
}
