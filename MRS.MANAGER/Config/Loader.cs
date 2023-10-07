using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisConfig;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;

namespace MRS.MANAGER.Config
{
    public class Loader : BusinessBase
    {
        public static Dictionary<string, HIS_CONFIG> dictionaryConfig = new Dictionary<string, HIS_CONFIG>();

        public static bool Refresh()
        {
            bool result = false;
            try
            {
                CommonParam paramGet = new CommonParam();
                //List<HIS_CONFIG> data = new HisConfigManager().Get(new HisConfigFilterQuery());
                List<HIS_CONFIG> data = new List<HIS_CONFIG>();

                string sql = "SELECT * FROM HIS_CONFIG WHERE IS_ACTIVE = 1";
                data = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_CONFIG>(sql);

                if (data != null && data.Count > 0)
                {
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
                    //foreach (var config in configs)
                    //{
                    //    if (!String.IsNullOrWhiteSpace(config.KEY))
                    //    {
                    //        dictionaryConfig[config.KEY] = config; //Ghi de du lieu cu ==> luu y tinh huong neu 2 config trung key thi config sau se de len config truoc. Loi nay thuoc ve constraint du lieu ko thuoc trach nhiem cua Loader.
                    //    }
                    //    else
                    //    {
                    //        LogSystem.Error("Key null." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                    //    }
                    //}
                    result = true;
                }
                else if (paramGet.HasException)
                {
                    LogSystem.Error("Query HisConfig co exception.");
                }
                else
                {
                    LogSystem.Warn("Khong co du lieu HisConfig & khong co exception.");
                    result = true;
                }
                if (result)
                {
                    LogSystem.Info("Load du lieu cau hinh HisConfig thanh cong.");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public static void ThreadRefresh()
        {
            System.Threading.Thread refresh = new System.Threading.Thread(DoRefresh);
            try
            {
                refresh.Start();
            }
            catch (Exception ex)
            {
                refresh.Abort();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private static void DoRefresh()
        {
            try
            {
                while (true)
                {
                    int repeatTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MRS.API.Scheduler.RefreshSystemConfigJob"]);
                    if (repeatTime > 0)
                    {
                        System.Threading.Thread.Sleep(repeatTime);
                        RefreshAllkey();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private static void RefreshAllkey()
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("Begin Refresh");
                if (Refresh())
                {
                    List<Type> cfgs = GetAllCFG();
                    if (cfgs != null && cfgs.Count > 0)
                    {
                        foreach (Type cfg in cfgs)
                        {
                            try
                            {
                                System.Reflection.MethodInfo methodInfo = cfg.GetMethod("Refresh");
                                if (methodInfo != null)
                                {
                                    methodInfo.Invoke(null, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogSystem.Error(ex);
                            }
                        }
                    }

                    HeinCardNumberGroups.Refresh();
                }
                Inventec.Common.Logging.LogSystem.Info("End Refresh");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private static List<Type> GetAllCFG()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsInterface
                    && !t.IsGenericTypeDefinition
                    && !t.IsAbstract
                    && t.Namespace == "MRS.MANAGER.Config"
                    && t.FullName.EndsWith("CFG"))
                .ToList();
        }
    }
}
