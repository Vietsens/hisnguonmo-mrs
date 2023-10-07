using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisPatientTypeAllowCFG
    {
        private static List<HIS_PATIENT_TYPE_ALLOW> activeData;
        public static List<HIS_PATIENT_TYPE_ALLOW> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisPatientTypeAllowGet().GetActive();
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
            var data = new HisPatientTypeAllowGet().GetActive();
            activeData = data;
        }
    }
}
