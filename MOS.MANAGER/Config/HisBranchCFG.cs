using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using MOS.MANAGER.HisService;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisBranchCFG
    {
        private static List<HIS_BRANCH> activeData;
        public static List<HIS_BRANCH> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    HisBranchFilterQuery filter = new HisBranchFilterQuery();
                    filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    activeData = new HisBranchGet().Get(filter);
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
            HisBranchFilterQuery filter = new HisBranchFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            var data = new HisBranchGet().Get(filter);
            activeData = data;
        }
    }
}
