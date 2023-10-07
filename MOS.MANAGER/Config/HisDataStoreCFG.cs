using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDataStore;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisDataStoreCFG
    {
        private static List<V_HIS_DATA_STORE> activeData;
        public static List<V_HIS_DATA_STORE> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisDataStoreGet().GetViewActive();
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
            var data = new HisDataStoreGet().GetViewActive();
            activeData = data;
        }
    }
}
