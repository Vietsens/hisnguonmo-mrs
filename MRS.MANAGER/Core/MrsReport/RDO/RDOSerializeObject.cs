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
    public class RDOSerializeObject : TFlexCelUserFunction
    {

        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");


            try
            {
                object objectGet = parameters[0];
                if (objectGet != null)
                {
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(objectGet);
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
