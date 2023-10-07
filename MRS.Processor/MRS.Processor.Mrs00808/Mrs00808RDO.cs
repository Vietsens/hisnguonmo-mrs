using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00808
{
    public class Mrs00808RDO
    {
        public long TREATMENT_ID { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }
        public string SERVICE_PARENT_NAME { get; set; }
        public string SERVICE_PARENT_CODE { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_NAME { set; get; }
        public string SERVICE_CODE { set; get; }
        public string DEPARTMENT_NAME { set; get; }
        public long DEPARTMENT_ID { set; get; }
        public string DEPARTMENT_CODE { set; get; }
        public decimal AMOUNT { set; get; }
        public decimal PRICE { set; get; }
        public long ROOM_ID { set; get; }
        public string ROOM_CODE { set; get; }
        public string ROOM_NAME { set; get; }
        public string DOCTOR_USERNAME { set; get; }
        public string DOCTOR_LOGINNAME { set; get; }
        public long TYPE { set; get; }


        public decimal AMOUNT_HH { set; get; }// số lượng xét nghiệm huyết học
        public decimal AMOUNT_HS { set; get; }// số lượng xét nghiệm hóa sinh
        public decimal AMOUNT_VS { get; set; }
        public decimal AMOUNT_VS_PCR { set; get; }// số lượng xét nghiệm vi sinh pcr
        public decimal AMOUNT_CT { set; get; }// số lượng chụp ct
        public decimal AMOUNT_XQ { set; get; }// số lượng chụp x quang
        public decimal AMOUNT_SA_MAU { set; get; }// số lượng siêu âm màu
        public decimal AMOUNT_SA_THUONG { set; get; }// số lượng siêu âm thường
        public decimal AMOUNT_DTD { set; get; }// số lượng điện tim đồ
        public decimal AMOUNT_DND { set; get; }// số lượng điện tim đồ
        public decimal AMOUNT_GPB { set; get; }// số lượng giải phẫu bệnh
        public decimal AMOUNT_TMH { set; get; }// số lượng tai mũi họng
        public decimal AMOUNT_NS_DD { set; get; }//số lượng nội soi dạ dày
        public decimal AMOUNT_NS_DT { set; get; }//số lượng nội soi đại tràng
        public decimal AMOUNT_MRI { set; get; }// số lượng MRI
        public decimal AMOUNT_NS_POLYP { set; get; }// sos lượng nối soi polyp
        public decimal TOTAL_AMOUNT_KH { set; get; }
        public decimal AMOUNT_CV_238 { set; get; }
        public decimal AMOUNT_CV_105_100 { get; set; }
        public decimal AMOUNT_CV_210_109 { get; set; }
        public decimal AMOUNT_CV_GOP { get; set; }
        public decimal AMOUNT_CV_TAI_TRO { get; set; }
        public decimal AMOUNT_PCR { get; set; }
        public decimal AMOUNT_PCR_KHAC { get; set; }

        public decimal AMOUNT_HH_TL { set; get; }// số lượng xét nghiệm huyết học
        public decimal AMOUNT_HS_TL { set; get; }// số lượng xét nghiệm hóa sinh
        public decimal AMOUNT_VS_TL { get; set; }
        public decimal AMOUNT_VS_PCR_TL { set; get; }// số lượng xét nghiệm vi sinh pcr
        public decimal AMOUNT_CT_TL { set; get; }// số lượng chụp ct
        public decimal AMOUNT_XQ_TL { set; get; }// số lượng chụp x quang
        public decimal AMOUNT_SA_MAU_TL { set; get; }// số lượng siêu âm màu
        public decimal AMOUNT_SA_THUONG_TL { set; get; }// số lượng siêu âm thường
        public decimal AMOUNT_DTD_TL { set; get; }// số lượng điện tim đồ
        public decimal AMOUNT_DND_TL { set; get; }// số lượng điện tim đồ
        public decimal AMOUNT_GPB_TL { set; get; }// số lượng giải phẫu bệnh
        public decimal AMOUNT_TMH_TL { set; get; }// số lượng tai mũi họng
        public decimal AMOUNT_NS_DD_TL { set; get; }//số lượng nội soi dạ dày
        public decimal AMOUNT_NS_DT_TL { set; get; }//số lượng nội soi đại tràng
        public decimal AMOUNT_MRI_TL { set; get; }// số lượng MRI
        public decimal AMOUNT_NS_POLYP_TL { set; get; }// sos lượng nối soi polyp
        public decimal TOTAL_AMOUNT_KH_TL { set; get; }
        public decimal AMOUNT_CV_238_TL { set; get; }
        public decimal AMOUNT_CV_105_100_TL { get; set; }
        public decimal AMOUNT_CV_210_109_TL { get; set; }
        public decimal AMOUNT_CV_GOP_TL { get; set; }
        public decimal AMOUNT_CV_TAI_TRO_TL { get; set; }
        public decimal AMOUNT_PCR_TL { get; set; }
        public decimal AMOUNT_PCR_KHAC_TL { get; set; }


        public Dictionary<string, decimal> DIC_SV_CD_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_SV_TL_AMOUNT { get; set; }


        public Dictionary<string, decimal> DIC_PAR_CD_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TL_AMOUNT { get; set; }

        public string CATEGORY_NAME { get; set; }

        public string CATEGORY_CODE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_CD_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TL_AMOUNT { get; set; }

        public string STR_SV_CD_AMOUNT { get; set; }

        public string STR_SV_TL_AMOUNT { get; set; }

        public string STR_PAR_CD_AMOUNT { get; set; }

        public string STR_PAR_TL_AMOUNT { get; set; }

        public string STR_CATE_CD_AMOUNT { get; set; }

        public string STR_CATE_TL_AMOUNT { get; set; }
    }
}
