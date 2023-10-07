using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatmentType;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisTreatmentTypeCFG
    {
        private static List<HIS_TREATMENT_TYPE> data;
        public static List<HIS_TREATMENT_TYPE> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisTreatmentTypeGet().Get(new HisTreatmentTypeFilterQuery());
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
            var tmp = new HisTreatmentTypeGet().Get(new HisTreatmentTypeFilterQuery());
            data = tmp;
        }
    }
}
