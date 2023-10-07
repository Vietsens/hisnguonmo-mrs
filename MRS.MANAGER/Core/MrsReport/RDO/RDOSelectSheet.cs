using FlexCel.Core;
using FlexCel.Report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MRS.MANAGER.Core.MrsReport.RDO
{
    public class RDOSelectSheet : TFlexCelUserFunction
    {
       
        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function. 1 parameter is sheetName");

            try
            {
               if (parameters[0] != null)
                {
                    result = parameters[0];
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

            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SelectSheet;
            return result;
        }

        private void SelectSheet(ref Inventec.Common.FlexCellExport.Store store, ref System.IO.MemoryStream resultStream)
        {
            try
            {

                resultStream.Position = 0;
                FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
                xls.Open(resultStream);

                xls.ActiveSheetByName = result.ToString();
                xls.Save(resultStream);
                resultStream.Position = 0;
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
