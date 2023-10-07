using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisMilitaryRank;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisMilitaryRankCFG
    {
        private static List<HIS_MILITARY_RANK> data;
        public static List<HIS_MILITARY_RANK> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisMilitaryRankGet().Get(new HisMilitaryRankFilterQuery());
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
            var tmp = new HisMilitaryRankGet().Get(new HisMilitaryRankFilterQuery());
            data = tmp;
        }
    }
}
