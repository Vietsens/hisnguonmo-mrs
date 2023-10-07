using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00468
{
    public class Mrs00468RDO
    {
        public long DEPARTMENT_ID { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }

        public long PATIENT_EX { get;  set;  }

        public long IMP_FIRST { get;  set;  }
        public long IMP_TRAN_DEPA { get;  set;  }

        public long EXP_CURED { get;  set;  }         // ra viện ổn
        public long EXP_TRANS_HOS { get;  set;  }     // chuyển viện
        public long EXP_DEATH { get;  set;  }         // chết
        public long EXP_SICKER { get;  set;  }        // nặng hơn xin về
        public long EXP_OTHER { get;  set;  }         // xuất viện khác
        public long EXP_TRAN_DEPA { get;  set;  }     // chuyển khoa

        public long PATIENT_END { get; set; }
        public long PATIENT_END_BH { get; set; }
        public long PATIENT_END_VP { get; set; }
        public long PATIENT_END_MALE { get; set; }
        public long PATIENT_END_FEMALE { get; set; }

        public double PATIENT_BED { get; set; }
        public double PATIENT_BED_YC { get; set; }
        public double PATIENT_BED_TH { get; set; }

        public Mrs00468RDO() { }

        public long EXP_TRON { get; set; }

        public string TREATMENT_CODE { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public long? CLINICAL_IN_TIME { get; set; }

        public long? DEPARTMENT_IN_TIME { get; set; }

        public long? NEXT_DEPARTMENT_IN_TIME { get; set; }

        public long? OUT_TIME { get; set; }

        public string TREATMENT_TYPE { get; set; }
    }

 
    public class DEPARTMENT_468
    {
        public long DEPARTMENT_ID { get; set; }         // id khoa đối ứng
        public string DEPARTMENT_NAME { get; set; }     // tên khoa đối ứng
        public long MAIN_DEPARTMENT_ID { get; set; }         // id khoa báo cáo
        public string MAIN_DEPARTMENT_NAME { get; set; }     // tên khoa báo cáo
        public string DEPA_IMP_KEY { get;  set;  }        // 
        public string DEPA_EXP_KEY { get;  set;  }
        public long IMP_TRAN_DEPA { get;  set;  }         // nhập từ khoa X (X là khoa được chọn lấy báo cáo (filter.DEPARTMENT_ID))
        public long EXP_TRAN_DEPA { get; set; }         // chuyển cho khoa X
        public string IMP_TRAN_DEPA_CODEs { get; set; }         // nhập từ khoa X (X là khoa được chọn lấy báo cáo (filter.DEPARTMENT_ID))
        public string EXP_TRAN_DEPA_CODEs { get; set; }         // chuyển cho khoa X
    }
}
