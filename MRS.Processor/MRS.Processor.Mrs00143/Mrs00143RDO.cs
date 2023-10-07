using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00143
{
    class Mrs00143RDO
    {
        public string NAME_FILTER { get;  set;  }
        public long? TOTAL_EXAM { get;  set;  }
        public long? TOTAL_EXAM_CHILDREN { get;  set;  }
        public long? TOTAL_PATIENT_KIND_POOR_EXAM { get;  set;  }
        public long? TOTAL_PATIENT_POOR_ACCESS_EXAM { get;  set;  }
        public long? TOTAL_SERVICE_YHCT { get;  set;  }
        public long? TOTAL_PATIENT_OUTPATIENT_TREATMENT_EXAM { get;  set;  }
        public long? TOTAL_PATIENT_INPATIENT_TREATMENT_TREAT { get;  set;  }
        public long? TOTAL_PATIENT_INPATIENT_TREATMENT_KIND_POOR { get;  set;  }
        public long? TOTAL_PATIENT_INPATIENT_TREATMENT_ACCESS_EXAM { get;  set;  }
        public decimal? TIME_AVERAGE_TREATMENT { get;  set;  }
        public long? TOTAL_TIME_AVERAGE_INPATIENT_TREATMENT { get;  set;  }
        public decimal? RATIO_TRANSFER_HOSPITAL { get;  set;  }
        public long? TOTAL_Treatment_PATIENT { get;  set;  }
        public long? TOTAL_DEATH { get;  set;  }
        public long? TOTAL_PATIENT_SURGERY { get;  set;  }
        public long? TOTAL_PATIENT_ENDOSCOPIC { get;  set;  }
        public long? TOTAL_SERVICE_XQ { get;  set;  }
        public long? TOTAL_SERVICE_CT { get;  set;  }
        public long? TOTAL_SERVICE_SUIM { get;  set;  }
        public long? TOTAL_SERVICE_DT { get;  set;  }
        public long? TOTAL_SERVICE_TEST { get;  set;  }
        public long? TOTAL_EMERGENCY_PATIENT { get;  set;  }
    }
}
