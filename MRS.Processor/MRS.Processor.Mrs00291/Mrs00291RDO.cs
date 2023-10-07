using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00291
{
    public class Mrs00291RDO
    {
				 public string PATIENT_CODE { get;  set;  }
				 public string PATIENT_NAME { get;  set;  }
				 public string DOB { get;  set;  }
				 public string ICD_NAME { get;  set;  }
				 public string SERVICE_NAME { get;  set;  }
				 public string ICD_TEXT { get;  set;  }
				 public string REQ_USERNAME { get;  set;  }
				 public string INSTRUCTION_TIME { get;  set;  }
				 public string ROOM_NAME { get;  set;  }
				 public string DEPARTMENT_NAME { get;  set;  }
                 public string SERVICE_TYPE_NAME { get; set; }
                 public string TDL_HEIN_SERVICE_BHYT_NAME { get; set; }
                 public string TDL_HEIN_SERVICE_BHYT_CODE { get; set; }
                 public string SERVICE_UNIT_NAME { get; set; }
				
				 public string WORK_PLACE_NAME { get;  set;  }	
				 public string ADDRESS { get;  set;  }
				 public decimal AMOUNT { get;  set;  }
				 public string PATIENT_TYPE_NAME { get;  set;  }



                 public string SERVICE_CODE { get; set; }

                 public decimal AMOUNT_NOITRU { get; set; }

                 public decimal AMOUNT_NGOAITRU { get; set; }

                 public decimal VIR_PRICE { get; set; }

                 public decimal VIR_TOTAL_PRICE { get; set; }

                 public string ROOM_CODE { get; set; }

                 public string DEPARTMENT_CODE { get; set; }

                 public string SERVICE_TYPE_CODE { get; set; }

                 public string HEIN_SERVICE_TYPE_NAME { get; set; }

                 public string HEIN_SERVICE_TYPE_CODE { get; set; }

                 public string  PTTT_GROUP_NAME { get; set; }
                 public decimal PRICE_HEIN { get; set; }
                 public decimal PRICE_NO_HEIN { get; set; }
                 public decimal PRICE_DVYC { get; set; }
                 public decimal TOTAL_USE { get; set; }
                 public decimal TOTAL_USE_BHYT { get; set; }
    }
		
}
