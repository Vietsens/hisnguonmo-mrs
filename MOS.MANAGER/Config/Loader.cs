using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.ConsumerManager;
using MOS.MANAGER.HisConfig;
using SDA.Filter;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class Loader : BusinessBase
    {
        public static Dictionary<string, HIS_CONFIG> dictionaryConfig = new Dictionary<string, HIS_CONFIG>();

        public static bool RefreshConfig()
        {
            bool result = false;
            try
            {
                CommonParam paramGet = new CommonParam();
                List<HIS_CONFIG> data = new HisConfigGet().Get(new HisConfigFilterQuery());
                foreach (var config in data)
                {
                    if (!String.IsNullOrWhiteSpace(config.KEY))
                    {
                        dictionaryConfig[config.KEY] = config;
                    }
                    else
                    {
                        LogSystem.Error("Key null." + LogUtil.TraceData(LogUtil.GetMemberName(() => config), config));
                    }
                }
                result = true;
                if (result)
                {
                    LogSystem.Info("Load du lieu cau hinh SdaConfig thanh cong.");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
