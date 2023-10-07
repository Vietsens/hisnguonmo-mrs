using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00629
{
    class Mrs00629RDO
    {
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public CountInfo EXAM_VP { get; set; }
        public CountInfo EXAM_BH { get; set; }
        public CountInfoTreat IN_VP { get; set; }
        public CountInfoTreat IN_BH { get; set; }
        public long TOTAL_DATE { get; set; }
        public Mrs00629RDO()
        {
            EXAM_VP = new CountInfo();
            EXAM_BH = new CountInfo();
            IN_VP = new CountInfoTreat();
            IN_BH = new CountInfoTreat();
        }
       
    }
    public class CountInfo
    { 
    public int COUNT_FEMALE { get; set; } //NỮ
    public int COUNT_CHILD_LESS6 { get; set; } //TRẺ EM <=6T	
    public int COUNT_CHILD_LESS15 { get; set; } //TRẺ EM <15T	
    public int COUNT_MORE60 { get; set; } //BN>=60T
    }
    public class CountInfoTreat : CountInfo
    {
        public int COUNT_KHOI { get; set; } //KHỎI
        public int COUNT_DO { get; set; } //ĐỠ
        public int COUNT_KTD { get; set; } //KHÔNG THAY ĐỔI	
        public int COUNT_NANG { get; set; } //NẶNG HƠN
        public int COUNT_CHET { get; set; } //TỬ VONG
    }
    public class CoTreatmentDepartment
    {
        public long TREATMENT_ID { get; set; } 
        public long DEPARTMENT_ID { get; set; }
    }
}
