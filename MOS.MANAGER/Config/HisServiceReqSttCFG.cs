using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceReqStt;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisServiceReqSttCFG
    {
        private static List<HIS_SERVICE_REQ_STT> data;
        public static List<HIS_SERVICE_REQ_STT> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisServiceReqSttGet().Get(new HisServiceReqSttFilterQuery());
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
            var tmp = new HisServiceReqSttGet().Get(new HisServiceReqSttFilterQuery());
            data = tmp;
        }
    }
}
