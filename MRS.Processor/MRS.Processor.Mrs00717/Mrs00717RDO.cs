using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00717
{
    class Mrs00717RDO
    {
       public int COUNT_PATIENT_CODE_FETUS{get;set;}
       public int COUNT_PATIENT_CODE_FETUS_LESS19 { get; set; }
       public int COUNT_TREATMENT_CODE_FETUS { get; set; }
       public int COUNT_TREATMENT_CODE_FETUS_ULTRASOUND { get; set; }
       public int COUNT_TREATMENT_CODE_FETUS_TEST_URINE { get; set; }
       public int COUNT_TREATMENT_CODE_TEST_FETUS_BLOOD { get; set; }
       public int COUNT_TREATMENT_CODE_FETUS_TEST_HIV { get; set; }
       public int COUNT_TREATMENT_CODE_EXAM_OBSTETRIC { get; set; }
       public int COUNT_TREATMENT_CODE_OBSTETRIC_ABORTION { get; set; }
    }
}
