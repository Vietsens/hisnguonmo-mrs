using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00327
{
    public class Mrs00327RDO:IMP_EXP_MEST_TYPE
    {
        // Báo cáo nhập xuất tồn toàn viện
        public int SERVICE_GROUP_ID { get; set; }
        public string SERVICE_GROUP_NAME { get; set; }

        public long PARENT_ID { get; set; }
        public string PARENT_CODE { get; set; }
        public string PARENT_NAME { get; set; }

        public long SERVICE_TYPE_ID { get; set; }
        public string IS_STENT { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public decimal IMP_PRICE { get; set; }

        public decimal BEGIN_AMOUNT { get; set; }           // tồn đầu
        public decimal END_AMOUNT { get; set; }             // tồn cuối

        public decimal IMP_MEST { get; set; }               // tổng nhập
        public decimal IMP_MEST_CHMS_BUSI { get; set; }     // nhập chuyển kho từ kho kinh doanh

        public decimal EXP_MEST { get; set; }               // tổng xuất
        public decimal EXP_MEST_CHMS_BUSI { get; set; }     // xuất chuyển kho cho kho kinh doanh

        #region Key viện tim
        public decimal IMP_AMOUNT_NCC { get; set; }
        public decimal IMP_AMOUNT_OTHER { get; set; }
        public decimal EXP_AMOUNT_NGT { get; set; }
        public decimal EXP_AMOUNT_NT { get; set; }
        public decimal EXP_AMOUNT_CAB { get; set; }
        public decimal EXP_AMOUNT_OTHER { get; set; }
        public decimal EXP_AMOUNT_BUSI { get; set; }
        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string CONCENTRA { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public string TUTORIAL { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public string DOSAGE_FORM { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }
        #endregion
        public Mrs00327RDO() { }

        public long MEDI_STOCK_ID { get; set; }
        public Dictionary<string,decimal> DIC_BEGIN { get; set; }
    }
}
