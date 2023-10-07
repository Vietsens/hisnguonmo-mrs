using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisServiceReqType;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisServiceReqTypeCFG
    {
        private static List<HIS_SERVICE_REQ_TYPE> data;
        public static List<HIS_SERVICE_REQ_TYPE> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisServiceReqTypeGet().Get(new HisServiceReqTypeFilterQuery());
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
            var tmp = new HisServiceReqTypeGet().Get(new HisServiceReqTypeFilterQuery());
            data = tmp;
        }
    }
}
