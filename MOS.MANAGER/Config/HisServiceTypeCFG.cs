using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceType;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisServiceTypeCFG
    {
        private static List<HIS_SERVICE_TYPE> data;
        public static List<HIS_SERVICE_TYPE> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisServiceTypeGet().Get(new HisServiceTypeFilterQuery());
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
            var tmp = new HisServiceTypeGet().Get(new HisServiceTypeFilterQuery());
            data = tmp;
        }
    }
}
