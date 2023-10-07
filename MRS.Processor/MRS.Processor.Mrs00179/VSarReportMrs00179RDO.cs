using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00179
{
    class VSarReportMrs00179RDO : IMP_EXP_MEST_TYPE
    {
        public long MEDI_STOCK_ID { get; set; }
        public long REPORT_TYPE_CAT_ID { get; set; }
        public string CATEGORY_NAME { get; set; }

        public int SERVICE_TYPE_ID { get; set; }

        public long MEDI_MATE_ID { get; set; }
        public long SERVICE_ID { get; set; }

        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public decimal BEGIN_AMOUNT { get; set; }
        public decimal END_AMOUNT { get; set; }
        public decimal IMP_PRICE { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }

        public long? MEDICINE_LINE_ID { get; set; }
        public string MEDICINE_LINE_CODE { get; set; }
        public string MEDICINE_LINE_NAME { get; set; }

        public long? MEDICINE_GROUP_ID { get; set; }
        public string MEDICINE_GROUP_CODE { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }

        public long? PARENT_MEDICINE_TYPE_ID { get; set; }
        public string PARENT_MEDICINE_TYPE_CODE { get; set; }
        public string PARENT_MEDICINE_TYPE_NAME { get; set; }

        public long? NUM_ORDER { get; set; }

        public string CONCENTRA { get; set; }
        public string EXPIRED_DATE_STR { get; set; }
        public string ACTIVE_INGR_BHYT_CODE { get; set; }//Mã Hoạt chất
        public string ACTIVE_INGR_BHYT_NAME { get; set; }//Tên hoạt chất
        public string REGISTER_NUMBER { get; set; }//Số đăng ký
        public string PACKAGE_NUMBER { get; set; }//Số lô
        public string BID_GROUP_CODE { get; set; }//Mã nhóm thầu
        public string BID_NUM_ORDER { get; set; }//Số thứ tự thầu

        public Dictionary<string, decimal> DIC_REASON { get; set; }
        public VSarReportMrs00179RDO()
        {
            DIC_REASON = new Dictionary<string, decimal>();
        }

        public string BID_PACKAGE_CODE { get; set; }

        public string MEDICINE_REGISTER_NUMBER { get; set; }
    }
    
}
