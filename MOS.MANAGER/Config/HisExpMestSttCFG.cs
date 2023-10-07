using Inventec.Common.Logging;
using MOS.DAO.HisExpMestStt;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestStt;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisExpMestSttCFG
    {
        private static List<HIS_EXP_MEST_STT> data;
        public static List<HIS_EXP_MEST_STT> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisExpMestSttGet().Get(new HisExpMestSttFilterQuery());
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
            var tmp = new HisExpMestSttGet().Get(new HisExpMestSttFilterQuery());
            data = tmp;
        }
    }
}
