using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisIcd;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisCareerCFG
    {
        private static List<HIS_CAREER> data;
        public static List<HIS_CAREER> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisCareerGet().Get(new HisCareerFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        public static void Reset()
        {
            data = null;
        }

        public static void Reload()
        {
            var tmp = new HisCareerGet().Get(new HisCareerFilterQuery());
            data = tmp;
        }
    }
}
