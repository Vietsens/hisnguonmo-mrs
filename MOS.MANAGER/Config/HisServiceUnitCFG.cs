using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisServiceUnit;

namespace MOS.MANAGER.Config
{
    class HisServiceUnitCFG
    {
        private static List<HIS_SERVICE_UNIT> data;
        public static List<HIS_SERVICE_UNIT> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisServiceUnitGet().Get(new HisServiceUnitFilterQuery());
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
            var tmp = new HisServiceUnitGet().Get(new HisServiceUnitFilterQuery());
            data = tmp;
        }
    }
}
