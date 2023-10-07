using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00282
{
    public class Mrs00282Filter
    {
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> IMP_SOURCE_IDs { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? IMP_SOURCE_ID { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public long? DOCUMENT_DATE_FROM { get; set; }
        public long? DOCUMENT_DATE_TO { get; set; }

        public List<long> MEDICINE_GROUP_IDs { get; set; }

        public bool? IS_IMP_EXP { get; set; } //true: nhap, false: xuat
    }
}
