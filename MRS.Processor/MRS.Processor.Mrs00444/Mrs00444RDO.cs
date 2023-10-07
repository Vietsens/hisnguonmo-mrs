using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00444
{
    public class Mrs00444RDO: V_HIS_SERE_SERV
    {

				 public string EXECUTE_DEPARTMENT_NAME { get;  set;  }
				 public string EXECUTE_ROOM_NAME { get;  set;  }
				 public string REQUEST_DEPARTMENT_NAME { get;  set;  }
				 public string REQUEST_ROOM_NAME { get;  set;  }
				 public long COUNT_TOTAL { get;  set;  }
                 public long COUNT_IN { get;  set;  }
                 public long COUNT_TT { get;  set;  }
                 public long COUNT_BHYT { get;  set;  }
                 public long COUNT_TE { get;  set;  }
                 public long COUNT_DV { get;  set;  }
                 public long COUNT_IN_1 { get;  set;  }
                 public long COUNT_BHYT_1 { get;  set;  }
                 public long COUNT_TE_1 { get;  set;  }
                 public long COUNT_DV_1 { get;  set;  }
                 public long COUNT_NGT { get;  set;  }
				 public long COUNT_EXAM_TRAN_PATI { get;  set;  }
				 public long COUNT_TREAT_TRAN_PATI { get;  set;  }

                 public long AMOUNT_BHYT { get;  set;  }
                 public long AMOUNT_VP { get;  set;  }
                 public long AMOUNT_IN { get;  set;  }
                 public long AMOUNT_TRAN { get;  set;  }
                 public long AMOUNT_BHYT_TRAN { get;  set;  }
    }


    public class SereServRdo : HIS_SERE_SERV
    {
        public long? START_TIME { get; set; }

        public long? FINISH_TIME { get; set; }

        public long? PTTT_GROUP_ID { get; set; }
    }
}
