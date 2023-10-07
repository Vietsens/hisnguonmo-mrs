using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisMaterialTypeCFG
    {
        private static List<HIS_MATERIAL_TYPE> data;
        public static List<HIS_MATERIAL_TYPE> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisMaterialTypeGet().Get(new HisMaterialTypeFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        public static bool IsStent(long serviceId)
        {
            bool result = false;
            if (HisMaterialTypeCFG.DATA != null && HisMaterialTypeCFG.DATA.Count > 0)
            {
                result = HisMaterialTypeCFG.DATA.Exists(o => o.SERVICE_ID == serviceId && o.IS_STENT == ManagerConstant.IS_TRUE);
            }
            return result;
        }

        public static void Reload()
        {
            var tmp = new HisMaterialTypeGet().Get(new HisMaterialTypeFilterQuery());
            data = tmp;
        }
    }
}
