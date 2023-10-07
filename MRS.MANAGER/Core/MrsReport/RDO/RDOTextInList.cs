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
    public class RDOTextInList : TFlexCelUserFunction
    {

        object result = false;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 2)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function. 2 parameter is text,listtext.");


            try
            {
                string text = (string)parameters[0];
                string ListText = (string)parameters[1];
                if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(ListText))
                {
                   
                    result = string.Format(",{0},",ListText).Contains(string.Format(",{0},", text));
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }

            return result;
        }
    }
}
