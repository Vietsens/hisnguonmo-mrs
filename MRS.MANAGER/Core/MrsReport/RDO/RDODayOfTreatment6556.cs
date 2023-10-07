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
    public class RDODayOfTreatment6556 : TFlexCelUserFunction
    {

        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 4)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function. 4 parameter is inTime,clinicalInTime,outTime,treatmentTypeId.");


            try
            {
                long inTime = 0;
                long? clinicalInTime = null;
                long? outTime = null;
                long treatmentTypeId = 0;
                if (parameters[0] != null && parameters[0] is long &&parameters[2] != null && parameters[2] is long)
                {
                    inTime = (long)parameters[0];
                    outTime = (long)parameters[2];
                    if (parameters[1] != null && parameters[1] is long)
                    {
                        clinicalInTime = (long)parameters[1];
                    }
                    if (parameters[3] != null && parameters[3] is long)
                    {
                        treatmentTypeId = (long)parameters[3];
                    }
                    result = HIS.Common.Treatment.Calculation.DayOfTreatment6556(inTime,clinicalInTime,outTime,treatmentTypeId);
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
