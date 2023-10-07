using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using Microsoft.Office.Interop.Excel;
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
    public class RDOSelectSheetChart : TFlexCelUserFunction
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
                string AppPath = HttpRuntime.AppDomainAppPath;// đường dẫn tiến trình xuất ảnh
                var temp = Path.GetTempPath(); // đường dẫn thư mục lưu file
                var file = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());//tạo tên file random
                var path = Path.Combine(temp, file + ".xlsx"); //gộp tên file random và đường dẫn
                var pathxls = Path.Combine(temp, file + ".xls"); //gộp tên file random và đường dẫn
                xls.Save(path);//lưu file excel vào thư mục
                string args = temp + "|" + file;//tạo param để chạy tiến trình xuất ảnh
                var process = Process.Start(string.Format(@"{0}\bin\ExpImage.exe", AppPath), args);//chạy tiến trình xuất ảnh
                process.WaitForExit();//đợi đến khi kết thúc tiến trình

                FlexCel.XlsAdapter.XlsFile fr = new FlexCel.XlsAdapter.XlsFile(true);
                fr.Open(pathxls);
                fr.Save(resultStream);
               
                foreach (Process clsProcess in Process.GetProcesses())
                    if (clsProcess.ProcessName.Equals("EXCEL"))  //Process Excel?
                        clsProcess.Kill();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
