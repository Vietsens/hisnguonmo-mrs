using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00410
{
    public class Mrs00410RDO : IMP_EXP_MEST_TYPE
    {
        public long? IS_ADDICTIVE { get; set; }                         // thuốc gây nghiện
        public long? IS_NEUROLOGICAL { get; set; }                      // thuốc hướng tâm thần
        public long MEDICINE_TYPE_ID { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string CONCENTRA { get; set; }                           // nồng độ, hàm lượng
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }      // tên biệt dược
        public string SERVICE_UNIT_NAME { get; set; }

        public decimal AMOUNT_BEGIN { get; set; }                       // tồn đầu

        public decimal IMP_AMOUNT { get; set; } // all

        public decimal AMOUNT_END { get; set; }                         // tồn cuối kỳ

        public decimal EXP_AMOUNT { get; set; } // all

        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string TCY_NUM_ORDER { get; set; }
        public string PACKING_TYPE_NAME { get; set; }

        public Mrs00410RDO() { }
    }
}
