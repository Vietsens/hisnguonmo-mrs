using FlexCel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00642
{
    public class RDOSumKeys : TFlexCelUserFunction
    {

        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 2)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            
            try
            {
                List<string> KeyGet = Convert.ToString(parameters[1]).Split(',').ToList();
                if (parameters[0] is Dictionary<string, int>)
                {
                    Dictionary<string, int> DicGet = parameters[0] as Dictionary<string, int>;
                    int value = 0;
                    foreach (string item in KeyGet)
                    {
                        if (DicGet.ContainsKey(item))
                        {
                            value += DicGet[item];
                        }
                    }
                    result = value;
                }
                else if (parameters[0] is Dictionary<string, long>)
                {
                    Dictionary<string, long> DicGet = parameters[0] as Dictionary<string, long>;

                    long value = 0;
                    foreach (string item in KeyGet)
                    {
                        if (DicGet.ContainsKey(item))
                        {
                            value += DicGet[item];
                        }
                    }
                    result = value;
                }
                else if (parameters[0] is Dictionary<string, decimal>)
                {
                    Dictionary<string, decimal> DicGet = parameters[0] as Dictionary<string, decimal>;
                    decimal value = 0;
                    foreach (string item in KeyGet)
                    {
                        if (DicGet.ContainsKey(item))
                        {
                            value += DicGet[item];
                        }
                    }
                    result = value;
                }
                else if (parameters[0] is Dictionary<string, string>)
                {
                    Dictionary<string, string> DicGet = parameters[0] as Dictionary<string, string>;

                    string value = "";
                    foreach (string item in KeyGet)
                    {
                        if (DicGet.ContainsKey(item))
                        {
                            value += ", "+DicGet[item];
                        }
                    }
                    result = value;
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
