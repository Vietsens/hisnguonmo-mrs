using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisGender;
using MOS.MANAGER.HisPatientType;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisGenderCFG
    {
        private static List<HIS_GENDER> data;
        public static List<HIS_GENDER> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisGenderGet().Get(new HisGenderFilterQuery());
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
            var tmp = new HisGenderGet().Get(new HisGenderFilterQuery());
            data = tmp;
        }
    }
}