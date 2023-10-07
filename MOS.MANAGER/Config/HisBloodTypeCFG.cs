using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBloodType;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisBloodTypeCFG
    {
        private static List<HIS_BLOOD_TYPE> data;
        public static List<HIS_BLOOD_TYPE> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisBloodTypeGet().Get(new HisBloodTypeFilterQuery());
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
            var tmp = new HisBloodTypeGet().Get(new HisBloodTypeFilterQuery());
            data = tmp;
        }
    }
}
