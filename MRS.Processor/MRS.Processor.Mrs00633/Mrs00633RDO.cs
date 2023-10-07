using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00633
{
    class Mrs00633RDO
    {
        public long DATE { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY { get; set; }
        public Dictionary<string, int> DIC_CATEGORY_COUNT { get; set; }
        public Dictionary<string, int> DIC_ICD { get; set; }
        public Dictionary<string, int> DIC_BEGIN { get; set; }
        public Dictionary<string, int> DIC_IMP { get; set; }
        public Dictionary<string, int> DIC_CHMS { get; set; }
        public Dictionary<string, int> DIC_EXP { get; set; }
        public Dictionary<string, int> DIC_EXP_END_TYPE { get; set; }

        public Mrs00633RDO()
        {
            this.DIC_CATEGORY = new Dictionary<string, decimal>();
            this.DIC_CATEGORY_COUNT = new Dictionary<string, int>();
            this.DIC_ICD = new Dictionary<string, int>();
            this.DIC_BEGIN = new Dictionary<string, int>();
            this.DIC_IMP = new Dictionary<string, int>();
            this.DIC_IMP_LESS15 = new Dictionary<string, int>();
            this.DIC_IMP_MORE15 = new Dictionary<string, int>();
            this.DIC_CHMS = new Dictionary<string, int>();
            this.DIC_EXP = new Dictionary<string, int>();
            this.DIC_EXP_END_TYPE = new Dictionary<string, int>();
            this.DIC_EXP_DIE_BEFORE_IN = new Dictionary<string, int>();
            this.DIC_EXP_DIE_AFTER_IN = new Dictionary<string, int>();
            this.DIC_IMP_MOVE_TREAT_IN = new Dictionary<string, int>();
            this.DIC_IMP_MOVE_EXAM = new Dictionary<string, int>();
            this.DIC_IMP_TRANSFER_IN = new Dictionary<string, int>();
            this.DIC_ACCIDENT_TYPE_RESULT = new Dictionary<string, int>();
            this.DIC_MISU_GROUP = new Dictionary<string, decimal>();
            this.DIC_END = new Dictionary<string, int>();
            this.DIC_EXP_RESULT_END_TYPE = new Dictionary<string, int>();
        }

        public Dictionary<string, int> DIC_IMP_LESS15 { get; set; }

        public Dictionary<string, int> DIC_IMP_MORE15 { get; set; }

        public Dictionary<string, int> DIC_EXP_CTCV { get; set; }

        public Dictionary<string, int> DIC_EXP_DIE_BEFORE_IN { get; set; }

        public Dictionary<string, int> DIC_EXP_DIE_AFTER_IN { get; set; }

        public Dictionary<string, int> DIC_IMP_MOVE_TREAT_IN { get; set; }

        public Dictionary<string, int> DIC_IMP_MOVE_EXAM { get; set; }

        public Dictionary<string, int> DIC_IMP_TRANSFER_IN { get; set; }

        public Dictionary<string, int> DIC_ACCIDENT_TYPE_RESULT { get; set; }

        public Dictionary<string, decimal> DIC_MISU_GROUP { get; set; }

        public Dictionary<string, int> DIC_END { get; set; }

        public Dictionary<string, int> DIC_EXP_RESULT_END_TYPE { get; set; }
    }
    public class DepartmentInOut
    {
        public long ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public long? DEPARTMENT_IN_TIME { get; set; }
        public long? NEXT_ID { get; set; }
        public long? NEXT_DEPARTMENT_ID { get; set; }
        public long? NEXT_DEPARTMENT_IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? OUT_DATE { get; set; }
        public string TREATMENT_ICD_CODE { get; set; }
        public string TREATMENT_ICD_NAME { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public short? IS_PAUSE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }

        public long TDL_PATIENT_DOB { get; set; }

        public long IN_TIME { get; set; }

        public long IN_DATE { get; set; }

        public long? DEATH_DOCUMENT_DATE { get; set; }

        public long? PREVIOUS_ID { get; set; }

        public string TRANSFER_IN_MEDI_ORG_CODE { get; set; }
    }
   
}
