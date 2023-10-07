using FlexCel.Report;
using IcdVn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.RDO
{
    public class RDOIcdVnCode : TFlexCelUserFunction
    {
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            string result = "";
            try
            {
                string CellGet = Convert.ToString(parameters[0]);

                if (CellGet != null && CellGet != "")
                {
                    return new IcdVnIcd(CellGet).ICDVN_CODE;
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
