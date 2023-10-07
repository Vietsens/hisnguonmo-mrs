using FlexCel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00627
{
    public class RDOElement : TFlexCelUserFunction
    {

        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 2)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");


            try
            {
                string KeyGet = Convert.ToString(parameters[1]);
                if (parameters[0] is Dictionary<string, int>)
                {
                    Dictionary<string, int> DicGet = parameters[0] as Dictionary<string, int>;
                    if (KeyGet == "\"\"") return DicGet.Values.Sum();
                    if (!DicGet.ContainsKey(KeyGet))
                    {
                        return DicGet.Where(o => o.Key.Contains(KeyGet)).Select(p => p.Value).Sum();
                        Inventec.Common.Logging.LogSystem.Info("Not exists key: " + KeyGet);
                    }
                    result = DicGet[KeyGet];
                }
                else if (parameters[0] is Dictionary<string, long>)
                {
                    Dictionary<string, long> DicGet = parameters[0] as Dictionary<string, long>;
                    if (KeyGet == "\"\"") return DicGet.Values.Sum();
                    if (!DicGet.ContainsKey(KeyGet))
                    {
                        return DicGet.Where(o => o.Key.Contains(KeyGet)).Select(p => p.Value).Sum();
                        Inventec.Common.Logging.LogSystem.Info("Not exists key: " + KeyGet);
                    }
                    result = DicGet[KeyGet];
                }
                else if (parameters[0] is Dictionary<string, decimal>)
                {
                    Dictionary<string, decimal> DicGet = parameters[0] as Dictionary<string, decimal>;
                    if (KeyGet == "\"\"") return DicGet.Values.Sum();
                    if (!DicGet.ContainsKey(KeyGet))
                    {
                        return DicGet.Where(o => o.Key.Contains(KeyGet)).Select(p => p.Value).Sum();
                        Inventec.Common.Logging.LogSystem.Info("Not exists key: " + KeyGet);
                    }
                    result = DicGet[KeyGet];
                }
                else if (parameters[0] is Dictionary<string, string>)
                {
                    Dictionary<string, string> DicGet = parameters[0] as Dictionary<string, string>;
                    if (KeyGet == "\"\"") return string.Join(",", DicGet.Values);
                    if (!DicGet.ContainsKey(KeyGet))
                    {
                        return string.Join(",", DicGet.Where(o => o.Key.Contains(KeyGet)).Select(p => p.Value));
                        Inventec.Common.Logging.LogSystem.Info("Not exists key: " + KeyGet);
                    }
                    result = DicGet[KeyGet];
                }
                else
                {
                    result = null;
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }

            return result;
        }
    }
}
