using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00236
{
    public class Mrs00236RDO
    {
        public V_HIS_IMP_MEST_MEDICINE V_HIS_IMP_MEST_MEDICINE { get; set; }
        public V_HIS_IMP_MEST_MATERIAL V_HIS_IMP_MEST_MATERIAL { get; set; }
        public long REPORT_TYPE_CAT_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public long MEDICINE_TYPE_ID { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string UNIT { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public Decimal IMP_PRICE { get; set; }
        public Decimal IMP_VAT_RATIO { get; set; }
        public Decimal TOTAL_IMP_VAT { get; set; }
        public Decimal AMOUNT { get; set; }
        public Decimal TOTAL_PRICE { get; set; }
        public string IMP_TIME { get; set; }
        public string DOCUMENT_DATE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string BID_NUMBER { get; set; }
        public long TYPE { get; set; }
        public string MEDI_MATE_PARENT_CODE { get; set; }
        public string MEDI_MATE_PARENT_NAME { get; set; }
    }
}
