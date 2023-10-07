using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisEmotionlessResult;

namespace MRS.MANAGER.Config
{
    public class HisEmotionlessResultCFG
    {
        private static List<MOS.EFMODEL.DataModels.HIS_EMOTIONLESS_RESULT> EmotionlessResults;
        public static List<MOS.EFMODEL.DataModels.HIS_EMOTIONLESS_RESULT> EMOTIONLESS_RESULTs
        {
            get
            {
                if (EmotionlessResults == null)
                {
                    EmotionlessResults = GetAll();
                }
                return EmotionlessResults;
            }
            set
            {
                EmotionlessResults = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_EMOTIONLESS_RESULT> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_EMOTIONLESS_RESULT> result = null;
            try
            {
                HisEmotionlessResultFilterQuery filter = new HisEmotionlessResultFilterQuery();
                result = new HisEmotionlessResultManager().Get(filter);
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
                EmotionlessResults = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
