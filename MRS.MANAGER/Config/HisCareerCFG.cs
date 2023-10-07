using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisCareer;

namespace MRS.MANAGER.Config
{
    public class HisCareerCFG
    {
        private const string CAREER_CODE__RETIRED_MILITARY = "MRS.HIS_CAREER.CAREER_CODE.RETIRED_MILITARY";

        private static long careerIdRetiredMilitary;
        public static long CAREER_ID__RETIRED_MILITARY
        {
            get
            {
                if (careerIdRetiredMilitary == 0)
                {
                    careerIdRetiredMilitary = GetId(CAREER_CODE__RETIRED_MILITARY);
                }
                return careerIdRetiredMilitary;
            }
            set
            {
                careerIdRetiredMilitary = value;
            }
        }

        private static long GetId(string code)
        {
            long result = -1;//de chi thuc hien load 1 lan
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                var data = new HisCareerManager().GetByCode(value);
                if (data == null) throw new ArgumentNullException(code);
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                careerIdRetiredMilitary = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
