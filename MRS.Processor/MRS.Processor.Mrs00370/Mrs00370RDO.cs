using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00370
{
    public class Mrs00370RDO
    {
        public string PATIENT_NAME { get;  set;  }
        public long AGE_FEMALE { get;  set;  }
        public long AGE_MALE { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string IS_HEIN { get;  set;  }
        public string ICD_NAME { get;  set;  }
        public string REQUEST_USERNAME { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public long SERVICE_ID { get;  set;  }
        public string CONCLUDE { get;  set;  }
        public string EXCUTE_USERNAME { get;  set;  }
        public string GROUP_DATE { get;  set;  }
        public long COUNT_IN_GROUP { get;  set;  }


        public long? NUMBER_OF_FILM { get; set; }

        public long? NUMBER_OF_FAIL_FILM { get; set; }

        public long? FILM_SIZE_ID { get; set; }

        public string FILM_SIZE_NAME { get; set; }
    }
}
