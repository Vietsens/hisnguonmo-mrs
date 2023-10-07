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
    public class RDOTimeSpan : TFlexCelUserFunction
    {

        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 2)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function. 2 parameter is inTime,outTime");


            try
            {
                
               if (parameters[0] != null && parameters[1] != null)
                {
                    System.DateTime? dateBefore = System.DateTime.ParseExact(parameters[0].ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    System.DateTime? dateAfter = System.DateTime.ParseExact(parameters[1].ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    if (dateBefore != null && dateAfter != null)
                    {
                        TimeSpan difference = dateAfter.Value - dateBefore.Value;
                        result = (double)difference.TotalDays;
                    }
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

        private  System.DateTime? TimeNumberToSystemDateTime(long time)
        {
            System.DateTime? result = null;
            try
            {
                if (time > 0)
                {
                    result = System.DateTime.ParseExact(time.ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
