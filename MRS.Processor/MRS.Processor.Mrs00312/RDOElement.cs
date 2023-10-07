using FlexCel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00312
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
                string listKey = Convert.ToString(parameters[1]);
                if (string.IsNullOrWhiteSpace(listKey))
                {
                    listKey = "";
                }
                string[] arrayKey = listKey.Split(',');
                if (parameters[0] is Dictionary<string, int>)
                {
                    Dictionary<string, int> DicGet = parameters[0] as Dictionary<string, int>;
                    result = DicGet.Where(o => arrayKey.Contains(o.Key)).Sum(p => p.Value);
                }
                else if (parameters[0] is Dictionary<string, long>)
                {
                    Dictionary<string, long> DicGet = parameters[0] as Dictionary<string, long>;
                    result = DicGet.Where(o => arrayKey.Contains(o.Key)).Sum(p => p.Value);
                }
                else if (parameters[0] is Dictionary<string, decimal>)
                {
                    Dictionary<string, decimal> DicGet = parameters[0] as Dictionary<string, decimal>;
                    result = DicGet.Where(o => arrayKey.Contains(o.Key)).Sum(p => p.Value);
                }
                else if (parameters[0] is Dictionary<string, string>)
                {
                    Dictionary<string, string> DicGet = parameters[0] as Dictionary<string, string>;
                    result = string.Join(";", DicGet.Where(o => arrayKey.Contains(o.Key)).Select(p => p.Value).ToList());
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
