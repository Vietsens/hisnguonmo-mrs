using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisImpMestStt;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisImpMestSttCFG
    {
        private static List<HIS_IMP_MEST_STT> data;
        public static List<HIS_IMP_MEST_STT> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisImpMestSttGet().Get(new HisImpMestSttFilterQuery());
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
            var datas = new HisImpMestSttGet().Get(new HisImpMestSttFilterQuery());
            data = datas;
        }
    }
}
