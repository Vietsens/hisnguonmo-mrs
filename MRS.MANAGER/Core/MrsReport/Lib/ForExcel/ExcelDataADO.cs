using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.Lib.ForExcel
{

    public class ExcelDataADO
    {
        public string TableName { get; set; }
        public int RowCellStart { get; set; }
        public int ColumnCellStart { get; set; }
        public int RowCount { get; set; }
        public string FieldNames { get; set; }
    }
}
