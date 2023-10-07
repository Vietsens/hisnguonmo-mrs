using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00505
{
    public class Mrs00505RDO
    {
        public long MATERIAL_TYPE_ID { get; set; }
        public string MATERIAL_TYPE_CODE { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }

        public decimal IMP_PRICE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }

        public decimal AMOUNT_01 { get; set; }
        public decimal AMOUNT_02 { get; set; }
        public decimal AMOUNT_03 { get; set; }
        public decimal AMOUNT_04 { get; set; }
        public decimal AMOUNT_05 { get; set; }
        public decimal AMOUNT_06 { get; set; }
        public decimal AMOUNT_07 { get; set; }
        public decimal AMOUNT_08 { get; set; }
        public decimal AMOUNT_09 { get; set; }
        public decimal AMOUNT_10 { get; set; }
        public decimal AMOUNT_11 { get; set; }
        public decimal AMOUNT_12 { get; set; }
    }
}
