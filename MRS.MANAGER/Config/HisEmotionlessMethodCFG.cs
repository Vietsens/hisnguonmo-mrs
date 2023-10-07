using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisEmotionlessMethod;

namespace MRS.MANAGER.Config
{
    public class HisEmotionlessMethodCFG
    {
        private static List<MOS.EFMODEL.DataModels.HIS_EMOTIONLESS_METHOD> EmotionlessMethods;
        public static List<MOS.EFMODEL.DataModels.HIS_EMOTIONLESS_METHOD> EMOTIONLESS_METHODs
        {
            get
            {
                if (EmotionlessMethods == null)
                {
                    EmotionlessMethods = GetAll();
                }
                return EmotionlessMethods;
            }
            set
            {
                EmotionlessMethods = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_EMOTIONLESS_METHOD> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_EMOTIONLESS_METHOD> result = null;
            try
            {
                HisEmotionlessMethodFilterQuery filter = new HisEmotionlessMethodFilterQuery();
                result = new HisEmotionlessMethodManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                EmotionlessMethods = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
