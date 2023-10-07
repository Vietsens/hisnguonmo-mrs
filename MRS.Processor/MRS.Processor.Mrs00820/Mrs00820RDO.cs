using MRS.Processor.Mrs00820;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;
using Inventec.Common.Logging;
using HIS.Common.Treatment;

namespace MRS.Proccessor.Mrs00820
{
    public class Mrs00820RDO
    {

        public string TREATMENT_CODE { get; set; }
        public long TREATMENT_ID { get; set; }
        public long IN_TIME { get; set; }
        public long  OUT_TIME { get; set; }
        public long CLINICAL_IN_TIME { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }

        public decimal COUNT_OLD { get; set; }

        public decimal COUNT_OLD_BHYT { get; set; }

        public decimal COUNT_NEW { get; set; }

        public decimal COUNT_NEW_EMERGENCY { get; set; }

        public decimal COUNT_NEW_BHYT { get; set; }

        public decimal COUNT_ALL { get; set; }

        public decimal POINT_DAY { get; set; }

        public decimal COUNT_EXP { get; set; }

        public decimal COUNT_OUT_CV { get; set; }

        public decimal COUNT_DIE { get; set; }

        public decimal COUNT_OUT_XINRAVIEN { get; set; }

        public decimal COUNT_OUT_KHAC { get; set; }

        public decimal COUNT_END { get; set; }

        public decimal COUNT_ALL_BHYT { get; set; }

        public decimal COUNT_ALL_VP { get; set; }

        public decimal COUNT_ALL_XHH { get; set; }

        public decimal COUNT_BHYT { get; set; }
        public decimal COUNT_MOVE { get; set; }
        public decimal COUNT_ALL_DIE { get; set; }
        public decimal COUNT_DIE_FEMALE { get; set; }
        public decimal COUNT_DIE_IN_24H { get; set; }
        public decimal TREATMENT_DAY_COUNT_ALl { get; set; }
        public decimal TREATMENT_DAY_COUNT_RV { get; set; }
        public decimal COUNT_NT_EMERGENCY { get; set; }
        public decimal COUNT_NT { get; set; }
        public decimal COUNT_NT_15_AGE { get; set; }
        public long REALITY_PATIENT_BED { get; set; }
        public decimal COUNT_NN { get; set; }// người bệnh nước ngoài
        public decimal COUNT_PN { get; set; }// phạm nhân
        public decimal COUNT_DAN { get; set; }// dân
        public long THEORY_PATIENT_BED { get; set; }


        public decimal COUNT_CHDP_IMP { get; set; }

        public long? ORDER { get; set; }

        public string HEAD_CARD { get; set; }

        public string PATIENT_CLASSIFY_CODE { get; set; }
        public string PATIENT_CLASSIFY_NAME { get; set; }

        public string TREATMENT_END_TYPE_CODE { get; set; }

        public string TREATMENT_RESULT_CODE { get; set; }
        public long TREATMENT_RESULT_ID { get; set; }
        public long TREATMENT_END_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public string TDL_PATIENT_NATIONAL_NAME { get; set; }
        public decimal COUNT_FROM_PKK { get; set; }
        public decimal COUNT_FROM_KCC { get; set; }

        public decimal COUNT_IN { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }
        public decimal COUNT_IN_TIME { get; set; }
        public decimal COUNT_OUT_TIME { get; set; }
    }

}

