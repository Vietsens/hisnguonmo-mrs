using FlexCel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00040
{
    public class fSearch : TFlexCelUserFunction
    {

        short result = 0;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            
            try
            {
                string value = Convert.ToString(parameters[0]);
                for (int i = 1; i < parameters.Length; i++)
                {
                    string keyWord = Convert.ToString(parameters[i]);
                    if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(keyWord)) continue;
                    if (value.ToUpper().Contains(keyWord.ToUpper())) return 1;
                }
            }
            catch (Exception ex)
            {
                
                Inventec.Common.Logging.LogSystem.Error(ex);
                return 0;
            }

            return result;
        }
    }
}
