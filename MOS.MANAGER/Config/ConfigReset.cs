using Inventec.Common.Logging;
using Inventec.Core;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.Config
{
    class ConfigReset : BusinessBase
    {
        internal ConfigReset()
            : base()
        {

        }

        internal ConfigReset(CommonParam param)
            : base(param)
        {

        }

        internal bool ResetConfig()
        {
            bool result = true;
            try
            {
                result = result && Config.Loader.RefreshConfig();
                if (result)
                {
                    List<Type> cfgs = GetAllCFG();
                    if (cfgs != null && cfgs.Count > 0)
                    {
                        foreach (Type cfg in cfgs)
                        {
                            try
                            {
                                MethodInfo methodInfo = cfg.GetMethod("Reload");
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
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
                param.HasException = true;
            }
            return result;
        }

        private List<Type> GetAllCFG()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsInterface && !t.IsGenericTypeDefinition && !t.IsAbstract && t.Namespace == "MOS.MANAGER.Config" && t.FullName.EndsWith("CFG"))
                .ToList();

        }
    }
}
