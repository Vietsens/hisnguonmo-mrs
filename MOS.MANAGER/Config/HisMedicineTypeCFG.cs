using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisMedicineTypeCFG
    {
        private static List<HIS_MEDICINE_TYPE> data;
        public static List<HIS_MEDICINE_TYPE> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisMedicineTypeGet().Get(new HisMedicineTypeFilterQuery());
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
            var tmp = new HisMedicineTypeGet().Get(new HisMedicineTypeFilterQuery());
            data = tmp;
        }
    }
}
