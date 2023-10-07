using FlexCel.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.RDO
{
    public class RDODistinctListText : TFlexCelUserFunction
    {

        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 2)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function. 2 parameter is text,listtext.");


            try
            {
                string ListText = Convert.ToString(parameters[0]);
                string Delimiters = Convert.ToString(parameters[1]);
                if (!string.IsNullOrEmpty(ListText) && !string.IsNullOrEmpty(Delimiters) && Delimiters.Length == 1)
                {
                    string[] items = ListText.Split(Delimiters[0]);
                    result = string.Join(Delimiters, items.Distinct());
                }
                else
                {
                    result = ListText;
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

            return result;
        }
    }
}
