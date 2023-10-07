using FlexCel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.RDO
{
    public class RDOCustomerFuncMergeSameData : TFlexCelUserFunction
    {
        string CellValue;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                string CellGet = Convert.ToString(parameters[0]);

                if (CellGet != null && CellGet != "")
                {
                    if (CellValue == CellGet)
                    {
                        return true;
                    }
                    else
                    {
                        CellValue = CellGet;
                        return false;
                    }
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
