using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using SDA.MANAGER.Core.SdaConfig;
using SDA.MANAGER.Core.SdaConfig.Get;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Config
{
    public class Loader : BusinessBase
    {
        public static Dictionary<string, SDA_CONFIG> dictionaryConfig = new Dictionary<string, SDA_CONFIG>();
        public static bool Refresh()
        {
            bool result = false;
            try
            {
                CommonParam paramGet = new CommonParam();
                //Cac backend khac thi se goi thong qua API
                //Nho su dung param de kiem tra exception
                List<SDA_CONFIG> listConfig = new SdaConfigBO().Get<List<SDA_CONFIG>>(new SdaConfigFilterQuery());
                if (listConfig != null && listConfig.Count > 0)
                {
                    foreach (var config in listConfig)
                    {
                        if (!String.IsNullOrWhiteSpace(config.KEY))
                        {
                            dictionaryConfig[config.KEY] = config; //Ghi de du lieu cu ==> luu y tinh huong neu 2 config trung key thi config sau se de len config truoc. Loi nay thuoc ve constraint du lieu ko thuoc trach nhiem cua Loader.
                        }
                        else
                        {
                            LogSystem.Error("Key null." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                        }
                    }
                    result = true;
                }
                else if (paramGet.HasException)
                {
                    LogSystem.Error("Query sdaconfig co exception.");
                }
                else
                {
                    LogSystem.Warn("Khong co du lieu sdaconfig & khong co exception.");
                    result = true;
                }
                if (result)
                {
                    LogSystem.Info("Load du lieu cau hinh sdaconfig thanh cong.");
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
