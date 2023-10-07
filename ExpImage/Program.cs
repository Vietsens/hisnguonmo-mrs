using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ExpImage
{

    class Program
    {
        static void Main(string[] args)
        {
            string arg = @"";
            if (args != null && args.Length > 0)
            {
                arg = args[0];
            }
            string[] strings = arg.Split('|');
            string temp = "";
            string fileName = "";
            if (arg.Length > 0)
            {
                temp = strings[0];
            }
            if (arg.Length > 1)
            {
                fileName = strings[1];
            }
            string path = Path.Combine(temp, fileName + ".xlsx");
            FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
            xls.Open(path);
            System.IO.MemoryStream resultStream = new MemoryStream();
            xls.Save(resultStream);
            SelectChart(resultStream, temp, fileName);
        }
        private static void SelectChart(System.IO.MemoryStream resultStream, string temp, string fileName)
        {
            var file = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());//tạo tên file random
            var path = Path.Combine(temp, file + ".xlsx"); //gộp tên file random và đường dẫn
            var pathxls = Path.Combine(temp, fileName + ".xls"); //gộp tên file random và đường dẫn

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))//khởi tạo dữ liệu file cần xử lý
            {
                resultStream.WriteTo(fs);// gán dữ liệu cần xử lý vào file

                app.Application oExcel = GetApp();//mở app excel
                oExcel.DisplayAlerts =false;

                var missing = Type.Missing;

                oExcel.Workbooks.Open(path);//mở file cần xử lý
                Microsoft.Office.Interop.Excel.Workbook wb = oExcel.ActiveWorkbook;// chọn workbook

                GC.Collect();
                GC.WaitForPendingFinalizers();
                wb.SaveAs(pathxls, Microsoft.Office.Interop.Excel.XlFileFormat.xlAddIn8, missing, missing, missing, missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);//lưu về excel xls

                wb.Close(false, Type.Missing, Type.Missing);
                Marshal.FinalReleaseComObject(wb);

                oExcel.Quit();
                Marshal.FinalReleaseComObject(oExcel);
            }
        }

        private static Microsoft.Office.Interop.Excel.Application GetApp()
        {
            Microsoft.Office.Interop.Excel.Application result = null;
            result = new Microsoft.Office.Interop.Excel.Application();

            return result;
        }
    }
}