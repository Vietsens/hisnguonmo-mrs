using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisIcd;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisIcdCFG
    {
        private static List<HIS_ICD> data;
        public static List<HIS_ICD> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisIcdGet().Get(new HisIcdFilterQuery());
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
            var tmp = new HisIcdGet().Get(new HisIcdFilterQuery());
            data = tmp;
        }
    }
}
