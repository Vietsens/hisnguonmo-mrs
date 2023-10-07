using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00819
{
    public class Mrs00819RDO
    {
        public decimal COUNT_XN { get; set; }//Xét nghiệm
        public decimal COUNT_SA { get; set; }//siêu âm
        public decimal COUNT_XQ { set; get; }//x quang
        public decimal COUNT_DTD { get; set; }//điện tâm đồ
        public decimal COUNT_DTDGS { get; set; }//điện tâm đồ gắng sức
        public decimal COUNT_DCD { get; set; }//điện cơ đồ
        public decimal COUNT_DN { get; set; }//điện não
        public decimal COUNT_NS_TH { set; get; }// nội soi tiêu hóa
        public decimal COUNT_NS_TMH { set; get; }// nội soi tai mũi họng
        public decimal COUNT_TNT { get; set; }// thận nhân tạo
        public decimal COUNT_DHH { get; set; }// đo hô hấp
        public decimal COUNT_ABI { get; set; }
        public decimal COUNT_DLX { set; get; }// đo loãng xương
        public decimal COUNT_SA_GAN { get; set; }// siêu âm gan
        public decimal COUNT_XQ_KTS { get; set; }
        public decimal COUNT_CT_SCANNER { get; set; }
        public decimal COUNT_COVI_PRC_BHYT_CA { get; set; }
        public decimal COUNT_COVI_PRC_BHYT{ get; set; }
        public decimal COUNT_COVI_PRC_DVYC { get; set; }
        public decimal COUNT_MAU { get; set; }
        public string  COUNT_MAU_STR { get; set; }
        public decimal COUNT_HIV { get; set; }
        public decimal COUNT_GPB { get; set; }
        public decimal COUNT_SH { get; set; }
        public decimal  COUNT_VS { get; set; }
        public decimal COUNT_HH { get; set; }
        public decimal COUNT_KH_BHYT_CA { get; set; }
        public decimal COUNT_KH_BHYT { get; set; }
        public decimal COUNT_KH_DVYC { get; set; }
        public decimal COUNT_TT_0 { set; get; }
        public decimal COUNT_TT_1 { set; get; }
        public decimal COUNT_TT_2 { set; get; }
        public decimal COUNT_TT_3 { set; get; }
        public decimal COUNT_TT_DB { set; get; }
        public decimal COUNT_PT_0 { set; get; }
        public decimal COUNT_PT_1 { set; get; }
        public decimal COUNT_PT_2 { set; get; }
        public decimal COUNT_PT_3 { set; get; }
        public decimal COUNT_PT_DB { set; get; }

        public decimal COUNT_TREATMENT_BD { get; set; }
        public decimal COUNT_TREATMENT_DAY_TB_NT { get; set; }
        public decimal COUNT_TREATMENT_END { get; set; }
        public decimal COUNT_TREATMENT_IN { get; set; }
        public decimal COUNT_TREATMENT_DAY_NT { get; set; }
        public decimal COUNT_TREATMENT_BHYT_CA_NT { get; set; }
        public decimal COUNT_TREATMENT_BHYT_NT { get; set; }
        public decimal COUNT_TREATMENT_DVYC_NT { get; set; }
        public decimal COUNT_TREATMENT_DAY_NGOAITRU { get; set; }
        public decimal COUNT_TOTAL_TREATMENT_NGOAITRU { get; set; }
        public decimal COUNT_ROOM { get; set; }
        public decimal? BED_TRUST { get; set; }//giường thực kê
        public decimal? BED_PLAN { get; set; }//giường kế hoạch
        public decimal? BED_CT { get; set; }//giường chỉ tiêu
        public decimal NUM_DAY { get; set; }//số ngày
        public decimal POWER_PLAN { get; set; }//công suất KH
        public decimal POWER_TRUST { get; set; }//công suât thực
        
        public long EXECUTE_DEPARTMENT_ID { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string HEIN_CARD_NUMBER { set; get; }
        public string PATIENT_TYPE_NAME { get; set; }
    }
}
