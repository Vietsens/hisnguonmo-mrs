using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00564
{
    public class Mrs00564RDO
    {

        public string TYPE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }

        public Dictionary<string,decimal> DIC_GROUP_AMOUNT { get; set; }

        public Mrs00564RDO()
        {
            DIC_GROUP_AMOUNT = new Dictionary<string, decimal>();
        }

        public long SERVICE_ID { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_CODE { set; get; }
        public string SERVICE_PARENT_NAME { get; set; }
        public string SERVICE_PARENT_CODE { set; get; }
        public long SERVICE_TYPE_ID { set; get; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string TREATMENT_TYPE_NAME { set; get; }
        public long TREATMENT_TYPE_ID { set; get; }
        public string TREATMENT_CODE{set;get;}
        public decimal AMOUNT { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
        public decimal AMOUNT_XN { get; set; }
        public decimal AMOUNT_HH { set; get; }
        public decimal AMOUNT_VS { get; set; }
        public decimal AMOUNT_SH { get; set; }
        public decimal AMOUNT_GPB { get; set; }
        public decimal AMOUNT_HIV { get; set; }
        public decimal AMOUNT_XQ { get; set; }
        public decimal AMOUNT_XQ_KTS { get; set; }
        public decimal AMOUNT_CT_Scanner { get; set; }
        public decimal AMOUNT_MAU { get; set; }
        public decimal AMOUNT_SA { get; set; }
        public decimal AMOUNT_NS { get; set; }
        public decimal AMOUNT_DT { get; set; }
        public decimal AMOUNT_SA_DHM { get; set; }
        public decimal AMOUNT_DLX { get; set; }// đo loãng xương
        public decimal AMOUNT_THAN_NT { get; set; }// thận nhân tạo
        public long? BLOOD_ID { get; set; }
    }
    public class CountService
    {
        public Dictionary<string,decimal> DIC_SERVICE_AMOUNT_NT { get; set; }
        public Dictionary<string,decimal> DIC_SERVICE_TYPE_AMOUNT_NT { get; set; }
        public Dictionary<string,decimal> DIC_PARENT_SERVICE_AMOUNT_NT { get; set; }

        public Dictionary<string, decimal> DIC_SERVICE_AMOUNT_NGTRU { get; set; }
        public Dictionary<string, decimal> DIC_SERVICE_TYPE_AMOUNT_NGTRU { get; set; }
        public Dictionary<string, decimal> DIC_PARENT_SERVICE_AMOUNT_NGTRU { get; set; }


        public long ID { get; set; }
        public decimal AMOUNT_XN { get; set; }
        public decimal AMOUNT_HH { set; get; }
        public decimal AMOUNT_VS { get; set; }
        public decimal AMOUNT_SH { get; set; }
        public decimal AMOUNT_GPB { get; set; }
        public decimal AMOUNT_HIV { get; set; }
        public decimal AMOUNT_XQ { get; set; }
        public decimal AMOUNT_XQ_KTS { get; set; }
        public decimal AMOUNT_CT_Scanner { get; set; }
        public decimal AMOUNT_MAU { get; set; }
        public decimal AMOUNT_SA { get; set; }
        public decimal AMOUNT_NS { get; set; }
        public decimal AMOUNT_DT { get; set; }
        public decimal AMOUNT_SA_DHM { get; set; }
        public decimal AMOUNT_DLX { get; set; }// đo loãng xương
        public decimal AMOUNT_THAN_NT { get; set; }// thận nhân tạo
        public decimal AMOUNT_XNNV { get; set; }// xét nghiệm ngoại viện
        public decimal AMOUNT_XNNT { get; set; }// xét nghiệm nước tiểu
        //ngoại trú
        public decimal TOTAL_AMOUNT { get; set; }
        public decimal AMOUNT_XN_NT { get; set; }
        public decimal AMOUNT_HH_NT { set; get; }
        public decimal AMOUNT_VS_NT { get; set; }
        public decimal AMOUNT_SH_NT { get; set; }
        public decimal AMOUNT_GPB_NT { get; set; }
        public decimal AMOUNT_HIV_NT { get; set; }
        public decimal AMOUNT_XQ_NT { get; set; }
        public decimal AMOUNT_XQ_KTS_NT { get; set; }
        public decimal AMOUNT_CT_Scanner_NT { get; set; }
        public decimal AMOUNT_MAU_NT { get; set; }
        public decimal AMOUNT_SA_NT { get; set; }
        public decimal AMOUNT_NS_NT { get; set; }
        public decimal AMOUNT_DT_NT { get; set; }
        public decimal AMOUNT_SA_DHM_NT { get; set; }
        public decimal AMOUNT_DLX_NT { get; set; }// đo loãng xương
        public decimal AMOUNT_THAN_NT_NT { get; set; }// thận nhân tạo
        public decimal AMOUNT_XNNV_NT { get; set; }// xét nghiệm ngoại viện
        public decimal AMOUNT_XNNT_NT { get; set; }// xét nghiệm nước tiểu
    }
}
