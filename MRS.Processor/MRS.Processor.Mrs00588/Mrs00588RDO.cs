using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00588
{
    class Mrs00588RDO
    {
        public string NAME { get; set; }
        public long ID { get; set; }
        public decimal COUNT_EXAM { get; set; }	
        public decimal COUNT_EXAM_FEMALE { get; set; }
        public decimal COUNT_EXAM_BHYT { get; set; }	
        public decimal COUNT_EXAM_VP { get; set; }
        public decimal COUNT_EXAM_LESS15_BHYT { get; set; }	
        public decimal COUNT_EXAM_LESS15 { get; set; }
        public decimal COUNT_EXAM_OVER60_BHYT { get; set; }
        public decimal COUNT_EXAM_OVER60 { get; set; }
        public decimal COUNT_EXAM_TRAN { get; set; }	
        public decimal COUNT_IN { get; set; }	
        public decimal COUNT_IN_FEMALE { get; set; }
        public decimal COUNT_IN_BHYT { get; set; }	
        public decimal COUNT_IN_VP { get; set; }	
        public decimal COUNT_IN_LESS15 { get; set; }
        public decimal COUNT_IN_OVER60 { get; set; }
        public decimal COUNT_IN_TRAN { get; set; }	
        public decimal TOTAL_DATE { get; set; }
        public decimal TOTAL_DATE_BH { get; set; }	
        public decimal COUNT_OUT { get; set; }	
        public decimal COUNT_OUT_BHYT { get; set; }
        public decimal COUNT_OUT_LESS15 { get; set; }		
        public decimal COUNT_OUT_OVER60 { get; set; }	
        public decimal COUNT_DEATH { get; set; }	
        public decimal COUNT_DEATH_LESS1 { get; set; }	
        public decimal COUNT_DEATH_LESS1_FEMALE { get; set; }	
        public decimal COUNT_DEATH_LESS1_ETHANIC { get; set; }	
        public decimal COUNT_DEATH_LESS5 { get; set; }	
        public decimal COUNT_DEATH_LESS5_FEMALE { get; set; }	
        public decimal COUNT_DEATH_LESS5_ETHANIC { get; set; }


        public decimal TOTAL_OUT_DATE_BH { get; set; }

        public decimal TOTAL_OUT_DATE { get; set; }

        public decimal COUNT_IN_IMP_FEMALE { get; set; }

        public decimal COUNT_IN_IMP_BHYT { get; set; }

        public decimal COUNT_IN_IMP_VP { get; set; }

        public decimal COUNT_IN_IMP_LESS15_BHYT { get; set; }

        public decimal COUNT_IN_IMP_LESS15 { get; set; }

        public decimal COUNT_IN_IMP_OVER60_BHYT { get; set; }

        public decimal COUNT_IN_IMP_OVER60 { get; set; }

        public decimal COUNT_IN_IMP_TRAN { get; set; }

        public decimal COUNT_IN_IMP { get; set; }

        public decimal COUNT_OUT_IMP { get; set; }

        public decimal COUNT_OUT_IMP_BHYT { get; set; }

        public decimal COUNT_OUT_IMP_LESS15 { get; set; }

        public decimal COUNT_OUT_IMP_OVER60 { get; set; }

//các thông tin ?i?u tr? ban ngày
        public int COUNT_LIGHT { get; set; }
        public int COUNT_LIGHT_FEMALE { get; set; }
        public int COUNT_LIGHT_BHYT { get; set; }
        public int COUNT_LIGHT_VP { get; set; }
        public int COUNT_LIGHT__LESS15 { get; set; }
        public int COUNT_LIGHT_TRAN { get; set; }
        public int COUNT_LIGHT_OVER60 { get; set; }
        public int COUNT_LIGHT_IMP { get; set; }
        public int COUNT_LIGHT_IMP_FEMALE { get; set; }
        public int COUNT_LIGHT_IMP_BHYT { get; set; }
        public int COUNT_LIGHT_IMP_VP { get; set; }
        public int COUNT_LIGHT_IMP_LESS15 { get; set; }
        public int COUNT_LIGHT_IMP_LESS15_BHYT { get; set; }
        public int COUNT_LIGHT_IMP_OVER60 { get; set; }
        public int COUNT_LIGHT_IMP_TRAN { get; set; }
        public int COUNT_LIGHT_IMP_OVER60_BHYT { get; set; }
        public decimal TOTAL_LIGHT_DATE { get; set; }
        public decimal TOTAL_LIGHT_DATE_BH { get; set; }
        public int COUNT_LIGHT_LESS15 { get;  set; }

        public long? REALITY_PATIENT_COUNT { get; set; }
        public long? THEORY_PATIENT_COUNT { get; set; }

        public int COUNT_EXAM_YHCT { get; set; }

        public int COUNT_IN_YHCT { get; set; }

        //số lượng các cận lâm sàng
        public decimal AMOUNT_TEST { get; set; }
        public decimal AMOUNT_XQUANG { get; set; }
        public decimal AMOUNT_SIEUAM { get; set; }
        public decimal AMOUNT_CT { get; set; }
        public decimal AMOUNT_MRI { get; set; }

        public Dictionary<string, decimal> DIC_CATE_AMOUNT { get; set; }
        public string CATEGORY_CODE { get; set; }
        public decimal AMOUNT { get; set; }
    }
}
