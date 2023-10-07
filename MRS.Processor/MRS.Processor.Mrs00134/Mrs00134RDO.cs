using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00134
{
    class Mrs00134RDO
    {
        public string DATE_EXAM { get;  set;  }
        public long TOTAL_PATIENT_EXAM_IN_DATE { get;  set;  }
        public long IS_RIGHT_ROUTE { get;  set;  }
        public long NOT_RIGHT_ROUTE { get;  set;  }
        public long PATIENT_CAREERS_PEOPLE { get;  set;  }
        public long TOTAL_TREAT { get;  set;  }
        public long TOTAL_MOVE_UP { get;  set;  }
    }

    public class Test
    {
        public string DATE_EXAM { get;  set;  }
        public V_HIS_TREATMENT TreatmentView { get;  set;  }
    }

    public class NewTreatmentView
    {
        public string DATE_EXAM { get;  set;  }
        public V_HIS_TREATMENT TreatmentView { get;  set;  }
    }
}
