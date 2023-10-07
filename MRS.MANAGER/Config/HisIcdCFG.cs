using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisIcdCFG
    {
        private const string ICD_CODE__FETUS = "MRS.HIS_ICD.ICD_CODE.FETUS";

        private static List<string> icdCodeFetus;
        public static List<string> ICD_CODE__FETUS_EXAM
        {
            get
            {
                if (icdCodeFetus == null)
                {
                    icdCodeFetus = GetListCode(ICD_CODE__FETUS);
                }
                return icdCodeFetus;
            }

        }

        private static List<string> GetListCode(string code)
        {
            List<string> result = new List<string>();
            try
            {

                HIS_CONFIG config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                var arr = value.Split(',').ToList();

                result = arr;


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Info("CODE:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code));
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                icdCodeFetus = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
