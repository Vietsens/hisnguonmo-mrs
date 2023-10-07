using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00496
{
    public class Mrs00496Filter
    {
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }
        public bool? IS_BLOOD { get; set; }
        public long MEDI_STOCK_ID { get; set; }
        public long? TIME { get; set; }
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public bool? IS_MERGER_EXPIRED_DATE { get; set; }
        public bool? IS_MERGER_PRICE { get; set; }
        public bool? IS_MERGER_PACKAGE_NUMBER { get; set; }
        public bool? IS_MERGER_BID_NUMBER { get; set; }

        public bool? IS_DETAIL_MEDICINE { get; set; }//tach thuoc nho nhat

        public List<RoleUserADO> EXECUTE_ROLE_GROUP { get; set; }
        public short? INPUT_DATA_ID_PRICE_TYPE { get; set; }//1. giá xuất trước VAT, 2. giá xuất sau VAT, 3. giá nhập trước VAT, 4. giá nhập sau VAT
        public long? PRICE_PATIENT_TYPE_ID { get; set; }// đối tượng tính giá xuất
        public string REASON_CODE__HV { get; set; }
    }
}
