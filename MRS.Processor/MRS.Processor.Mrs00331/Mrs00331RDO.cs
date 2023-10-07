using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00331
{
    class Mrs00331RDO
    {
        //public string REQUEST_ROOM_NAME { get;  set;  }

        //public string EXECUTE_ROOM_NAME { get;  set;  }

        //public decimal TOTAL_KHAM { get;  set;  }
        //public decimal TOTAL_BHYT { get;  set;  }
        //public decimal TOTAL_NOI_TINH { get;  set;  }
        //public decimal TOTAL_NGOAI_TINH { get;  set;  }
        //public decimal TOTAL_6_TUOI_NOI_TINH { get;  set;  }
        //public decimal TOTAL_6_TUOI_NGOAI_TINH { get;  set;  }
        //public decimal TOTAL_15_TUOI { get;  set;  }
        //public decimal TOTAL_60_TUOI { get;  set;  }
        //public decimal TOTAL_CHUYEN_VIEN { get;  set;  }
        //public decimal TOTAL_VV_NOI_TINH { get;  set;  }
        //public decimal TOTAL_VV_NGOAI_TINH { get;  set;  }

        public long EXECUTE_ROOM_ID { get;  set;  }               // mã phòng thực hiện
        public string EXECUTE_ROOM_CODE { get;  set;  }           // mã khoa thực hiện (xử lý)
        public string EXECUTE_ROOM_NAME { get;  set;  }           // tên khoa thực hiện

        public decimal TOTAL_EXAM { get;  set;  }                 //x tổng số BN khám và đã kết thúc khám tại khoa
        public decimal EXAM_IN_PROVINCE { get;  set;  }           //x số bệnh nhân nội tỉnh
        public decimal EXAM_OUT_PROVINCE { get;  set;  }          //x số bệnh nhân ngoại tỉnh
        public decimal EXAM_HEIN { get;  set;  }                  //x số bệnh nhân có bhyt

        public decimal CLINICAL_IN_PROVINCE { get;  set;  }       //x nhập viện nội tỉnh
        public decimal CLINICAL_OUT_PROVINCE { get;  set;  }      //x nhập viện ngoại tỉnh

        public decimal UNDER_6_IN_PROVINCE { get;  set;  }        //x dưới 6 tuổi nội tỉnh
        public decimal UNDER_6_OUT_PROVINCE { get;  set;  }       //x dưới 6 tuổi ngoại tỉnh

        public decimal UNDER_15 { get;  set;  }                   // dưới 15 tuổi
        public decimal OVER_6_AND_UNDER_15 { get;  set;  }        // từ 6 -> 15 tuổi
        public decimal OVER_60 { get;  set;  }                    // trên 60 tuổi

        public decimal TRAN_PATI { get;  set;  }                  // bệnh nhân được chỉ định chuyển viện

        public Mrs00331RDO() { }


        public string DISTRICT_CODE { get; set; }

        public string DISTRICT_NAME { get; set; }

        public decimal TOTAL_EXAM_CC { get; set; }

        public decimal TOTAL_EXAM_VP { get; set; }

        public decimal TOTAL_EXAM_BHYT { get; set; }

        public string TREATMENT_CODE { get; set; }
    }
}
