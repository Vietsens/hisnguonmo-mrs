using FlexCel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00630
{
    public class RDOTime : TFlexCelUserFunction
    {

        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");


            try
            {
                int time = Convert.ToInt32(parameters[0]);
                int hour = (int)(time / 3600);
                int min = (int)((time % 3600) / 60);
                int sec = (int)(((time % 3600) % 60));
                result = string.Format("{0}:{1}:{2}", hour, min, sec);
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
